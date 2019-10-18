using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractBehavior : MonoBehaviour
{
    DialogueHandler dh;
    GameController gc;
    public Character character;
    public string _name;

    float waitTime = 0.5f; // allow dialogue handler to find the CORRECT audio
    bool dialogueStarted;

    // Start is called before the first frame update
    void Start()
    {
        dh = FindObjectOfType<DialogueHandler>();
        gc = FindObjectOfType<GameController>();
        _name = character._name;

        if (gc.CheckIfGone(_name))
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (gc.CheckIfDefeated(_name) && !dh.isActive)
        {
            waitTime -= Time.deltaTime;
            if(waitTime <= 0f)
            {
                if (dialogueStarted)
                {
                    Destroy(gameObject);
                }
                else
                {
                    StartConversation(this,gc.playerLost);
                    dialogueStarted = true;
                }
            }
            
        }
    }

    public void StartConversation(InteractBehavior interact, bool playerLost)
    {
        dh.nameTxt.text = interact.character._name;
        gc.storedChar = Instantiate(interact.character); // this is so we never DIRECTLY edit the object in the folder during the battle
        switch (interact.character._type)
        {
            case (GameController.Types.tiredness):
                dh.whichVoice = 0;
                break;
            case (GameController.Types.kindness):
                dh.whichVoice = 1;
                break;
            case (GameController.Types.liveliness):
                dh.whichVoice = 2;
                break;
        }

        dh.currentLine = 0;
        dh.label.SetActive(false);
        dh.myLines.Clear();
        if (!playerLost)
        {
            foreach (string line in interact.character.defeatLines)
            {
                dh.myLines.Add(line);

            }
        }
        else
        {
            foreach (string line in interact.character.winLines)
            {
                dh.myLines.Add(line);

            }
        }

        dh.isShowingDefeat = true;
        dh.EnableTextBox();
    }
}
