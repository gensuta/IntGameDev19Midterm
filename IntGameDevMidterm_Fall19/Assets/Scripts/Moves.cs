using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Objects",menuName = "Objects/Moves", order = 1)]
public class Moves : ScriptableObject
{
    public string _name;

    public bool doesDmg; // 
    public int amount; // amt of dmg or healing

    public GameController.Types moveType;

    public int moveCost;

    [TextArea(2,8)]
    public string[] dialogue;


    public string GetRandomDialogue()
    {
        int randNum;
        randNum = Random.Range(0, dialogue.Length);
        return dialogue[randNum];
    }

    public void UseMove(Character target, PlayerActions player)
    {
		if (doesDmg)
		{

			target.currentBp -= DetermineTrueAmount(moveType, target._type, amount);

		}
		else
		{
			target.currentBp += DetermineTrueAmount(moveType, target._type, amount);
		}
        player.mp -= moveCost;
    }

    public void UseMoveOnPlayer(PlayerActions target)
    {
        GameController.Types playerType = target.myPersonas[target.currentPersona]._type;

		if (doesDmg)
		{

			target.bp -= DetermineTrueAmount(moveType, playerType, amount);

		}
		else
		{
			target.bp += DetermineTrueAmount(moveType, playerType, amount);
		}
    }

    public int DetermineTrueAmount(GameController.Types type1, GameController.Types type2, int amt )
    {
        int trueAmt = amt;

        if(type1 == GameController.Types.kindness)
        {
            if(type2 == GameController.Types.tiredness)
            {
                Debug.Log("SUPER EFFECTIVE!");
                trueAmt *= 2;
            }
        }
        else if (type1 == GameController.Types.liveliness)
        {
            if (type2 == GameController.Types.kindness)
            {
                Debug.Log("SUPER EFFECTIVE!");
                trueAmt *= 2;
            }
        }
        else if (type1 == GameController.Types.tiredness)
        {
            if (type2 == GameController.Types.liveliness) // no matter how upbeat u r 
            {
                Debug.Log("SUPER EFFECTIVE!");
                trueAmt *= 2;
            }
        }

        return trueAmt;
    }
}
