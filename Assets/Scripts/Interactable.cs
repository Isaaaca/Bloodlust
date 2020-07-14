using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{

    public static event Action<Dialogue> OnInteractEvent = (dialogue) => { };
    
    private bool interactable = false;
    [SerializeField]private Dialogue message = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (interactable)
        {
            if (Input.GetKeyDown(KeyCode.R)) OnInteractEvent(message);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name.Equals("Player"))
        {
            interactable = true;   
        }
    }
}
