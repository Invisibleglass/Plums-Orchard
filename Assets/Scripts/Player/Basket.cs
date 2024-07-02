using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Basket : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("fruit"))
        {
            //collect points from fruit by checking parent object
            Destroy(collision.gameObject);
        }
    }
}