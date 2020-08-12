using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] PlayerController player = null;
    [SerializeField] ScreenFader screen = null;
    [SerializeField] CamController camController = null;
    [SerializeField] CutsceneDirector cutsceneDirector = null;
    [SerializeField] GameEventDictionary eventDictionary = null;

    public static event Action<bool> SetGameplayEnabled = (enable) => { };


    private bool gameEventRunning = false;
    private bool isReloading = false;

    private void Awake()
    {
        Interactable.OnInteractEvent += HandleInteractEvent;
        Room.OnEnterRoom += HandleEnterRoomEvent;
        Character.OnCharacterDeath += HandleCharacterDeath;
        PlayerController.OnGameOver += HandleGameOver;
        CutsceneDirector.OnSequenceEnd += HandleSequenceEnd;
        DialogueManager.OnDialogueEvent += HandleDialogueEvent;
    }

    private void HandleDialogueEvent(string eventCode)
    {
        SaveManager.AddEvent(eventCode);
    }

    private void HandleSequenceEnd(Sequence sequence)
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

    private void HandleEnterRoomEvent(Room room)
    {
        CheckForScriptedEvent(room.name);
    }

    private void HandleInteractEvent(string id, Interactable interactable)
    {
        Sequence seq = interactable.GetSequence();
        if (seq != null) StartSequence(seq);
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
            Sequence seq = eventDictionary[key].GetSequence();
            if (seq != null)
            {
                StartSequence(seq);
                gameEventRunning = true;
            }
        }
    }


    private void StartSequence(Sequence sequence)
    {
        player.SetInputControllable(false);
        cutsceneDirector.PlaySequence(sequence);

    }
}
