using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class MonsterSpawner : MonoBehaviour
{
    public GameObject[] monsterPrefabs;//스폰할 몬스터 배열
    public Magic0[] magicComponents;
    private Collider spawnAreaCollider;
    
    public int NumberOfMonsters;
    public int spawnCount;
    public int aliveCount;
    
    void Awake()
    {
        spawnAreaCollider = GetComponent<Collider>();
        aliveCount = NumberOfMonsters;
    }

    public void SpawnMonster()
    {
        if (spawnAreaCollider == null)
        {
            Debug.LogError("콜라이더 못 찾았음...");
            return;
        }

        for (int i = 0; i < NumberOfMonsters; i++) {SpawnMonsterPoint(); }
    }

    void SpawnMonsterPoint()
    {
        Vector3 randomPoint = GetRandomPointInCollider(spawnAreaCollider);
        GameObject selectedMonsterPrefab = monsterPrefabs[Random.Range(0, monsterPrefabs.Length)];
        GameObject monster = Instantiate(selectedMonsterPrefab, randomPoint, Quaternion.identity);

        // 몬스터죽으면 어라이브카운트 줄이는 함수를 몬스터쪽에서 불러오기 위한것
        var meleeMonster = monster.GetComponent<MeleeMonster>();
        if (meleeMonster != null)
        {
            meleeMonster.spawner = this;
        }

    }
    
    public void CheckAliveCount()
    {
        if (aliveCount <= 0 && spawnCount<=0)
        {
            foreach (Magic0 magic in magicComponents) {magic.EnableComponents();}
        }
        else if (aliveCount <= 0)
        {
            SpawnMonster();
            aliveCount = NumberOfMonsters;
            spawnCount -= 1;
        }
    }
    
    Vector3 GetRandomPointInCollider(Collider collider)
    {
        int maximumAttempts = 30;
        for(int attempts = 0; attempts < maximumAttempts; attempts++)
        {
            Vector3 randomPoint = new Vector3(
                Random.Range(collider.bounds.min.x, collider.bounds.max.x),
                1f,
                Random.Range(collider.bounds.min.z, collider.bounds.max.z)
            );
        
            RaycastHit hit;
            if (Physics.Raycast(randomPoint, Vector3.down, out hit))
            {
                if (hit.collider == collider) 
                {
                    return randomPoint;
                }
            }
        }
    
        Debug.LogError("Collider 위의 위치 못찾");
        return collider.bounds.center;
    }
}
