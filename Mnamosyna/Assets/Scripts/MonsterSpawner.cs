using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    public GameObject[] monsterPrefabs;//스폰할 몬스터 배열
    public Magic0[] magicComponents;
    private Collider spawnAreaCollider;
    
    public int spawnCount=0;
    public int aliveCount = 0;
    
    void Awake()
    {
        spawnAreaCollider = GetComponent<Collider>();
        aliveCount = spawnCount;
    }

    public void SpawnMonster()
    {
        if (spawnAreaCollider == null)
        {
            Debug.LogError("콜라이더 못 찾았음...");
            return;
        }

        for (int i = 0; i < spawnCount; i++) {SpawnMonsterPoint(); }
    }

    void SpawnMonsterPoint()
    {
        Vector3 randomPoint = GetRandomPointInCollider(spawnAreaCollider);
        GameObject selectedMonsterPrefab = monsterPrefabs[Random.Range(0, monsterPrefabs.Length)];
        GameObject monster = Instantiate(selectedMonsterPrefab, randomPoint, Quaternion.identity);

        // 몬스터 죽음 추적
        var meleeMonster = monster.GetComponent<MeleeMonster>();
        if (meleeMonster != null)
        {
            meleeMonster.spawner = this;
        }
        else
        {
            Debug.LogError("스포너가 몬스터 스크립트 못 가져옴");
        }
    }
    
    public void CheckAliveCount()
    {
        if (aliveCount <= 0)
        {
            foreach (Magic0 magic in magicComponents)
            {
                magic.EnableComponents();
            }
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
