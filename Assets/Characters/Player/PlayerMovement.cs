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
    float dirX = Input.GetAxisRaw("Horizontal");
    float dirY = Input.GetAxisRaw("Vertical");
    private Animator animator;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        //processing inputs
        ProcessInputs();
    }

    void Start() 
    {
        animator = GetComponent<Animator>();

    }

    void FixedUpdate()
    {
        //physics calculations
        Move();
        GenerateAnimations();
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



    void GenerateAnimations()
    {
    
        //I need to get the players current orientation
        //when the player chooses a different direction, flip the sprite to face that directoin
        // change Input.getAXISRAW into a switchable variable

        if (dirX > 0.5 || dirX < -0.5f)
        {
            if (dirX > 0.5)
            {
                direction = playerDirection.right;
                Console.WriteLine(direction
                    .ToString());

            } else if (Input.GetAxisRaw("Horizontal") < -0.5f)
            {
                
                direction = playerDirection.left;
            }
               
        }

//set vertical animations for running up and down
        if (dirY > 0.5 || dirY < -0.5f)
        {
            if (dirY > 0.5)
            {
                direction = playerDirection.up;

            }else if (dirY < -0.5f)
            {
                animator.SetBool("isRunning", true);
                direction = playerDirection.down;
            } else {
                animator.SetBool("isRunning", false);
            }
        }
        /*
         *         switch (direction)
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
         */

    }
}
