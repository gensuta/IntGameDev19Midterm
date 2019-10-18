using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    BattleHandler bh;

    public bool isBattleCamera;
    public Transform target;
    public float speed;


    Vector3 offset;
    Vector3 newPos;
    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        if (isBattleCamera)
        {
            bh = FindObjectOfType<BattleHandler>();
            anim = GetComponent<Animator>();
        }
        else
        {
            transform.position = GameController.gc.cameraLastPos;
            offset = transform.position - target.position;
            //distance for movement should have a minimum so that the camera only moves if the distance is far
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(!isBattleCamera) // hallway camera
        {
            //transform.LookAt(target);
            newPos = target.position + offset;
            newPos.y = transform.position.y;
            //newPos.z = transform.position.z;

            Vector3 moveVector = transform.position - newPos;
            moveVector *= 0.5f; //75% of the way there
            newPos += moveVector;
            transform.position = newPos;
        }
    }

    public void ChangeCamAnim(int which)   //0 idle/playerTurn 1 still/enemyTurn 2 personaChange 3 persona menu
    {
        anim.SetInteger("whichAnim", which);
        //anim.Play(0);
    }

}
