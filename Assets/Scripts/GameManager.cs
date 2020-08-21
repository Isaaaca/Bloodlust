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
    [SerializeField] LevelEndMenu levelEndMenu = null;
    [SerializeField] string lastCondition = "";
    [SerializeField] GameEventDictionary eventDictionary = null;

    public static event Action<bool> SetGameplayEnabled = (enable) => { };


    private bool gameEventRunning = false;
    private bool isReloading = false;
    private string currRoom = "";

    private void Awake()
    {
        Interactable.OnInteractEvent += HandleInteractEvent;
        Room.OnEnterRoom += HandleEnterRoomEvent;
        Character.OnCharacterDeath += HandleCharacterDeath;
        PlayerController.OnGameOver += HandleGameOver;
        CutsceneDirector.OnSequenceEnd += HandleSequenceEnd;
        DialogueManager.OnDialogueEvent += HandleDialogueEvent;
        AreaTrigger.OnEnterAreaTrigger += HandleTrigger;
    }

    private void HandleTrigger(string triggerCode)
    {
        CheckForScriptedEvent(triggerCode);
    }

    private void HandleDialogueEvent(string eventCode)
    {
        SaveManager.AddEvent(eventCode);
        CheckForScriptedEvent(eventCode);
    }

    private void HandleSequenceEnd(Sequence sequence)
    {
        //TODO: check for continued event/conditional dialogue
        SaveManager.AddEvent(sequence.name);
        screen.FadeIn();
        gameEventRunning = false;
        camController.SwitchCamera(CamController.CameraMode.Follow);
        player.SetInputControllable(true);
        CheckForScriptedEvent(sequence.name);
    }

    private void HandleGameOver(char type)
    {
        string code = type + currRoom;
        SaveManager.IncrementCounter(code);
        screen.FadeToBlack();
        isReloading = true;
        CheckForScriptedEvent(code);
    }

    private void HandleCharacterDeath(Character character)
    {
         
    }

    private void HandleEnterRoomEvent(Room room)
    {
        currRoom = room.name;
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
        if (isReloading &&! gameEventRunning)
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
        else if (key == lastCondition)
        {
            isReloading = false;
            screen.FadeToBlack();
            player.enabled = false;
            //Load Level End screen
            levelEndMenu.ShowMenu();
        }
    }


    private void StartSequence(Sequence sequence)
    {
        player.SetInputControllable(false);
        cutsceneDirector.PlaySequence(sequence);

    }
}
