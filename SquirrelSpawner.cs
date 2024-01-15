using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquirrelSpawner : MonoBehaviour
{   

    public GameObject squirrelPrefab;
    public float minSpawnTime;
    public float maxSpawnTime;
    private float timeUntilSpawn;
    private bool spawned = false;
    
    // Start is called before the first frame update
    void Start()
    {
        SetTimeUntilSpawn();
    }

    // Update is called once per frame
    void Update()
    {
        timeUntilSpawn -= Time.deltaTime;
        float x = Random.Range(0f,100f);
        if(!spawned && x<0.1f)
        {
            print(x);
            spawned = true;
            Instantiate(squirrelPrefab,transform.position,Quaternion.identity);
            SetTimeUntilSpawn();
        }
    }

    private void SetTimeUntilSpawn(){
        timeUntilSpawn = Random.Range(minSpawnTime,maxSpawnTime);
    }
}
