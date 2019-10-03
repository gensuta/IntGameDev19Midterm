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
    GameController gc;

    bool nearObj;
    public bool canJump = true;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        dh = FindObjectOfType<DialogueHandler>();
        gc = FindObjectOfType<GameController>();
    }

    // Update is called once per frame
    void Update()
    {

        rotateDir = DirConstraints(Input.GetAxis("Horizontal"));
        movementDir = DirConstraints(Input.GetAxis("Vertical"));
    }

    private void FixedUpdate()
    {

        if (!dh.isActive)
        {
            if (Input.GetButton("Horizontal")) // rotate with left/right or ad
            {
                transform.Rotate(0, rotateSpeed * rotateDir, 0);
                if (canJump)
                {
                    rb.drag = 1;
                }
            }

            if (Input.GetButton("Vertical")) // move with up/down or ws
            {
                rb.AddForce(transform.forward * moveSpeed * movementDir, ForceMode.Force);
                if (canJump)
                {
                    rb.drag = 1;
                }
            }
            if (Input.GetButtonUp(("Horizontal")))
            {
                rb.drag = 5;
            }
            if (Input.GetButtonUp(("Vertical")))
            {
                rb.drag = 5;
            }

            if (Input.GetKeyDown(KeyCode.Space) && canJump) // jump
            {
                rb.AddForce(transform.up * jumpSpeed, ForceMode.Impulse);
                canJump = false;
            }
        }
        if (!canJump && rb.velocity.y> 1.5f)
        {
            rb.velocity += Vector3.up * Physics2D.gravity.y * (jumpSpeed - 1) * Time.deltaTime;
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

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == ("Ground") && !nearObj)
        {
            canJump = true;
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
            canJump = false;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                StartConversation(interact);

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

    public void StartConversation(InteractBehavior interact)
    {
        dh.nameTxt.text = interact.character._name;
        gc.storedChar = Instantiate(interact.character); // this is so we never DIRECTLY edit the object in the folder during the battle
        switch (interact.character._type)
        {
            case (GameController.Types.tiredness):
                dh.whichVoice = 0;
                break;
            case (GameController.Types.kindness):
                dh.whichVoice = 1;
                break;
            case (GameController.Types.liveliness):
                dh.whichVoice = 2;
                break;
        }

        dh.currentLine = 0;
        dh.label.SetActive(false);
        dh.myLines.Clear();
        foreach (string line in interact.character.exploreLines)
        {
            dh.myLines.Add(line);
        }

        dh.EnableTextBox();

        //dc.CheckWhosTalking(dc.myLines[dc.currentLine]);
        //dc.textHolder.SetActive(true);
        //dc._text.text = dc.myLines[dc.currentLine];
    }
}
