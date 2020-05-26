using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu]
public class Dialogue : ScriptableObject
{
    [Serializable]
    public class DialogueLine
    {
        public string speaker = "";
        public string text = "";
        public string[] options;

    }

    public int index;
    public DialogueLine[] dialogueLines;
}
