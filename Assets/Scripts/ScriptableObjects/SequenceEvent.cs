using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SequenceEvent
{
    public enum EventType
    {
        Dialogue,
        Fade,
        Switchable,
        Null
    }

    public EventType eventType;
    public SwitchControllableObject switchable;
    public Dialogue dialogue;
    public float duration;
    public float opacity;
   
}
