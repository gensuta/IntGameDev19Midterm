using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMovement : MonoBehaviour
{
    Rigidbody rb;
    public float moveSpeed;

    public float maxSpd;
    public float minSpd;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        moveSpeed = Random.Range(minSpd, maxSpd);
    }

    // Update is called once per frame
    void Update()
    {
        rb.AddForce(-transform.up * moveSpeed);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "Ground")
        {
            float rand = Random.value;
            if (rand < 0.5f) //turn left
            {
                transform.Rotate(0, -90f, 0);
            }
            else
            {
                transform.Rotate(0, 90f, 0);
            }
        }
    }
}
