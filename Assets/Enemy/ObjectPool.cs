using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [Tooltip("green = 0, red = 1, blue = 2, yellow = 3")]
    [SerializeField] GameObject[] enemies;
    [SerializeField] [Range(0.1f, 30f)] float spawnTimer = 10f;
    int poolSize = 4;
    GameObject[] pool;

    enum EnemyType { green = 0, red = 1, blue = 2, yellow = 3 };

    void Awake() 
    {
        PopulatePool();    
    }

    void Start()
    {
        StartCoroutine(SpawnEnemy());
    }

    void PopulatePool()
    {
        pool = new GameObject[poolSize];
        for (int i = 0; i < pool.Length; i++) 
        {
            pool[i] = Instantiate(enemies[i], transform);
            pool[i].SetActive(false);

        }
    }

    void EnableObjectInPool(int indexToStart)
    {
        bool wrappedOver = false;
        for (int i = indexToStart ; i < pool.Length; i++)
        {
            if (!pool[i].activeInHierarchy)
            {
                pool[i].SetActive(true);
                return;
            }
            // if reaches end of pool, wrap over. Only does this once.
            if (!wrappedOver && i == pool.Length-1)
            {
                i = 0;
                wrappedOver = true;
            }
        }
    }

    IEnumerator SpawnEnemy()
    {
        while (Application.isPlaying)
        {
            int randNum = Random.Range(0, 4); // minInclusive, maxExclusive
            //Instantiate(enemies[randNum], transform); // spawn as child of ObjectPool in scene
            EnableObjectInPool(randNum);    
            yield return new WaitForSeconds(spawnTimer);
        }
    }
}
