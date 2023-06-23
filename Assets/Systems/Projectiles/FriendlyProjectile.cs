using UnityEngine;

public class FriendlyProjectile : MonoBehaviour
{    
    [SerializeField] Rigidbody2D _rigidBody;

    public int Speed = 10;

    public void MoveProjectile(Vector3 targetPosition)
    {
        print("away!");
        _rigidBody.velocity = targetPosition * Speed;
    }
}
