using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerPos : MonoBehaviour
{

    public GameObject player;
    public Vector2 playerPosition;
    public Transform target;
    public Vector3 dirNearPlayer;
    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate() {
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        dirNearPlayer = new Vector3(1,0,0);
        playerPosition = target.position + dirNearPlayer;
        Debug.Log(playerPosition);
    }
}
