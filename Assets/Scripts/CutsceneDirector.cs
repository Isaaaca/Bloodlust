using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneDirector : MonoBehaviour
{
    public static event Action<Sequence> OnSequenceEnd = (senquence) => { };

    [SerializeField]private ScreenFader screen = null;
    [SerializeField]private DialogueManager dialogueManager = null;
    private int index = -1;
    private Sequence currentSequence = null;
    private SequenceEvent.EventType currEventType = SequenceEvent.EventType.Null;
    private SequenceEvent currEvent = null;
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
                    case SequenceEvent.EventType.Dialogue:
                        dialogueManager.LoadDialogue(currEvent.dialogue);
                        waiting = true;
                        break;
                    case SequenceEvent.EventType.Fade:
                        screen.CustomFade(currEvent.opacity, currEvent.duration);
                        waiting = true;
                        break;
                    case SequenceEvent.EventType.Switchable:
                        currEvent.switchable.OnSwitch();
                        waiting = true;
                        Invoke("StopWaiting", currEvent.switchable.GetDuration());
                        break;
                }
            }

        }
        else if (currEventType == SequenceEvent.EventType.Fade)
        {
            if (!screen.isTransitioning())
            {
                StopWaiting();
            }
        }
    }

    public void PlaySequence(Sequence eventSequence)
    {
        currentSequence = eventSequence;
        index = -1;
        StopWaiting();
    }

    private void HandleDialogueEvent(bool start)
    {
        if (currEventType == SequenceEvent.EventType.Dialogue && !start)
        {
            StopWaiting();
        }
    }

    private void StopWaiting()
    {
        waiting = false;
        index++;
    }
}
