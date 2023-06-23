using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPos : MonoBehaviour
{
    public GameObject player;
    public Vector2 playerPos;
    public Transform target;
    public Vector3 dirNearPlayer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        

        
    }

    void FixedUpdate() {
        GetPlayerPosition();
    }

    public void GetPlayerPosition() {
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        dirNearPlayer = new Vector3(1,0,0);
        playerPos = target.position + dirNearPlayer;
        Debug.Log(playerPos);
    }
}
