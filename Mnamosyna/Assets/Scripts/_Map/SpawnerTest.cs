using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerTest : MonoBehaviour
{
    public Spawner spawner;
    //[SerializeField] private GameObject monsterSpawnerObj;
    void Start()
    {
        /*
        if(monsterSpawnerObj == null)
        {
            Debug.LogError("Base 오브젝트를 못찾");
            return;
        }
        //monsterSpawner = monsterSpawnerObj.GetComponent<MonsterSpawner>();
        */
        
        if(spawner == null)
        {
            Debug.LogError("스크립트 못찾");
            return;
        }
         
    }
    void OnTriggerEnter(Collider other)
    {
        if (GetComponent<Collider>().enabled)//other.gameObject.CompareTag("Player") && 
        {
            GetComponent<Collider>().enabled = false;
            Debug.Log("몬스터스폰");
            spawner.SpawnWaves();
            spawner.waitTime -= Time.deltaTime;
        }
    }
}
