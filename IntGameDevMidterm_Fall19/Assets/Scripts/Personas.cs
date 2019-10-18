using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu (fileName = "Objects",menuName = "Objects/Persona", order =2)]
public class Personas : ScriptableObject
{
    public string _name;

    public GameObject ps; //particle system

    public Sprite neutralPose;
    public GameObject myModel;
    public Vector3 customRotation;

    public GameController.Types _type; // is this character lively, tired, or kind?

    public Moves[] moves;
    public RuntimeAnimatorController animControl;

    public void SetImage(Image img, Sprite newSprite)
    {
        img.sprite = newSprite;
    }

    public void SetAnimator(Animator anim)
    {
        anim.runtimeAnimatorController = animControl;
    }

    public GameObject SpawnMe(Vector3 pos, PlayerActions player)
    {

        GameObject g = Instantiate(myModel, pos, Quaternion.identity);
        g.transform.parent = player.transform;
        g.transform.rotation = Quaternion.Euler(customRotation);
        return g;
    }
}
