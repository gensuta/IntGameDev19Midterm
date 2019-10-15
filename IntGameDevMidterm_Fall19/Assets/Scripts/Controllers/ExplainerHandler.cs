using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class ExplainerHandler : MonoBehaviour
{
    PlayerActions player;
    public TextMeshProUGUI moveExplain;
    public TextMeshProUGUI personaExplain;
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerActions>();
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void CheckMoves(int whichMove)
    {
        Personas persona = player.myPersonas[player.currentPersona];
        moveExplain.text = Explain(persona.moves[whichMove]);
    }

    public void CheckPersona(int whichP)
    {
        personaExplain.text = Explain(player.myPersonas[whichP]);
    }

    string Explain(Moves move)
    {
        string explanation = "";
        explanation += "Name: " + move._name;
        explanation += "\n Type: " + move.moveType.ToString();
        if(move.doesDmg)
        {
            explanation += " - Breaks Bonds";
        }
        else
        {
            explanation += " - Make Bonds";
        }
        explanation += " Power: " + move.amount;

        return explanation;
    }
    string Explain(Personas persona)
    {
        string explanation = "";

        explanation += "Name: " + persona._name;
        explanation += " Type: " + persona._type.ToString();
        explanation += "\nMoves:";
        foreach(Moves move in persona.moves)
        {
            explanation += " " + move._name;
        }
        return explanation;
    }
}
