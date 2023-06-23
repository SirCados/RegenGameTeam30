using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIDamagePlayer : MonoBehaviour
{
    public PlayerHealth playerHealth;
    public int damage = 2;

    private void Start()
    {
        playerHealth = GameObject.Find("Player_prefab").GetComponent<PlayerHealth>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player") 
        {
            Debug.Log("HIT");
            playerHealth.TakeDamage(damage);
        }
    }
}
