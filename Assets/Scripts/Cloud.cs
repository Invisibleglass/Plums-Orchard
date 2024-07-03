using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour
{
    public GameObject leftClamp;
    public GameObject rightClamp;
    public GameObject currentTarget;
    public float speed;
    private Rigidbody2D rb;

    private void FixedUpdate()
    {
        if (currentTarget != null)
        {
            // Calculate direction towards the current target
            Vector2 direction = (currentTarget.transform.position - transform.position).normalized;

            // Apply velocity based on calculated direction
            rb.velocity = direction * speed;
            if(transform.position.x +0.1f >= currentTarget.transform.position.x >= transform.position.x -0.1f)
            {
               if (currentTarget == rightClamp)
               {
                    currentTarget = leftClamp;
               }
               else if (currentTarget == leftClamp)
               {
                    currentTarget = rightClamp;
               }
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
       currentTarget = leftClamp;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}