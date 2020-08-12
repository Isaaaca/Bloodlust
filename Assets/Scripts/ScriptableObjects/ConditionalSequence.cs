using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GameEvent/Conditional Sequence")]
public class ConditionalSequence : BaseGameEvent
{
    [SerializeField] private Sequence sequence = null;
    [SerializeField] private string condition ="";
    public override Sequence GetSequence()
    {
        if (SaveManager.CheckCondition(condition))
            return sequence;
        else
            return null;
    }
}
