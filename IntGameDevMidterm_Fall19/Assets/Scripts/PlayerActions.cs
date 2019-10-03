using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerActions : MonoBehaviour // BATTLE!! ACTIONS!!
{
    BattleHandler bh;

    public Personas[] myPersonas;
    public int currentPersona;
  
    public int bp;
    public int mp;

    [Space]
    [Header("UI mess")]
    public TextMeshProUGUI bpTxt;
    public TextMeshProUGUI mpTxt;
    public Slider bpSlider;
    public Slider mpSlider;

    [Space]
    public GameObject actionHolder;
    public GameObject moveHolder;
    public GameObject personaHolder;


    // Start is called before the first frame update
    void Start()
    {
        bpSlider.maxValue = 20;
        mpSlider.maxValue = 20;

        mp = 20;
        bh = FindObjectOfType<BattleHandler>();

        if (bh.opponent.willBreakBond)
        {
            bp = 20;
        }
        else
        {
            bp = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateUI();

        if (Input.GetKey(KeyCode.Alpha0))
        {
            if(Input.GetKeyDown(KeyCode.M))
            {
                BackToMainMenu();
            }
        }
    }

    void UpdateUI()
    {
        bpTxt.text = "Bond Points: " + bp;
        mpTxt.text = "Mindful Points: " + mp;
        bpSlider.value = bp;
        mpSlider.value = mp;
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
                button.GetComponentInChildren<TextMeshProUGUI>().text = myPersonas[currentPersona].moves[whichMove]._name;
                whichMove++;
            }
        }
        else
        {
            Debug.Log("hey, don't forget to breathe");
        }
    }

   

    public void ShowPersonas()
    {
        actionHolder.SetActive(false);
        personaHolder.SetActive(true);
        int whichP = 0;
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
            Debug.Log("silly! you have that persona equipped already!");
        }
        else
        {
            currentPersona = whichPersona;
            Debug.Log("you're new persona is " + myPersonas[currentPersona]._name);
            EndPlayerTurn();
        }

    }

    public void Breathe() // restore mp
    {
        if(mp < 20)
        {
            mp += Random.Range(3, 7);
        }
        else
        {
            Debug.Log("silly! your mp is already full!");

        }
        EndPlayerTurn();
    }

    public void Stare() // restore bp
    {
        if(bh.unwantedBP == 0)
        {
            if(bp < 20)
            {
                bp += Random.Range(1, 5);
            }
            else
            {
                Debug.Log("silly! your bp is already full!");
            }
        }
        else
        {
            if (bp > 0)
            {
                bp -= Random.Range(1, 5);
            }
            else
            {
                Debug.Log("silly! your bp is already empty!");
            }
        }

        EndPlayerTurn();
    }

    public void SelectMove(int whichMove)
    { 
        Moves currentMove = myPersonas[currentPersona].moves[whichMove];
        if (currentMove.amount < mp)
        {
            currentMove.UseMove(bh.opponent, this);
            Debug.Log("player used " + currentMove + " on " + bh.opponent._name);
            EndPlayerTurn();
        }
        else
        {
            Debug.Log("Hey, maybe you need to zone out for a bit");
        }
    }

    public void BackToMainMenu()
    {
        moveHolder.SetActive(false);
        personaHolder.SetActive(false);
        actionHolder.SetActive(true);
    }

    public void BeginPlayerTurn()
    {
        ShowOrHideActions(true);
        bh.isPlayerTurn = true;

    }

    public void EndPlayerTurn()
    {
        bh.actionTimer = 1.5f;
        bh.isPlayerTurn = false;
        BackToMainMenu();
        ShowOrHideActions(false);
       
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
