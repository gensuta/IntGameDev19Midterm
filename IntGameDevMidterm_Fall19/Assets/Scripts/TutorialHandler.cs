using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialHandler : MonoBehaviour
{
    public TutText[] tutWords;
    public string[] currentStrings;


    public int wordTracker;

    GameController gc;
    BattleHandler bh;
    DialogueHandler dh;

    public GameObject[] actionButtons;

    public GameObject opponentSymbol;

	// Start is called before the first frame update
	void Start()
	{
		gc = FindObjectOfType<GameController>();
		bh = FindObjectOfType<BattleHandler>();
		dh = FindObjectOfType<DialogueHandler>();

		if (!dh.isActive && gc.isTutMode)
		{
            opponentSymbol.SetActive(false);
			BeginText(tutWords[gc.tutTracker].lines);
		}
	}

    // Update is called once per frame
    void Update()
	{
		if (!dh.isActive && gc.isTutMode && gc.tutTracker < 6)
		{
			if (wordTracker < currentStrings.Length)
			{
				dh.DisplayBattleText(currentStrings[wordTracker]);
				wordTracker++;
			}
			else
			{
                TurnOnThings();
				gc.isTutMode = false;
			}
		}
	}

	public void BeginText(string[] c)
	{
		currentStrings = c;
        wordTracker = 0;
    }

	void TurnOnThings()
    {
        if(gc.tutTracker == 0)
        {
            foreach(GameObject g in actionButtons)
            {
                g.SetActive(false);
            }
            actionButtons[0].SetActive(true);
        }
        else if (gc.tutTracker == 1)
        {
            PlayerActions player = FindObjectOfType<PlayerActions>();
            player.playerHolder.SetActive(true);
            opponentSymbol.SetActive(true);
        }
        else if (gc.tutTracker == 2)
        {
            PlayerActions player = FindObjectOfType<PlayerActions>();
            player.playerHolder.SetActive(true);
            foreach (GameObject g in actionButtons)
            {
                g.SetActive(false);
            }
            actionButtons[2].SetActive(true);
        }
        else if (gc.tutTracker == 3)
        {
            bh.isPlayerTurn = true;
            PlayerActions player = FindObjectOfType<PlayerActions>();
            player.playerHolder.SetActive(true);

            foreach (GameObject g in actionButtons)
            {
                g.SetActive(false);
            }
            actionButtons[1].SetActive(true);
        }
        else if (gc.tutTracker == 4)
        {
            bh.isPlayerTurn = true;
            PlayerActions player = FindObjectOfType<PlayerActions>();
            player.playerHolder.SetActive(true);
            
            foreach (GameObject g in actionButtons)
            {
                g.SetActive(false);
            }
            actionButtons[1].SetActive(true);
        }
        else
        {
            bh.opponent.currentBp = 0;
            PlayerActions player = FindObjectOfType<PlayerActions>();
            player.playerHolder.SetActive(true);
            foreach (GameObject g in actionButtons)
            {
                g.SetActive(true);
            }
            gc.tutTracker = 6;
        }
    }

   

    [System.Serializable]
    public class TutText
    {
        [TextArea(2, 5)]
        public string[] lines;
    }
}

