using UnityEngine;

public class FriendlyProjectile : MonoBehaviour
{    
    [SerializeField] Rigidbody2D _rigidBody;

    public int Speed = 10;
    public int Damage = 1;

    public void MoveProjectile(Vector2 targetPosition)
    {
        print("away!");
        _rigidBody.velocity = targetPosition * Speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        print("collide!");
        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<MonsterHealth>().TakeDamage(Damage);
        }
        if (!collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}
