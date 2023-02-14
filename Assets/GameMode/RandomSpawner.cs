using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpawner : MonoBehaviour
{
    public Transform[] _spawnPoints;
    public GameObject[] _enemyPrefabs;
    public int _spawnTimer;
    public static int _spawnCount = 0;

    // Update is called once per frame
    void FixedUpdate()
    {
        if(_spawnCount < 3)
        {

            int _randEnemy = Random.Range(0, _enemyPrefabs.Length);
            int _randSpawnPoint = Random.Range(0, _spawnPoints.Length);

            Instantiate(_enemyPrefabs[_randEnemy], _spawnPoints[_randSpawnPoint].position, transform.rotation);
            _spawnCount++;
        }
    }


   
}
