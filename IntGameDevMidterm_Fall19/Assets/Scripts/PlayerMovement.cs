using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed;
    public float rotateSpeed;
    public float jumpSpeed;
    Rigidbody rb;

    float rotateDir;
    float movementDir;

    DialogueHandler dh;

    bool nearObj;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        dh = FindObjectOfType<DialogueHandler>();
    }

    // Update is called once per frame
    void Update()
    {


    }

    private void FixedUpdate()
    {
        rotateDir = DirConstraints(Input.GetAxis("Horizontal"));
        movementDir = DirConstraints(Input.GetAxis("Vertical"));

        if (Input.GetButton("Horizontal")) // rotate with left/right or ad
        {
            rb.AddTorque(transform.up * rotateSpeed * rotateDir, ForceMode.Force);
        }

        if (Input.GetButton("Vertical")) // move with up/down or ws
        {
            rb.AddForce(transform.forward * moveSpeed * movementDir, ForceMode.Force);
        }

        if (Input.GetKeyDown(KeyCode.Space)) // jump
        {
            rb.AddForce(transform.up * jumpSpeed, ForceMode.Impulse);
        }
    }

    int DirConstraints(float dir)
    {
        if (dir > 0)
        {
            return 1;
        }
        else if (dir < 0)
        {
            return -1;
        }
        else
        {
            return 0;
        }
    }

    private void OnTriggerStay(Collider collision)
    {
        if (collision.gameObject.GetComponent<InteractBehavior>() != null && !dh.isActive)
        {
            nearObj = true;
            InteractBehavior interact = collision.gameObject.GetComponent<InteractBehavior>();
            dh.label.SetActive(true);
            dh.labelText.text = interact._name;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                dh.label.SetActive(false);
                dh.myLines.Clear();
                foreach (string line in interact.character.exploreLines)
                {
                    dh.myLines.Add(line);
                }
                dh.currentLine = 0;
                dh.EnableTextBox();

                //dc.CheckWhosTalking(dc.myLines[dc.currentLine]);
                //dc.textHolder.SetActive(true);
                //dc._text.text = dc.myLines[dc.currentLine];

            }

        }
    }
    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.GetComponent<InteractBehavior>() != null)
        {
            nearObj = false;
            dh.label.SetActive(false);
        }
    }
}