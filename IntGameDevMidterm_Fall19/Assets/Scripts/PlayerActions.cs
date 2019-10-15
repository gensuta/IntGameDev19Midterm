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

    bool waitForText;
    bool madeMove;
    Moves storedMove;

    // Start is called before the first frame update
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

      //  ShowPersonaFX();
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
            dh.DisplayBattleText("You used " + storedMove._name + " on " + bh.opponent._name);
            EndPlayerTurn();
        }
    }

    void ShowPersonaFX()
    {
        if(GameObject.FindWithTag("fx") != null)
        {
            Destroy(GameObject.FindWithTag("fx"));
        }
        Instantiate(myPersonas[currentPersona].ps);
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
            backButton.SetActive(true);
            actionHolder.SetActive(false);
            moveHolder.SetActive(true);
            int whichMove = 0;
            foreach (Button button in moveHolder.GetComponentsInChildren<Button>())
            {
                button.GetComponentInChildren<TextMeshProUGUI>().text = myPersonas[currentPersona].moves[whichMove]._name;
                whichMove++;
            }
        }
        else
        {
            dh.DisplayBattleText("Hey, don't forget to <i>breathe</i>.");
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
            dh.DisplayBattleText("Silly! You have that persona equipped already!");
        }
        else
        {
            currentPersona = whichPersona;
            dh.DisplayBattleText("You're new persona is <color=red>" + myPersonas[currentPersona]._name + "</color>");
            EndPlayerTurn();
        }

    }

    public void Breathe() // restore mp
    {
        if(mp < 20)
        {
            mp += Random.Range(3, 7);
            EndPlayerTurn();
        }
        else
        {
            dh.DisplayBattleText("Silly! Your MP is already full!");

        }
    }

    public void Stare() // restore bp
    {
        if(bh.unwantedBP == 0)
        {
            if(bp < 20)
            {
                bp += Random.Range(1, 5);
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
                bp -= Random.Range(1, 5);
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
        Moves currentMove = myPersonas[currentPersona].moves[whichMove];
        storedMove = currentMove;
        if (currentMove.amount < mp)
        {
            ShowOrHideActions(false);
            currentMove.UseMove(bh.opponent, this);
            dh.DisplayBattleText(currentMove.GetRandomDialogue());
            madeMove = true;
        }
        else
        {
            dh.DisplayBattleText("Hey, maybe you need to zone out for a bit");
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
        ShowOrHideActions(true);
        bh.isPlayerTurn = true;

    }

    public void EndPlayerTurn()
    {
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
        //foreach (Button button in actionHolder.GetComponentsInChildren<Button>())
        //{
        //    button.gameObject.SetActive(isShow);
        //}
    }


}
