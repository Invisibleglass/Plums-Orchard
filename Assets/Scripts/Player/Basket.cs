using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Basket : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("fruit"))
        {
            GameObject player = GetComponentInParent<PlayerController>().gameObject;
            int points = collision.GetComponent<Fruit>().points;
            GameObject.Find("GameManager").GetComponent<GameManager>().UpdateScore(player, points);
            Destroy(collision.gameObject);
        }
    }
}