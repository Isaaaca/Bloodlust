using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneDirector : MonoBehaviour
{
    public static event Action<ScriptedEventSequence> OnSequenceEnd = (senquence) => { };

    [SerializeField]private ScreenFader screen = null;
    [SerializeField]private DialogueManager dialogueManager = null;
    private int index = -1;
    private ScriptedEventSequence currentSequence = null;
    private ScriptedEvent.EventType currEventType = ScriptedEvent.EventType.Null;
    private ScriptedEvent currEvent = null;
    private bool waiting = true;

    private void Awake()
    {
        DialogueManager.OnDialogueStartEnd += HandleDialogueEvent;
    }



    // Update is called once per frame
    void Update()
    {
        if (!waiting)
        {
            if (index >= currentSequence.scriptedEvents.Length)
            {
                //End Sequence
                OnSequenceEnd(currentSequence);
            }
            else
            {
                currEvent = currentSequence.scriptedEvents[index];
                currEventType = currEvent.eventType;

                switch (currEventType)
                {
                    case ScriptedEvent.EventType.Dialogue:
                        dialogueManager.LoadDialogue(currEvent.dialogue);
                        waiting = true;
                        break;
                    case ScriptedEvent.EventType.Fade:
                        screen.CustomFade(currEvent.opacity, currEvent.duration);
                        waiting = true;
                        break;
                }
            }

        }
        else if (currEventType == ScriptedEvent.EventType.Fade)
        {
            if (!screen.isTransitioning())
            {
                waiting = false;
                index++;
            }
        }
    }

    public void PlaySequence(ScriptedEventSequence eventSequence)
    {
        currentSequence = eventSequence;
        index = 0;
        waiting = false;
    }

    private void HandleDialogueEvent(bool start)
    {
        if (currEventType == ScriptedEvent.EventType.Dialogue && !start)
        {
            waiting = false;
            index++;
        }
    }
}
