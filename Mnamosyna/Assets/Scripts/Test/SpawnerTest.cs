using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerTest : MonoBehaviour
{
    public MonsterSpawner monsterSpawner;
    void Start()
    {
        // MonsterSpawner 인스턴스 찾기
        //monsterSpawner = FindObjectOfType<MonsterSpawner>();
        monsterSpawner = GameObject.Find("Phase-0-Unlock-Base").GetComponent<MonsterSpawner>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        monsterSpawner.SpawnMonster();
    }
}
