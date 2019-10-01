using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu (fileName = "Objects",menuName = "Objects/Persona", order =2)]
public class Personas : ScriptableObject
{
    public string _name;

    public Sprite neutralPose;
    public ParticleSystem ps;

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
}
