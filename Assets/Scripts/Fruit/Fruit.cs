using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruit : MonoBehaviour
{
    public float Points;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("fruitKiller"))
        {
            Destroy(gameObject);
        }
    }
}
