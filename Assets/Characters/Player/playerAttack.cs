using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerAttack : MonoBehaviour
{

    public int ID;
    public float speed;
    public float shotDamage;
    public float meleeDamage;

    public Rigidbody2D fireball;
    public float fireballSpeed;
public GameObject projectile;
public Transform shotPoint;
public GameObject child;
public Transform target;

    // Start is called before the first frame update
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate() {
        ProjectileAttack();
    }

    public void ProjectileAttack() {

        bool lmb = Input.GetMouseButtonDown(0);
        
        RaycastHit hit;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit)) {
            Debug.Log("Scored a hit");
            //EnemyHealth.takeDamage(shotDamage);
        }

        

        if (lmb) {
            Debug.Log("SHOOTING with left mouse click");
            float step = fireballSpeed * 2 * Time.deltaTime;
            var fireballInst = Instantiate(fireball, transform.position, Quaternion.Euler(new Vector2(0, 0)));
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            fireballInst.velocity = worldPosition;
            
            Destroy(fireballInst, 5F);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        bool rightClick = Input.GetMouseButton(1);

        if (rightClick && collision.gameObject.tag == "Enemy") {
            Debug.Log("HITTING WITH F");
            //EnemyHealth.takeDamage(meleeDamage);
        }
    }
}
