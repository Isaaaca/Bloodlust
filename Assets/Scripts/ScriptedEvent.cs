using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ScriptedEvent
{
    public enum EventType
    {
        Dialogue,
        Fade,
        Null
    }

    public EventType eventType;
    public Dialogue dialogue;
    public float duration;
    public float opacity;
   
}
