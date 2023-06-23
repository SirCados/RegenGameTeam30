using UnityEngine;

public class MonsterHealth : MonoBehaviour
{
    public int MaxHealth = 3;
    int _currentHealth;

    // Start is called before the first frame update
    void Start()
    {
        _currentHealth = MaxHealth;
    }

    public void TakeDamage(int damageToTake)
    {
        _currentHealth -= damageToTake;
        if(_currentHealth < 1)
        {
            Destroy(gameObject);
        }
    }
}
