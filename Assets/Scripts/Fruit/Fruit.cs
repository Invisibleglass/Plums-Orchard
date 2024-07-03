using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruit : MonoBehaviour
{
    public int points;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("fruitKiller"))
        {
            Destroy(gameObject);
        }
    }
}
