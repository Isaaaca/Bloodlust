using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObservableObject : Interactable
{


    public ScriptedEventSequence sequence = null;
    // Start is called before the first frame update
    void Start()
    {
        id = gameObject.name;
    }

}
