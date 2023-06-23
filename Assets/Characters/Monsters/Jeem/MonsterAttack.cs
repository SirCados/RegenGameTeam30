using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAttack : MonoBehaviour
{
    [SerializeField] Animator _animator;
    [SerializeField] int _damage;

    PlayerHealth _playerHealth;

    // Start is called before the first frame update
    void Start()
    {
        _animator = transform.parent.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        ResetAttackAnimation();
    }

    void AttackAnimation()
    {
        
    }

    void ResetAttackAnimation()
    {
        if (_animator.GetAnimatorTransitionInfo(0).duration == 0 && _animator.GetCurrentAnimatorStateInfo(0).IsName("Jeem02_Attack"))
        {
            print("animation reset");
            _animator.SetBool("isAttacking", false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {            
            if (!_animator.GetBool("isAttacking"))
            {
                _animator.SetBool("isAttacking", true);
                print("Have at you!");
                print(collision.gameObject.GetComponent<PlayerHealth>());
                collision.gameObject.GetComponent<PlayerHealth>().takeDamage(_damage);                
            }
        }
    }
}

/*

print("collide!");
        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<MonsterHealth>().TakeDamage(Damage);
        }
        if (!collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }

if (_animator.GetAnimatorTransitionInfo(0).duration == 0 && _animator.GetCurrentAnimatorStateInfo(0).IsName("Attack1"))
            {
                _hurtbox.EnableHurtBoxCollider();
                _animator.SetBool("Attack1", false);
                _animator.SetBool("Attack1Recovery", true);;
            } 

 */
