using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_AI : MonoBehaviour
{
    //Assign game obeject 
    public GameObject player;
    [SerializeField] Animator _animator;

    // Assign the speed of the Ai's movement
    public float speed;

    private float distance;
    // Start is called before the first frame update

    public float distanceBetween;

    void Start()
    {
        player = GameObject.Find("Player");
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!_animator.GetBool("isAttacking"))
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
        }        

        //distance = Vector2.Distance(transform.position, player.transform.position);

        //Vector2 direction = player.transform.position - transform.position;

        //direction.Normalize();
        //if (distance < distanceBetween) 
        //{
        //    transform.position = Vector2.MoveTowards(this.transform.position, player.transform.position, speed * Time.deltaTime);

            
        //}
    }
    
}
