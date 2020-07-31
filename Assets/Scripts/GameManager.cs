using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] DialogueManager dialogueManager = null;
    [SerializeField] PlayerController player = null;
    [SerializeField] ScreenFader screen = null;
    [SerializeField] Cinemachine.CinemachineVirtualCamera followCam = null;
    [SerializeField] Cinemachine.CinemachineVirtualCamera convoCam = null;

    private bool gameEventRunning = false;
    private bool isReloading = false;

    private void Awake()
    {
        Interactable.OnInteractEvent += HandleInteractEvent;
        Room.OnEnterRoom += HandleEnterRoomEvent;
        DialogueManager.OnDialogueStartEnd += HandleDialogueEvent;
        Character.OnCharacterDeath += HandleCharacterDeath;
        PlayerController.OnGameOver += HandleGameOver;
    }

    private void HandleGameOver(char type)
    {
        switch (type)
        {
            case 'H':
                print("death");
                screen.FadeToBlack();
                isReloading = true;
                break;
            case 'L':
                print("tranform");
                break;
        }
    }

    private void HandleCharacterDeath(Character character)
    {
         
    }

    private void HandleDialogueEvent(bool dialogueStart)
    {
        if (dialogueStart)
        {
            player.SetInputControllable(false);
            convoCam.enabled = true;
            followCam.enabled = false;
        }
        else if (!gameEventRunning)
        {
            player.SetInputControllable(true);
            convoCam.enabled = false;
            followCam.enabled = true;
        }
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
        if (isReloading)
        {
            if (!screen.isTransitioning())
            {
                if (screen.getOpacity() == 1)
                {
                    player.Respawn();
                    //TODO: reset rooms
                    screen.FadeIn();
                }
                else if (screen.getOpacity() == 0)
                {
                    player.PlayWakeAnim();
                    isReloading = false;
                }
            }
        }
    }
}
