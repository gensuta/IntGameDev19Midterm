using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialHandler : MonoBehaviour
{
    public TutText[] tutWords;
    public string[] currentString;


    int wordTracker;

    GameController gc;
    BattleHandler bh;
    DialogueHandler dh;

    public GameObject[] actionButtons;

    // Start is called before the first frame update
    void Start()
    {
        gc = FindObjectOfType<GameController>();
        bh = FindObjectOfType<BattleHandler>();
        dh = FindObjectOfType<DialogueHandler>();
    }

    // Update is called once per frame
    void Update()
	{
		if (!dh.isActive && gc.isTutMode)
		{
            BeginText(tutWords[gc.tutTracker].lines);
			if (wordTracker < currentString.Length)
			{
				dh.DisplayBattleText(currentString[wordTracker]);
				wordTracker++;
			}
			else
			{
                TurnOnButtons();
				gc.isTutMode = false;
                gc.tutTracker++;
			}
		}
	}

    void TurnOnButtons()
    {
        if(gc.tutTracker == 0)
        {
            foreach(GameObject g in actionButtons)
            {
                g.SetActive(false);
            }
            actionButtons[0].SetActive(true);
        }
    }

    void BeginText(string[] c)
    {
        wordTracker = 0;
        currentString = c;
    }

    [System.Serializable]
    public class TutText
    {
        [TextArea(2, 5)]
        public string[] lines;
    }
}

