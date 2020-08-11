using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : Interactable
{
    public enum ConversationType
    {
        Cycle,
        Onetime
    }

    [SerializeField] private ConversationType convoType = ConversationType.Cycle;
    [SerializeField]private Dialogue[] dialogues;
    [SerializeField]private int currDialogue = 0;

    public Dialogue GetDialogue()
    {
        return dialogues[currDialogue];
    }

    protected override void OnInteract()
    {
        base.OnInteract();
        if (convoType == ConversationType.Cycle)
        {
            currDialogue = (currDialogue + 1) % dialogues.Length;
        }
        else if (convoType == ConversationType.Onetime)
        {
            currDialogue += currDialogue < (dialogues.Length - 1) ? 1 : 0;
        }
    }
}
