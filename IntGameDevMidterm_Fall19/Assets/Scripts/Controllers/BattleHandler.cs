using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleHandler : MonoBehaviour
{
    GameController gc;
    AudioController ac;
    SceneController sc;

    OpponentActions oa;

    bool doOnce;
    bool didWin;

    int opponentBP;
    int desiredBP;
    int playerBP;


    public PlayerActions player;
    public int unwantedBP; // the bp amt that will make the player lose

    public bool isIncreasingBp; // are we, the player, taking down someone's bp or increasing it?
    public Character opponent;
    public bool isPlayerTurn;

    public float actionTimer = 1.5f; //here to slow shit down!!


    // Start is called before the first frame update
    void Awake()
    {
        
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
                        int randAction = 0;
                        randAction = Random.Range(0, 6);
                        if(randAction > 2)
                        {
                            oa.ChooseRandomMove();
                        }
                        else
                        {
                            oa.BeQuirky();
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
                        Debug.Log("Battle over! You didn't break down yet!");
                        sc.WaitThenLoad("Hallway", 1f, 2);
                    }
                    else
                    {
                        Debug.Log("Somethings coming.");
                        Debug.Log("I don't think you got this one today, and that's ok.");
                        Debug.Log("But brace yourself. I know you don't like to lose");
                        sc.WaitThenLoad("Boss Battle", 1f);
                    }
                    doOnce = true;
                }
               
            }

        }
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
