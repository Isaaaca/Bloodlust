using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test: MonoBehaviour
{
    public DialogueManager dm;
    public Dialogue d;
    // Start is called before the first frame update
    private void Start()
    {
        dm.LoadDialogue(d);
        
    }

    public static void poop()
    {
        Debug.Log("poop");
    }
}
