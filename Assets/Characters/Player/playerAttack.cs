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

    [SerializeField] GameObject _friendlyProjectile;
    [SerializeField] Camera _camera;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        SpawnProjectile();
    }

    void FixedUpdate() 
    {
        //ProjectileAttack();
        
    }

    void SpawnProjectile()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            print("Spawn");
            Vector3 screenPosition = Input.mousePosition;
            Vector3 worldPosition = _camera.ScreenToWorldPoint(screenPosition);

            //var worldPosition = _camera.ScreenToWorldPoint(screenPosition);
            GameObject projectile = Instantiate(_friendlyProjectile, transform);
            FriendlyProjectile projectileScript = projectile.GetComponent<FriendlyProjectile>();
            projectileScript.MoveProjectile(worldPosition);
        }
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
         var fireballInst = Instantiate(fireball, transform.position, Quaternion.Euler(new Vector2(0, 0)));
    Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    fireballInst.velocity = worldPosition;

        }
    } 

}
