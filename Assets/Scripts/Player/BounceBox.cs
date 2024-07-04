using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceBox : MonoBehaviour
{
    public PlayerController player;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("bouncable"))
        {
            player.BounceMe();
            if (collision.GetComponentInParent<WalkingEnemy>() != null)
                collision.GetComponentInParent<WalkingEnemy>().DeathAnimation();
        }
    }
}
