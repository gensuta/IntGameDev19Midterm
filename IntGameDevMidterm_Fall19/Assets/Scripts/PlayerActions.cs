using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerActions : MonoBehaviour // BATTLE!! ACTIONS!!
{
    BattleHandler bh;
    CameraHandler ch;
    AudioController ac;
    DialogueHandler dh;

    public Personas[] myPersonas;
    public Moves[] myMoves;
    public int currentPersona;

    float storedBp;
    float storedMp;

    [Space]
    public int bp;
    public int mp;
    public float lerpSpeed = 0.25f; // for slider

    [Space]
    [Header("UI mess")]
    public GameObject backButton;
    public TextMeshProUGUI bpTxt;
    public TextMeshProUGUI mpTxt;
    public Slider bpSlider;
    public Slider mpSlider;

    [Space]
    public GameObject actionHolder;
    public GameObject moveHolder;
    public GameObject personaHolder;
    public GameObject personaButton;
    public GameObject playerHolder;

    bool waitForText;
    bool madeMove;
    Moves storedMove;

    public Vector3 personaPos;
    public GameObject currentPersonaObj;


    // Start is called before the first frame update
    void Awake()
    {
        if (GameController.gc.lastUsed == null)
        {
            GameController.gc.lastUsed = myPersonas[0];
        }
        else
        {
           int i = 0;
           foreach(Personas p in myPersonas)
            {
                if(GameController.gc.lastUsed._name == p._name)
                {
                    currentPersona = i;
                }
                i++;
            }
        }
    }

    void Start()
    {
        dh = FindObjectOfType<DialogueHandler>();
        ac = FindObjectOfType<AudioController>();
        ch = FindObjectOfType<CameraHandler>();
        bh = FindObjectOfType<BattleHandler>();

        bpSlider.maxValue = 20;
        mpSlider.maxValue = 20;

        mp = 20;
 

        if (bh.opponent.willBreakBond)
        {
            bp = 20;
        }
        else
        {
            bp = 0;
        }

        storedBp = bp;
        storedMp = mp;
        if (bh.opponent._name == "Bubbly" && bh.opponent._name == "Shy" && bh.opponent._name == "RAW")
        {
            currentPersonaObj = myPersonas[currentPersona].SpawnMe(personaPos, this);
            ShowPersonaFX();
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateUI();


        if(bp > 20)
        {
            bp = 20;
        }
        else if (bp < 0)
        {
            bp = 0;
        }

        if (mp > 20)
        {
            mp = 20;
        }
        else if (mp < 0)
        {
            mp = 0;
        }

        if (waitForText)
        {
            if(!dh.isActive)
            {
                ch.ChangeCamAnim(0);
                waitForText = false;
            }
        }

        if (Input.GetKey(KeyCode.Alpha0))
        {
            if(Input.GetKeyDown(KeyCode.M))
            {
                BackToMainMenu();
            }
        }

        if(madeMove && !dh.isActive)
        {
            if (storedMove.onSelf)
            {
                dh.DisplayBattleText("You used " + storedMove._name + "!");
            }
            else
            {
                dh.DisplayBattleText("You used " + storedMove._name + " on " + bh.opponent._name);
            }
            
            EndPlayerTurn();
        }
    }

    void ShowPersonaFX()
    {
        Vector3 psPos = new Vector3(-1.19f, 0, -8.27f);
        Instantiate(myPersonas[currentPersona].ps,psPos,transform.rotation,currentPersonaObj.transform);
    }

    void UpdateUI()
    {
        storedMp = Mathf.Lerp(storedMp, mp, lerpSpeed * Time.deltaTime);
        storedBp = Mathf.Lerp(storedBp, bp, lerpSpeed * Time.deltaTime);
        bpTxt.text = "Bond Points: " + bp;
        mpTxt.text = "Mindful Points: " + mp;
        bpSlider.value = storedBp;
        mpSlider.value = storedMp;
    }


    public void SetMoves()
    {
        if (mp > 0)
        {
            actionHolder.SetActive(false);
            moveHolder.SetActive(true);
            int whichMove = 0;
            foreach (Button button in moveHolder.GetComponentsInChildren<Button>())
            {
                if (bh.opponent._name != "A new friend" && bh.opponent._name != "A cool person")
                {
                    button.GetComponentInChildren<TextMeshProUGUI>().text = myPersonas[currentPersona].moves[whichMove]._name;
                    whichMove++;
                }
                else
                {
                    button.GetComponentInChildren<TextMeshProUGUI>().text = myMoves[whichMove]._name;
                    whichMove++;
                }
            }
        }
        else
        {
            backButton.SetActive(false);
            dh.DisplayBattleText("Hey, don't forget to <i>breathe</i>. Your MP is low.");
        }
    }

   

    public void ShowPersonas()
    {
        backButton.SetActive(true);
        actionHolder.SetActive(false);
        personaHolder.SetActive(true);
        int whichP = 0;
        ch.ChangeCamAnim(3);

        foreach (Button button in personaHolder.GetComponentsInChildren<Button>())
        {
            button.GetComponentInChildren<TextMeshProUGUI>().text = myPersonas[whichP]._name;
            whichP++;

        }
    }

    // the four actions that spend up a turn

    public void ChangePersona(int whichPersona)
    {
        if(whichPersona == currentPersona)
        {
            backButton.SetActive(false);
            dh.DisplayBattleText("Silly! You have that persona equipped already!");
        }
        else
        {
            if (bh.opponent._name != "A new friend" && bh.opponent._name != "A cool person")
            {
                ChangeForReal(whichPersona);
            }
            else
            {
                if(myPersonas[whichPersona].name == "RAW")
                {
                    backButton.SetActive(false);
                    dh.DisplayBattleText("They CAN NOT interact with this one.");
                }
            }
           
        }

    }


    void ChangeForReal(int whichPersona)
    {
        backButton.SetActive(false);
        ac.PlaySFX(ac.battleNoises[2]);
        ac.FadeOut();
        Destroy(currentPersonaObj);
        currentPersona = whichPersona;
        currentPersonaObj = myPersonas[whichPersona].SpawnMe(personaPos, this);

        GameController.gc.lastUsed = myPersonas[currentPersona];
        dh.DisplayBattleText("You're new persona is <color=red>" + myPersonas[currentPersona]._name + "</color>");
        ShowPersonaFX();
        EndPlayerTurn();
    }

    public void Breathe() // restore mp
    {
        if(mp < 20)
        {
            int randNum = Random.Range(3, 7);
            mp += randNum;
            dh.DisplayBattleText("Breathing restored " + randNum + " MP!");

            playerHolder.SetActive(false);
            EndPlayerTurn();
        }
        else
        {
            dh.DisplayBattleText("Silly! Your MP is already full!");

        }
    }

    public void Stare() // restore bp
    {
        if(GameController.gc.tutTracker == 4)
        {
            GameController.gc.tutTracker = 5;
        }

        if(bh.unwantedBP == 0)
        {
            if(bp < 20)
            {
                int randNum = Random.Range(3, 6);
                bp += randNum;
                dh.DisplayBattleText("Listening restored " + randNum + " BP!");

                playerHolder.SetActive(false);
                EndPlayerTurn();
            }
            else
            {
                dh.DisplayBattleText("Silly! Your BP is already <i>full!</i>");
            }
        }
        else
        {
            if (bp > 0)
            {
                int randNum = Random.Range(1, 5);
                bp -= randNum;
                dh.DisplayBattleText("Listening took away " + randNum + " BP!");

                ShowOrHideActions(false);
                EndPlayerTurn();
            }
            else
            {
                dh.DisplayBattleText("Silly! Your BP is already <i>empty!</i>");
            }
        }        
    }

    public void SelectMove(int whichMove)
    {

        Moves currentMove;

        if (bh.opponent._name != "A new friend" && bh.opponent._name != "A cool person")
        {
            currentMove = myPersonas[currentPersona].moves[whichMove];
        }
        else
        {
            currentMove = myMoves[whichMove];
        }

        storedMove = currentMove;
        if (currentMove.amount < mp)
        {
            playerHolder.SetActive(false);
            currentMove.UseMove(bh.opponent, this);
            dh.DisplayBattleText(currentMove.GetRandomDialogue());
            madeMove = true;
        }
        else
        {
            dh.DisplayBattleText("Hey, don't forget to <i>breathe</i>!");
        }
    }

    public void BackToMainMenu()
    {
        if (!dh.isActive)
        {
            ch.ChangeCamAnim(0);
        }
        backButton.SetActive(false);
        moveHolder.SetActive(false);
        personaHolder.SetActive(false);
        actionHolder.SetActive(true);
    }

    public void BeginPlayerTurn()
    {
        if (dh.isActive)
        {
            waitForText = true;
        }
        playerHolder.SetActive(true);
        ShowOrHideActions(true);
        bh.isPlayerTurn = true;

    }

    public void EndPlayerTurn()
    {
        playerHolder.SetActive(false);
        madeMove = false;
        ch.ChangeCamAnim(1);
        bh.actionTimer = 1.5f;
        bh.isPlayerTurn = false;
        bh.actionChosen = false;
        BackToMainMenu();
    }

    void ShowOrHideActions(bool isShow) // false hide true show
    {
        actionHolder.SetActive(isShow);
    }


}
