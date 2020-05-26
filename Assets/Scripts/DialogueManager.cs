using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private Text nameText = null;
    [SerializeField] private Text mainText = null;
    [SerializeField] private Text[] optionTexts = null;
    [SerializeField] private Image[] selectionImages = null;

    private Dialogue dialogue;
    private int currLine = 0;
    private int currSelection = 0;
    private int numOpt = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) NextLine();
        if (Input.GetKeyDown(KeyCode.DownArrow))ChangeSelection(true);
        if (Input.GetKeyDown(KeyCode.UpArrow))ChangeSelection(false);
    }

    public void LoadDialogue(Dialogue dialogue)
    {
        this.dialogue = dialogue;
        currLine = 0;
        UpdateTexts();
    }

    public void NextLine()
    {
        //send dialogue events
        print(dialogue.index.ToString()+ currLine.ToString() + currSelection.ToString());
        currLine++;
        if (currLine >= dialogue.dialogueLines.Length)
        {
            //end convo
        }
        else
        {
            UpdateTexts();
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

    private void UpdateTexts()
    {
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
