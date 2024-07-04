using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snek : WalkingEnemy
{
    private bool startedDirectionSwitcherBool = false;
    private Vector2 landingOffsetValue = new Vector2(0f, 0.4f); // when the snake lands his boxcollider needs to change with animation size
    private Vector2 landingSizeValue = new Vector2(1.17f, 0.77f); //^
    [Header("Connections")]
    public GameObject bounceBox;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        targets = GameObject.FindGameObjectsWithTag("target");

        FindObjectOfType<SoundManager>().PlayOneShot(spawnSound);
        if (this.transform.position.x > centerScreen.x)
        {
            sr.flipX = false;
            currentTarget = targets[1];
        }
        else
        {
            sr.flipX = true;
            currentTarget = targets[0];
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (IsGrounded())
        {
            if (!startedDirectionSwitcherBool)
            {
                StartCoroutine(ChangeDirectionRoutine());
                anim.SetBool("isGrounded", true);
                GetComponent<BoxCollider2D>().offset = landingOffsetValue;
                GetComponent<BoxCollider2D>().size = landingSizeValue;
                bounceBox.SetActive(true);
                startedDirectionSwitcherBool = true;
            }

            if (currentTarget != null)
            {
                // Calculate direction towards the current target
                Vector2 direction = (currentTarget.transform.position - transform.position).normalized;

                // Apply velocity based on calculated direction
                rb.velocity = direction * speed;
            }
        }
    }

    private bool IsGrounded()
    {
        // Check if the object's collider is touching the Ground layer
        return GetComponent<Collider2D>().IsTouchingLayers(LayerMask.GetMask("Ground"));
    }

    override public void DeathAnimation()
    {
        GetComponent<BoxCollider2D>().excludeLayers += LayerMask.GetMask("Player");
        currentTarget = null;
        dieingBool = true;
        if (sr.sprite.name == "snek_2_4")
        {
            anim.Play("Hurt");
        }
        else
        {
            anim.Play("Hurt2");
        }
        StartCoroutine(Death());
    }
}
