using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] DialogueManager dialogueManager = null;
    [SerializeField] PlayerController player = null;
    [SerializeField] ScreenFader screen = null;
    [SerializeField] CamController camController = null;
    [SerializeField] CutsceneDirector cutsceneDirector = null;
    [SerializeField] EventSequenceDictionary eventDictionary = null;

    private bool gameEventRunning = false;
    private bool isReloading = false;

    private void Awake()
    {
        Interactable.OnInteractEvent += HandleInteractEvent;
        Room.OnEnterRoom += HandleEnterRoomEvent;
        DialogueManager.OnDialogueStartEnd += HandleDialogueEvent;
        Character.OnCharacterDeath += HandleCharacterDeath;
        PlayerController.OnGameOver += HandleGameOver;
        CutsceneDirector.OnSequenceEnd += HandleSequenceEnd;
    }

    private void HandleSequenceEnd(ScriptedEventSequence sequence)
    {
        //TODO: check for continued event/conditional dialogue
        screen.FadeIn();
        gameEventRunning = false;
        camController.SwitchCamera(CamController.CameraMode.Follow);
        player.SetInputControllable(true);
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
            camController.SwitchCamera(CamController.CameraMode.Center);
        }
        else if (!gameEventRunning)
        {
            player.SetInputControllable(true);
            camController.SwitchCamera(CamController.CameraMode.Follow);
        }
    }

    private void HandleEnterRoomEvent(Room room)
    {
        print(room.name);
        CheckForScriptedEvent(room.name);
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

    private void CheckForScriptedEvent(string key)
    {
        if (eventDictionary.ContainsKey(key))
        {
            cutsceneDirector.PlaySequence(eventDictionary[key]);
            gameEventRunning = true;
        }
    }
}
