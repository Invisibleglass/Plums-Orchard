using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D), typeof(SpriteRenderer))]
public class WalkingEnemy : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sr;

    Vector3 centerScreen = new Vector3(0f, 0f, 0f);

    private bool startRightBool;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();

        if (this.transform.position.x > centerScreen.x)
        {
            sr.flipX = false;
            startRightBool = true;
        }
        else
        {
            sr.flipX = true;
            startRightBool = false;
        }
    }

    private void FixedUpdate()
    {
        if (startRightBool)
        {
            rb.velocity.Set()
        }
        else
        {
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
