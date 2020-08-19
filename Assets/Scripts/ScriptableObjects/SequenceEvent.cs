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
        CameraPanTo,
        CameraJumpTo,
        Teleport,
        TeleportRelative,
        Null
    }

    public EventType eventType;
    public SwitchControllableObject switchable;
    public Dialogue dialogue;
    public string target;
    public Transform targetTransform;
    public float duration;
    public float opacity;
    public Vector2 position;
   
}
