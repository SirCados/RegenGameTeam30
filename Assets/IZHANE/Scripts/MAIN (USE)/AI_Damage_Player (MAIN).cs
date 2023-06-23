using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIDamagePlayer : MonoBehaviour
{

    [SerializeField] Animator _animator;

    public PlayerHealth playerHealth;

    public int damage = 2;

    private float TimeofAttack;
    bool CanAttack;

    

    private void Start()
    {
        playerHealth = GameObject.Find("Player_prefab").GetComponent<PlayerHealth>();
        _animator = GetComponent<Animator>();
    }
    private void Update()
    {
        ResetAttack();
    }

    void MonsterAttack()
    {
        if (CanAttack == true)
        {
            _animator.SetBool("isAttacking", true); 
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && CanAttack)
        {
            MonsterAttack();
            Debug.Log("HIT");
            CanAttack = false;
            TimeofAttack = Time.time + 5;
            
            playerHealth.takeDamage(damage);
        }

    }
    void ResetAttack()
    {
        if (Time.time == TimeofAttack)
        {
            print("reset");
            CanAttack = true; 
        }
    }
}
