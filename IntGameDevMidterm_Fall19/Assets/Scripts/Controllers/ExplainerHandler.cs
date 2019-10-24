using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class ExplainerHandler : MonoBehaviour
{
    BattleHandler bh;
    PlayerActions player;
    public TextMeshProUGUI moveExplain;
    public TextMeshProUGUI personaExplain;

    public Sprite[] types;
    public Image moveRep;
    public Image pRep;
    // Start is called before the first frame update
    void Start()
    {
        bh = FindObjectOfType<BattleHandler>();
        player = FindObjectOfType<PlayerActions>();
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void CheckMoves(int whichMove)
    {
        if (bh.opponent._name != "Shy" && bh.opponent._name != "Bubbly" && bh.opponent._name != "RAW")
        {
            Personas persona = player.myPersonas[player.currentPersona];
            moveExplain.text = Explain(persona.moves[whichMove]);
        }
        else
        {
            moveExplain.text = Explain(player.myMoves[whichMove]);
        }
    }

    public void CheckPersona(int whichP)
    {
        personaExplain.text = Explain(player.myPersonas[whichP]);
    }

    string Explain(Moves move)
    {
        string explanation = "";
        explanation += "Name: " + move._name;
        explanation += "\nType: " + move.moveType.ToString();
        explanation += " Power:";
        if (move.doesDmg)
        {
            explanation += " -" + move.amount;
        }
        else
        {
            explanation += " +" + move.amount;
        }
        explanation += "\nCost: " + move.moveCost;


        if(move.moveType == GameController.Types.kindness)
        {
            moveRep.sprite = types[0];
        }
        else if (move.moveType == GameController.Types.tiredness)
        {
            moveRep.sprite = types[1];
        }
        else if (move.moveType == GameController.Types.liveliness)
        {
            moveRep.sprite = types[2];
        }

        return explanation;
    }
    string Explain(Personas persona)
    {
        string explanation = "";

        explanation += "Name: " + persona._name;
        explanation += "\nType: " + persona._type.ToString();
        explanation += "\nMoves:";
        foreach(Moves move in persona.moves)
        {
            explanation += " " + move._name;
        }

        if (persona._type == GameController.Types.kindness)
        {
            pRep.sprite = types[0];
        }
        else if (persona._type == GameController.Types.tiredness)
        {
            pRep.sprite = types[1];
        }
        else if (persona._type == GameController.Types.liveliness)
        {
            pRep.sprite = types[2];
        }

        return explanation;
    }
}
