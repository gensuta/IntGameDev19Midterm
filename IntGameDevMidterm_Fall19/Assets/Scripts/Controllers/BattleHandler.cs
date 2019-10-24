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

    public int tracker;
    public bool actionChosen;

    public Vector3 spawnPos;

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

        gc.playerLost = false;
        opponent = gc.storedChar;
        DetermineEndConditions();
        isPlayerTurn = true;
        opponentBP = opponent.currentBp;
        playerBP = player.bp;

        opponent.SpawnMe(spawnPos);

        if(!gc.isTutMode)
        {
            ShowGoal();
        }

        ac.BeginBattleMusic();
        
    }

    // Update is called once per frame
    void Update()
    {
        opponentBP = opponent.currentBp;
        playerBP = player.bp;

        if (!gc.isTutMode) //PLEASE REMEMBER TO SET THIS VAR TO TRUE AT THE BEG OF THE GAME!
        {
            if (!CheckIfItsOver())
            {
                if (!isPlayerTurn)
                {
                    if (gc.tutTracker == 2 || gc.tutTracker > 5)
                    {
                        if (actionTimer > 0)
                        {
                            actionTimer -= Time.deltaTime;
                        }
                        else
                        {
                            if (!actionChosen)
                            {
                                if (gc.tutTracker == 2)
                                {
                                    oa.ChooseRandomMove();
                                   
                                }
                                else
                                {
                                    int randAction = 0;
                                    randAction = Random.Range(0, 12);

                                    if (randAction > 2)
                                    {
                                        oa.ChooseRandomMove();
                                    }
                                    else
                                    {
                                        oa.BeQuirky();
                                    }
                                }
                                gc.tutTracker++;
                                actionChosen = true;
                            }
                        }
                    }
                    else
                    {
                        if (!actionChosen)
                        {
                            if (gc.tutTracker < 4)
                            {
                                gc.tutTracker++;
                            }
                            else if (gc.tutTracker == 5)
                            {
                                TutorialHandler tutC = FindObjectOfType<TutorialHandler>();
                                tutC.BeginText(tutC.tutWords[4].lines);
                                isPlayerTurn = true;
                                gc.isTutMode = true;
                            }
                            else
                            {
                                TutorialHandler tutC = FindObjectOfType<TutorialHandler>();
                                tutC.wordTracker = -1;
                                tutC.BeginText(tutC.tutWords[gc.tutTracker].lines);
                                isPlayerTurn = true;
                                gc.isTutMode = true;
                            }
                            
                        }
                    }
                }
                else
                {
                    if(gc.tutTracker == 3)
                    {
                        TutorialHandler tutC = FindObjectOfType<TutorialHandler>();
                        tutC.wordTracker = -1;
                        tutC.BeginText(tutC.tutWords[gc.tutTracker].lines);
                        isPlayerTurn = true;
                        gc.isTutMode = true;
                        gc.tutTracker++;
                    }
                }
                
            }
            else
            {
                if (!doOnce)
                {
                    if (didWin)
                    {
                        ac.PlaySFX(gc.ac.battleNoises[3]);
                        gc.opponents[gc.GetTwin(opponent._name)].isDefeated = true;
                        dh.DisplayBattleText("Battle over!\nYou didn't break down yet! :D ");

                        ac.SwitchSong(ac.hallwayMusic);
                        sc.WaitThenTransitionAndLoad("Hallway", 3f, 2);
                        doOnce = true;

                    }
                    else
                    {
                        ac.PlaySFX(gc.ac.battleNoises[4]);
                        gc.opponents[gc.GetTwin(opponent._name)].isDefeated = true;
                        gc.playerLost = true;
                        player.playerHolder.SetActive(true);
                        dh.DisplayBattleText("Battle over!\n...You're highkey panicking.");
                        sc.WaitThenTransitionAndLoad("Hallway", 3f, 2);
                        doOnce = true;
                    }


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
            goalTxt.text += "\nBut don't let them get your BP to 20 first!";
        }
        else
        {
            goalTxt.text += "\nBut don't let them get your BP to 0 first!";
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
