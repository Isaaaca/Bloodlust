using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class testing : MonoBehaviour
{
    public DialogueManager dm;
    public Dialogue d;
    private void Start()
    {
        dm.LoadDialogue(d);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
   
}
