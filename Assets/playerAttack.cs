using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerAttack : MonoBehaviour
{

    public int ID;
    public float speed;
    public float dmg;

    public Rigidbody2D fireball;
    public float fireballSpeed = 8f;
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

        bool lmb = Input.GetMouseButton(0);
        bool fButton = Input.GetKeyDown(KeyCode.F);
        RaycastHit hit;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit)) {
            Debug.Log("Scored a hit");
        }

        if (lmb) {
            Debug.Log("SHOOTING with left mouse click");
            float step = speed * Time.deltaTime;
         transform.position = Vector2.MoveTowards(transform.position,target.position ,step);
         var fireballInst = Instantiate(fireball, transform.position, Quaternion.Euler(new Vector2(0, 0)));
    fireballInst.velocity = new Vector2(fireballSpeed, 0);

        } else if (fButton) {
            Debug.Log("Shooting with f ");
        }
    }

    
}
