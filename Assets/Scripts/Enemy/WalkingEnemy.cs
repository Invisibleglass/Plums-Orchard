using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D), typeof(SpriteRenderer))]
public class WalkingEnemy : MonoBehaviour
{
    protected Rigidbody2D rb;
    protected Animator anim;
    protected SpriteRenderer sr;

    protected Vector3 centerScreen = new Vector3(0f, 0f, 0f);

    protected GameObject[] targets;
    protected GameObject currentTarget;
    protected bool dieingBool = false;

    [Header("Varibles")]
    public float speed;
    public float changeDirectionIntervalMin;
    public float changeDirectionIntervalMax;
    public float deathTime;

    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        targets = GameObject.FindGameObjectsWithTag("target");

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

        // Start coroutine for changing direction
        StartCoroutine(ChangeDirectionRoutine());
    }

    protected IEnumerator ChangeDirectionRoutine()
    {
        while (!dieingBool)
        {
            // Randomly determine how long to wait before changing direction
            float waitTime = Random.Range(changeDirectionIntervalMin, changeDirectionIntervalMax);
            yield return new WaitForSeconds(waitTime);
            if (!dieingBool)
            {

                // Determine new target based on current position relative to centerScreen
                if (currentTarget != targets[0])
                {
                    currentTarget = targets[0];
                    sr.flipX = true;
                }
                else
                {
                    currentTarget = targets[1];
                    sr.flipX = false;
                }
            }
        }

    }

    protected IEnumerator Death()
    {
        yield return new WaitForSeconds(deathTime);

        Destroy(this.gameObject);
    }

    // FixedUpdate is used for physics calculations
    private void FixedUpdate()
    {
        if (currentTarget != null)
        {
            // Calculate direction towards the current target
            Vector2 direction = (currentTarget.transform.position - transform.position).normalized;

            // Apply velocity based on calculated direction
            rb.velocity = direction * speed;
        }
    }

    public virtual void DeathAnimation()
    {
        currentTarget = null;
        dieingBool = true;
        if (sr.sprite.name == "snek_2_11")
        {
            anim.Play("Hurt");
        }
        else
        {
            anim.Play("Hurt2");
        }
        StartCoroutine(Death());
    }

    // Update is called once per frame
    void Update()
    {

    }
}
