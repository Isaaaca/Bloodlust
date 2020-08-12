using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : Interactable
{
    private Sequence sequence = null;
    [SerializeField] SwitchControllableObject controlledObject = null;
    public void Awake()
    {
        //create sequence
        sequence = ScriptableObject.CreateInstance<Sequence>();
        SequenceEvent seqEvent = new SequenceEvent();
        seqEvent.eventType = SequenceEvent.EventType.Switchable;
        seqEvent.switchable = controlledObject;
        sequence.scriptedEvents[0] = seqEvent;
    }
    public override Sequence GetSequence()
    {
        return sequence;
    }

}
