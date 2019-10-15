using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Character", menuName = "Objects/Characters", order = 0)]
public class Character : ScriptableObject
{
    public bool hasShownDefeatedLines;
    public bool isDefeated;
    public string _name;

    public int maxBp; // bond points. 
    public int currentBp;

    public GameController.Types _type; // is this character lively, tired, or kind?

    public bool willBreakBond; // does this character want to break away from 'You' or not?
    // some characters want to keep their bond with you and will try to increase your bp while you decrease it
    // if willBreakBond, all moves shouldn't do dmg

    public bool bondWanted; // do 'you' want to keep this bond?

    [TextArea (5,8)]
    public List<string> exploreLines;//lines seen when you first meet them

    [TextArea(5, 8)]
    public List<string> defeatLines;//lines seen when you 'defeat' them

    public Moves[] moves;

}
