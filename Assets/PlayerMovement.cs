using JetBrains.Annotations;
using System;
using System.IO;
using Unity.VisualScripting.InputSystem;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    enum playerDirection
    {
        up,
        down,
        left,
        right
    }

    public float moveSpeed;
    public Rigidbody2D rigidBody;
    public InputAction playerMovement;
    Vector2 moveDirection = Vector2.zero;
    playerDirection direction;


    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        //processing inputs
        ProcessInputs();
    }


    void FixedUpdate()
    {
        //physics calculations
        Move();
        SetOrientation();
    }

    private void OnEnable()
    {
        playerMovement.Enable();
    }

    private void OnDisable()
    {
        playerMovement.Disable();
    }

    void ProcessInputs()
    {
        //float moveX = Input.GetAxisRaw("Horizontal");
        // float moveY = Input.GetAxisRaw("Vertical");

        moveDirection = playerMovement.ReadValue<Vector2>();
    }

    void Move()
    {
        rigidBody.velocity = new Vector2(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed);
    }

    void SetOrientation()
    {
        
        //I need to get the players current orientation
        //when the player chooses a different direction, flip the sprite to face that directoin
        // change Input.getAXISRAW into a switchable variable

        if (Input.GetAxisRaw("Horizontal") > 0.5 || Input.GetAxisRaw("Horizontal") < -0.5f)
        {
            if (Input.GetAxisRaw("Horizontal") > 0.5)
            {
                direction = playerDirection.right;

            } else if (Input.GetAxisRaw("Horizontal") < -0.5f)
            {
                direction = playerDirection.left;
            }
               
        }

        if (Input.GetAxisRaw("Vertical") > 0.5 || Input.GetAxisRaw("Vertical") < -0.5f)
        {
            if (Input.GetAxisRaw("Vertical") > 0.5)
            {
                direction = playerDirection.up;

            }else if (Input.GetAxisRaw("Vertical") < -0.5f)
            {
                direction = playerDirection.down;
            }
        }

        switch (direction)
        {
            case playerDirection.right:
                transform.localScale = new Vector2(1, 0);
                break;
            case playerDirection.left:
                transform.localScale = new Vector2(-1, 0);
                break;
            case playerDirection.up:
                transform.localScale = new Vector2(0, 1);
                break;
            case playerDirection.down:
                transform.localScale = new Vector2(0, -1);
                break;
        }
    }
}
