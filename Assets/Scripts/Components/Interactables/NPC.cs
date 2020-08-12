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

    [SerializeField]private ConversationType convoType = ConversationType.Cycle;
    [SerializeField]private Sequence[] sequences = new Sequence[1];
    private int currSeqIndex = -1;

    public override Sequence GetSequence()
    {
        return sequences[currSeqIndex];
    }

    protected override void OnInteract()
    {
        if (currSeqIndex == -1) currSeqIndex = 0;
        else if (convoType == ConversationType.Cycle)
        {
            currSeqIndex = (currSeqIndex + 1) % sequences.Length;
        }
        else if (convoType == ConversationType.Onetime)
        {
            currSeqIndex += currSeqIndex < (sequences.Length - 1) ? 1 : 0;
        }
        base.OnInteract();
    }
}
