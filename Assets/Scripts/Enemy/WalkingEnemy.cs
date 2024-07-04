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

    [Header("Sounds")]
    public AudioClip hitSound;
    public AudioClip spawnSound;
    [Header("HitMarker")]
    public GameObject hitMarker;
    [Header("Varibles")]
    public float speed;
    public float changeDirectionIntervalMin;
    public float changeDirectionIntervalMax;
    public float deathTime;
    public int points;
    public int damagePoints;

    // Start is called before the first frame update
    private void Start()
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

        // Start coroutine for changing direction
        StartCoroutine(ChangeDirectionRoutine());
    }

    protected virtual IEnumerator ChangeDirectionRoutine()
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
        GetComponent<BoxCollider2D>().excludeLayers += LayerMask.GetMask("Player");
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

    public void SpawnHitMarker(PlayerController player)
    {
        hitMarker.SetActive(true);
        // Get the direction to the target
        Vector3 direction = player.hitMarkerHelper.transform.position - hitMarker.transform.position;
        direction.z = 0; // Ensure no rotation around the Z-axis

        // Calculate the rotation to look at the target
        Quaternion rotation = Quaternion.LookRotation(Vector3.forward, direction);

        // Apply the rotation
        hitMarker.transform.rotation = rotation;
    }

    protected void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("player"))
        {
            collision.gameObject.GetComponent<PlayerController>().Ouch();
            FindObjectOfType<SoundManager>().PlayOneShot(hitSound);
            GameObject.Find("GameManager").GetComponent<GameManager>().UpdateScore(collision.gameObject, damagePoints);
            StopCoroutine(ChangeDirectionRoutine());
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
            StartCoroutine(ChangeDirectionRoutine());
        }
    }


}
