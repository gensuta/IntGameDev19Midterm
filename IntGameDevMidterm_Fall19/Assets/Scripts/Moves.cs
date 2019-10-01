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


    public void UseMove(Character target)
    {
        if (doesDmg)
        {
            if(target._type == moveType)
            {
                target.currentBp -= DetermineTrueAmount(moveType,target._type,amount);
            }
           
        }
        else
        {
            target.currentBp += DetermineTrueAmount(moveType, target._type,amount);
        }
    }

    public int DetermineTrueAmount(GameController.Types type1, GameController.Types type2, int amt )
    {
        int trueAmt = amt;

        if(type1 == GameController.Types.kindness)
        {
            if(type2 == GameController.Types.tiredness)
            {
                trueAmt *= 2;
            }
        }
        else if (type1 == GameController.Types.liveliness)
        {
            if (type2 == GameController.Types.kindness)
            {
                trueAmt *= 2;
            }
        }
        else if (type1 == GameController.Types.tiredness)
        {
            if (type2 == GameController.Types.liveliness) // no matter how upbeat u r 
            {
                trueAmt *= 2;
            }
        }


        return trueAmt;
    }
}
