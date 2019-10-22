using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody rb;
    DialogueHandler dh;
    GameController gc;


    [Space]
    [Header("Player Nums")]
    public float moveSpeed;
    public float fallMultiplier = 2.5f;
    public float jumpMultiplier = 2f;
    public float jumpSpeed;



    int hDir;
    int vDir;

    public bool nearObj;
    public bool canJump;

    // Start is called before the first frame update
    void Start()
    {
        canJump = true;
        rb = GetComponent<Rigidbody>();
        dh = FindObjectOfType<DialogueHandler>();
        gc = FindObjectOfType<GameController>();

       transform.position = gc.playerLastPos;
    }

    // Update is called once per frame
    void Update()
    {
        hDir = DirConstraints(Input.GetAxis("Horizontal"));
        vDir = DirConstraints(Input.GetAxis("Vertical"));

        if (!dh.isActive)
        {
            if (Input.GetKeyDown(KeyCode.Space) && canJump)
            {
                Jump(Vector2.up);
                canJump = false;
            }
        }
    }

    private void FixedUpdate()
    {

        if (!dh.isActive)
        {
            Vector3 dir = new Vector3(hDir, 0, vDir);
            Walk(dir);
        }

        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector3.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (rb.velocity.y > 0 && !Input.GetKey(KeyCode.Space))
        {
            rb.velocity += Vector3.up * Physics2D.gravity.y * (jumpMultiplier - 1) * Time.deltaTime;
        }

    }

    public void Walk(Vector3 dir) 
    {
        rb.velocity = new Vector3(dir.x * moveSpeed, rb.velocity.y, dir.z * moveSpeed);
    }

    public void Jump(Vector3 dir)
    {
        rb.velocity = new Vector3(rb.velocity.x, 0,rb.velocity.z);
        rb.velocity += dir * jumpSpeed;
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

    private void OnCollisionStay(Collision collision)
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
            InteractBehavior interact = collision.gameObject.GetComponent<InteractBehavior>();

            if (interact.character._name == "RAW")
            {
                StartConversation(interact);
                nearObj = false;
                canJump = false;
            }
            else
            {
                if (!interact.character.isDefeated)
                {
                    nearObj = true;
                    dh.label.SetActive(true);
                    dh.labelText.text = interact._name;

                    if (Input.GetKeyDown(KeyCode.Space))
                    {
                        StartConversation(interact);

                    }
                }
                else
                {
                    nearObj = false;
                    canJump = false;
                }
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
        gc.cameraLastPos = Camera.main.transform.position;
        gc.playerLastPos = transform.position;
        //dc.CheckWhosTalking(dc.myLines[dc.currentLine]);
        //dc.textHolder.SetActive(true);
        //dc._text.text = dc.myLines[dc.currentLine];
    }
}
