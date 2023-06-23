using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_RandomSpawner : MonoBehaviour
{
    // Start is called before the first frame update
    //reference to the monster object
    [SerializeField] GameObject _monsterPrefab;
    //container for the different possible spawn points
    [SerializeField] GameObject[] _spawnPointArray;
    [SerializeField] float _spawnTimer; 

    // Start is called before the first frame update
    void Start()
    {
        //Calls the SpawnMonster() method x seconds after starting and every y seconds after that.
        InvokeRepeating("SpawnMonster", 1f, _spawnTimer);

    }

    void SpawnMonster()
    {
        //gets a random integer for the array's index, using 0 as the starting point and using the length of the array - 1 as the cap.
        int randomIndex = Random.Range(0, _spawnPointArray.Length);
        //spawns the monster at the randomly chosen spawn point's transform
        GameObject monster = Instantiate(_monsterPrefab, _spawnPointArray[randomIndex].transform);

        monster.transform.parent = monster.transform.parent.transform.parent;

    }
}
