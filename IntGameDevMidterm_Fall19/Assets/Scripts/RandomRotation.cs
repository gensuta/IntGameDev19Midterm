using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomRotation : MonoBehaviour
{
    public float rotateSpeed;

    float randX;
    float randY;
    float randZ;


    bool isRotatingX;
    bool isRotatingY;
    bool isRotatingZ;
    // Start is called before the first frame update
    void Start()
    {
        randX = GetRandNum();
        randY = GetRandNum();
        randZ = GetRandNum();

        int r = Random.Range(0, 3);
        switch(r)
        {
            case (0):
                isRotatingX = true;
                break;
            case (1):
                isRotatingY = true;
                break;
            case (2):
                isRotatingZ = true;
                break;
        }


        Vector3 newRotation = new Vector3(randX, randY, randZ);

        transform.rotation = Quaternion.Euler(newRotation);
    }

    // Update is called once per frame
    void Update()
    {
        if(isRotatingX)
        {
            transform.Rotate(Vector3.right * rotateSpeed);
        }
        else if(isRotatingY)
        {
            transform.Rotate(Vector3.up * rotateSpeed);
        }
        else if(isRotatingZ)
        {
            transform.Rotate(Vector3.forward * rotateSpeed);
        }
    }

    float GetRandNum()
    {
        float rand;

        rand = Random.Range(-360f, 360.1f);

        return rand;
    }
}
