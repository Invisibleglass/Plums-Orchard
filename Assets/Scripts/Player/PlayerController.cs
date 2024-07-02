using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator), typeof(SpriteRenderer))]
public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sr;
    public PlayerControls controls;

    Vector2 moveInput;

    [Header("PlayerParts")]
    public GameObject basket;
    private Vector2 basketOffsetValue = new Vector2(0.4f, 0); // used when the player moves left and right to put the basket box in the right place
    [Header("Player Values")]
    public float moveSpeed;
    public float jumpForce;
    public float jumpsAllowed;
    private float jumpsRemaining;
    private bool jumpPressed;

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        controls = new PlayerControls();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }
    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    private void Move(InputAction.CallbackContext ctx)
    {
        moveInput = ctx.action.ReadValue<Vector2>();
    }

    // Start is called before the first frame update
    void Start()
    {
        controls.Player.Move.performed += ctx => Move(ctx);
        controls.Player.Move.canceled += ctx => Move(ctx);

        jumpsRemaining = jumpsAllowed;
    }

    private bool IsGrounded()
    {
        // Check if the object's collider is touching the Ground layer
        return GetComponent<Collider2D>().IsTouchingLayers(LayerMask.GetMask("Ground"));
    }

    private void FixedUpdate()
    {
        float horizontalMovement = moveInput.x * moveSpeed * Time.fixedDeltaTime;

        rb.velocity = new Vector2(horizontalMovement, rb.velocity.y);

        //jump logic
        if(moveInput.y > 0 && !jumpPressed)
        {
            jumpPressed = true;

            if(IsGrounded())
            {
                jumpsRemaining = jumpsAllowed;
                //first jump
                rb.velocity = new Vector2(rb.velocity.x, 0); //resets vertical velocity
                rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
                jumpsRemaining--;
                Debug.Log("Jumps remaining: " + jumpsRemaining);
            }
            else if (jumpsRemaining > 0)
            {
                // Double jump
                rb.velocity = new Vector2(rb.velocity.x, 0); // Reset vertical velocity
                rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
                jumpsRemaining--;
                Debug.Log("Jumps remaining: " + jumpsRemaining);
            }
        }
        else if (moveInput.y <= 0)
        {
            jumpPressed = false; // Reset jumpPressed flag when no longer holding jump input
        }
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetBool("isGrounded", IsGrounded());

        if(moveInput.x != 0)
        {
            anim.speed = 1f;
            anim.SetBool("leftRightBool", true);
            if (moveInput.x < 0)
            {
                sr.flipX = true;
                basket.GetComponent<BoxCollider2D>().offset = -basketOffsetValue;
            }
            else if (moveInput.x > 0)
            {
                sr.flipX = false;
                basket.GetComponent<BoxCollider2D>().offset = basketOffsetValue;
            }

        }
        else if(moveInput.y !=0)
        {
            anim.speed = 1f;
            if (moveInput.y < 0)
            {
                anim.SetBool("leftRightBool", false);
                basket.GetComponent<BoxCollider2D>().offset = new Vector2(0f, 0f); // back to in front of the player;
            }
        }
        else
        {
            anim.speed = 0f;
        }
    }
}
