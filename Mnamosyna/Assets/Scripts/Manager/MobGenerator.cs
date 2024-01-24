using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MobGenerator : MonoBehaviour
{
    public static MobGenerator instance;
    public GameObject mobPrefab;
    public int maxMobToSpawn; // 생성할 몬스터의 수
    public int curMobSpawn = 0; // 현재까지 생성한 몬스터 수

    public float spawnInterval; // 몬스터 생성 간격(시간)
    public int spawnMonsterCount; //한번에 생성하는 몬스터 


    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        StartCoroutine(Generator());
    }

    IEnumerator Generator()
    {
        while (curMobSpawn < maxMobToSpawn)
        {
            for (int i = 0; i < spawnMonsterCount; i++)
            {
                SpawnMonster();
            }
            yield return new WaitForSeconds(spawnInterval);
        }
    }
    void SpawnMonster()
    {
        Instantiate(mobPrefab, transform.position, Quaternion.identity);
        curMobSpawn++;
    }
}
