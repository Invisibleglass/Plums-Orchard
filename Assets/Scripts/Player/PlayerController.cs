using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator), typeof(SpriteRenderer))]
public class PlayerController : MonoBehaviour
{
    protected Rigidbody2D rb;
    protected Animator anim;
    protected SpriteRenderer sr;
    public PlayerControls controls;

    Vector2 moveInput;

    [Header("PlayerParts")]
    public GameObject basket;
    public GameObject bounceBox;
    private Vector2 basketOffsetValue = new Vector2(1.05f, 0f); // used when the player moves left and right to put the basket box in the right place
    private Vector2 playerOffsetValue = new Vector2(0.1f, 1.1f); //^ but for the players hitbox's
    [Header("Player Values")]
    public float moveSpeed;
    public float jumpForce;
    public float jumpsAllowed;
    public float blinkTime;
    protected float jumpsRemaining;
    protected bool jumpPressed;

    // Start is called before the first frame update
    protected void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        controls = new PlayerControls();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }
    protected void OnEnable()
    {
        controls.Enable();
    }

    protected void OnDisable()
    {
        controls.Disable();
    }

    protected void Move(InputAction.CallbackContext ctx)
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


    protected void FixedUpdate()
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
    protected bool IsGrounded()
    {
        // Check if the object's collider is touching the Ground layer
        return GetComponent<Collider2D>().IsTouchingLayers(LayerMask.GetMask("Ground"));
    }

    public void BounceMe()
    {
        rb.velocity = new Vector2(rb.velocity.x, 0); // Reset vertical velocity
        rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
    }

    public void Ouch()
    {
        StartCoroutine(OuchRoutine());
    }

    protected IEnumerator OuchRoutine()
    {
        sr.color = Color.clear;
        yield return new WaitForSeconds(blinkTime);
        sr.color = Color.white;
        yield return new WaitForSeconds(blinkTime);
        sr.color = Color.clear;
        yield return new WaitForSeconds(blinkTime);
        sr.color = Color.white;
        yield return new WaitForSeconds(blinkTime);
        sr.color = Color.clear;
        yield return new WaitForSeconds(blinkTime);
        sr.color = Color.white;
    }
    // Update is called once per frame
    protected void Update()
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
                GetComponent<BoxCollider2D>().offset = playerOffsetValue;
                bounceBox.GetComponent<BoxCollider2D>().offset = new Vector2(playerOffsetValue.x, 0f);
            }
            else if (moveInput.x > 0)
            {
                sr.flipX = false;
                basket.GetComponent<BoxCollider2D>().offset = basketOffsetValue;
                GetComponent<BoxCollider2D>().offset = new Vector2(-playerOffsetValue.x, playerOffsetValue.y);
                bounceBox.GetComponent<BoxCollider2D>().offset = new Vector2(-playerOffsetValue.x, 0f);
            }

        }
        else if(moveInput.y !=0)
        {
            anim.speed = 1f;
            if (moveInput.y < 0)
            {
                anim.SetBool("leftRightBool", false);
                basket.GetComponent<BoxCollider2D>().offset = new Vector2(0f, 0f); // back to in front of the player;
                GetComponent<BoxCollider2D>().offset = new Vector2(0f, 1.1f);
                bounceBox.GetComponent<BoxCollider2D>().offset = new Vector2(0f, 0f);
            }
        }
        else
        {
            anim.speed = 0f;
        }
    }
}
