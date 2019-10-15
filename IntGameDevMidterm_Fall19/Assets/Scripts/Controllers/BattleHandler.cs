using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleHandler : MonoBehaviour
{
    GameController gc;
    AudioController ac;
    SceneController sc;
    public DialogueHandler dh;
    OpponentActions oa;

    bool doOnce;
    bool didWin;

    int opponentBP;
    int desiredBP;
    int playerBP;

    public TextMeshProUGUI goalTxt;
    public Animator goalAnim;

    public PlayerActions player;
    public int unwantedBP; // the bp amt that will make the player lose
    public bool isIncreasingBp; // are we, the player, taking down someone's bp or increasing it?
    public Character opponent;


    public bool isPlayerTurn;
    public float actionTimer = 1.5f; //here to slow shit down!!

    [TextArea(2,5)]
    public string[] loseWords;
    [TextArea(2, 5)]
    public string[] tutWords;
    public int tracker;
    public bool actionChosen;

    // Start is called before the first frame update
    void Awake()
    {
        tracker = 0;
        dh = FindObjectOfType<DialogueHandler>();
        player = FindObjectOfType<PlayerActions>();
        oa = FindObjectOfType<OpponentActions>();
        gc = FindObjectOfType<GameController>();
        ac = FindObjectOfType<AudioController>();
        sc = FindObjectOfType<SceneController>();

        opponent = gc.storedChar;
        DetermineEndConditions();
        isPlayerTurn = true;
        opponentBP = opponent.currentBp;
        playerBP = player.bp;

        if(!gc.isFirstBattle)
        {
            ShowGoal();
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        opponentBP = opponent.currentBp;
        playerBP = player.bp;

        if (!gc.isFirstBattle) //PLEASE REMEMBER TO SET THIS VAR TO TRUE AT THE BEG OF THE GAME!
        {
            if (!CheckIfItsOver())
            {
                if (!isPlayerTurn)
                {
                    if(actionTimer > 0)
                    {
                        actionTimer -= Time.deltaTime;
                    }
                    else
                    {
                        if (!actionChosen)
                        {
                            int randAction = 0;
                            randAction = Random.Range(0, 10);

                            if (randAction > 2)
                            {
                                Debug.Log("hm");
                                oa.ChooseRandomMove();
                            }
                            else
                            {
                                Debug.Log("fuck");
                                oa.BeQuirky();
                            }
                            actionChosen = true;
                        }
                    }
                }
            }
            else
            {
                if (!doOnce)
                {
                    if (didWin)
                    {
                        //gc.opponents[gc.GetTwin(opponent._name)].isDefeated = true;
                        gc.opponents[gc.GetTwin(opponent._name)].isDefeated = true;
                        dh.DisplayBattleText("Battle over!\nYou didn't break down yet! :D ");
                        sc.WaitThenTransitionAndLoad("Hallway", 3f, 2);
                        doOnce = true;
                    }
                    else
                    {
                        if (!dh.isActive)
                        {
                            if (tracker < loseWords.Length)
                            {
                                dh.DisplayBattleText(loseWords[tracker]);
                                tracker++;
                            }
                            else
                            {
                                sc.WaitThenLoad("Boss Battle", 1.5f,2);
                            }
                        }
                    }
                    
                }
               
            }
        }
        else
        {
            Debug.Log(tutWords.Length);
            if (!dh.isActive)
            {
                if (tracker < tutWords.Length)
                {
                    dh.DisplayBattleText(tutWords[tracker]);
                    tracker++;
                }
                else
                {
                    tracker = 0;
                    gc.isFirstBattle = false;
                    ShowGoal();
                }
            }
        }
    }

    void ShowGoal()
    {
        goalAnim.gameObject.SetActive(true);
        if(desiredBP >0)
        {
            goalTxt.text = "MAKE THAT BOND!\nIncrease their bond points to 20!";
        }
        else
        {
            goalTxt.text = "BREAK THE BOND!\nDecrease their bond points to 0!";
        }


        if (unwantedBP > 0)
        {
            goalTxt.text += "\nBut don't let them make a bond with you...";
        }
        else
        {
            goalTxt.text += "\nBut don't let them break the bond they have with you...";
        }
        goalAnim.SetBool("canStart", true);
    }

    bool CheckIfItsOver()
    {
        bool isOver = false;
        if (opponent.bondWanted)
        {
            if(opponentBP >= desiredBP)
            {
                didWin = true;
                return true;
            }
        }
        else
        {
            if (opponentBP <= desiredBP)
            {
                didWin = true;
                return true;
            }
        }

        //what you dont want for yourself
        if (opponent.willBreakBond)
        {
            if(playerBP <= unwantedBP)
            {
                return true;
            }
        }
        else
        {
            if (playerBP >= unwantedBP)
            {
                return true;
            }
        }
        return isOver;
    }

    void DetermineEndConditions()
    {
        //what you want for your opponent
        if (opponent.bondWanted)
        {
            desiredBP = 20;
        }
        else
        {
            desiredBP = 0;
        }

        //what you dont want for yourself
        if (opponent.willBreakBond)
        {
            unwantedBP = 0;
        }
        else
        {
            unwantedBP = 20;
        }
    }
}
