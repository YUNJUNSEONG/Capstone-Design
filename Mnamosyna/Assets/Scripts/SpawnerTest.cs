using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerTest : MonoBehaviour
{
    public MonsterSpawner monsterSpawner;
    [SerializeField] private GameObject monsterSpawnerObj;
    void Start()
    {
        //GameObject monsterSpawnerObj = GameObject.Find("Phase-0-Unlock-Base");
        if(monsterSpawnerObj == null)
        {
            Debug.LogError("Base 오브젝트를 못찾");
            return;
        }
        monsterSpawner = monsterSpawnerObj.GetComponent<MonsterSpawner>();
        if(monsterSpawner == null)
        {
            Debug.LogError("스크립트 못찾");
            return;
        }

    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && GetComponent<Collider>().enabled)
        {
            GetComponent<Collider>().enabled = false;
            Debug.Log("몬스터스폰");
            monsterSpawner.SpawnMonster();
        }
    }
}
