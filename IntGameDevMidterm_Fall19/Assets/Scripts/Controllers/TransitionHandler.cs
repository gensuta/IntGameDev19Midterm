using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionHandler : MonoBehaviour
{
    public Animator anim;
    int storedNum;
    float timer = 0f;
    bool isCountingDown;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isCountingDown)
        {
            timer -= Time.deltaTime;
            if(timer <= 0f)
            {
                anim.SetInteger("whichAnim", storedNum);
                anim.Play(0);
                isCountingDown = false;
            }
        }
    }

    /*
   0 is black fadein fadeout
   1 is...
   */
    public void PlayAnim(int num)
    {
        anim.SetInteger("whichAnim", num);
        anim.Play(0);
    }

    public void PlayDelayedAnim(int num, float _timer)
    {
        timer = _timer;
        storedNum = num;
        isCountingDown = true;
    }
}
