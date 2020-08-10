using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static event Action<bool> OnDialogueStartEnd = (active) => { };

    [SerializeField] private CamController camController = null;
    [SerializeField] private TextMeshProUGUI nameText = null;
    [SerializeField] private TextMeshProUGUI mainText = null;
    [SerializeField] private TextMeshProUGUI[] optionTexts = null;
    [SerializeField] private Image[] selectionImages = null;
    [SerializeField] private GameObject dialogueBox = null;

    private Dialogue dialogue;
    private int currLine = 0;
    private int currSelection = 0;
    private int numOpt = 0;
    private Dictionary<string,Transform> cameraTargets = new Dictionary<string, Transform>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (dialogueBox.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Space)) NextLine();
            if (Input.GetKeyDown(KeyCode.DownArrow)) ChangeSelection(true);
            if (Input.GetKeyDown(KeyCode.UpArrow)) ChangeSelection(false);
        }
    }

    public void LoadDialogue(Dialogue dialogue)
    {
        this.dialogue = dialogue;
        GetSpeakers(dialogue);
        currLine = 0;
        UpdateDialogueDisplay();
        dialogueBox.SetActive(true);
        OnDialogueStartEnd(true);
    }

    private void GetSpeakers(Dialogue dialogue)
    {
        cameraTargets.Clear();
        foreach (Dialogue.DialogueLine  line in dialogue.dialogueLines)
        {
            string target = line.lookAt;
            if (target == "")
                target = line.speaker;

            if (target!="" && !cameraTargets.ContainsKey(target))
            {
                var gameObj = GameObject.Find(target);
                if(gameObj != null)
                {
                    cameraTargets.Add(target, gameObj.transform);
                }
            }
        }
    }

    public void NextLine()
    {
        //TODO: send dialogue events
        print(dialogue.index.ToString()+ currLine.ToString() + currSelection.ToString());

        currLine++;
        if (currLine >= dialogue.dialogueLines.Length)
        {
            //end convo
            dialogueBox.SetActive(false);
            OnDialogueStartEnd(false);

        }
        else
        {
            UpdateDialogueDisplay();
        }
    }

    private void ChangeSelection(bool increment)
    {
        if (increment && currSelection < numOpt)
        {
            currSelection++;
            UpdateSelectionUI();
        }
        else if(!increment && currSelection > 1)
        {
            currSelection--;
            UpdateSelectionUI();
        }
    }

    private void UpdateSelectionUI()
    {
        for (int i = 0; i < 3; i++)
        {
            if (i == currSelection-1) selectionImages[i].enabled = true;
            else selectionImages[i].enabled = false;
        }
    }

    private void UpdateDialogueDisplay()
    {
        string target = dialogue.dialogueLines[currLine].lookAt;
        if (target == "")
            target = dialogue.dialogueLines[currLine].speaker;

        if (target!="" && cameraTargets[target] != null)
            camController.ChangeFollowTarget(cameraTargets[target]);

        nameText.text = dialogue.dialogueLines[currLine].speaker;
        mainText.text = dialogue.dialogueLines[currLine].text;
        numOpt = dialogue.dialogueLines[currLine].options.Length;
        if (numOpt > 0)
        {
            for (int i = 0; i < numOpt; i++)
            {
                optionTexts[i].text = dialogue.dialogueLines[currLine].options[i];
            }
            currSelection = 1;

        }
        else currSelection = 0;
        for (int i = numOpt; i < 3; i++)
        {
            optionTexts[i].text = "";
        }
        UpdateSelectionUI();
    }
}
