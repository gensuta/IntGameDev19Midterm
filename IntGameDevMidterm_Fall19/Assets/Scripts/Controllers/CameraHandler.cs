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
        //anim = GetComponent<Animator>();

        if (isBattleCamera)
        {
            bh = FindObjectOfType<BattleHandler>();
        }
        else
        {
            offset = transform.position - target.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isBattleCamera)
        {
            transform.LookAt(target);
        }
        else // hallway camera
        {
            //transform.LookAt(target);
            newPos = target.position + offset;
            newPos.y = transform.position.y;
            //newPos.z = transform.position.z;

            Vector3 moveVector = transform.position - newPos;
            moveVector *= 0.75f; //75% of the way there
            newPos += moveVector;
            transform.position = newPos;
        }
    }

}
