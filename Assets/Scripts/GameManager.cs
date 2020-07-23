using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] DialogueManager dialogueManager = null;

    private void Awake()
    {
        Interactable.OnInteractEvent += HandleInteractEvent;
        Room.OnEnterRoom += HandleEnterRoomEvent;
    }

    private void HandleEnterRoomEvent(Room room)
    {
        print(room.name);
    }

    private void HandleInteractEvent(string id, Interactable interactable)
    {
        string type = id.Substring(0,3);
        switch (type)
        {
            case "OBJ":
                ObservableObject obj = interactable as ObservableObject;
                dialogueManager.LoadDialogue(obj.message);
                break; 
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
