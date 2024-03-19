using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    public GameObject[] monsterPrefabs;//스폰할 몬스터 배열
    private Collider spawnAreaCollider;
    public int spawnCount=0;
    
    void Awake()
    {
        spawnAreaCollider = GetComponent<Collider>();
    }

    public void SpawnMonster()
    {
        if (spawnAreaCollider == null)
        {
            Debug.LogError("콜라이더 못 찾았음...");
            return;
        }

        for (int i = 0; i < spawnCount; i++)
        {
            // 겹치지 않는 위치에 몬스터 스폰
            SpawnMonsterPoint();
        }
    }

    void SpawnMonsterPoint()
    {
        Vector3 randomPoint = GetRandomPointInCollider(spawnAreaCollider);

        // 랜덤한 몬스터 프리팹 선택
        GameObject selectedMonsterPrefab = monsterPrefabs[Random.Range(0, monsterPrefabs.Length)];

        // 몬스터 프리팹을 해당 위치에 스폰
        GameObject monster = Instantiate(selectedMonsterPrefab, randomPoint, Quaternion.identity);
    }
    
    Vector3 GetRandomPointInCollider(Collider collider)
    {
        while (true)
        {
            // 콜라이더 내의 랜덤한 위치 생성
            Vector3 randomPoint = new Vector3(
                Random.Range(collider.bounds.min.x, collider.bounds.max.x),
                Random.Range(collider.bounds.min.y, collider.bounds.max.y),
                Random.Range(collider.bounds.min.z, collider.bounds.max.z)
            );

            // 아래 방향으로 레이캐스트 발사하여 충돌 체크
            RaycastHit hit;
            if (Physics.Raycast(randomPoint, Vector3.down, out hit))
            {
                // 레이캐스트가 다른 콜라이더에 충돌하면 유효하지 않은 위치로 판단
                if (hit.collider != collider)
                {
                    continue; // 다시 위치 선택
                }
            }

            // 유효한 위치가 확인되면 해당 위치 반환
            return randomPoint;
        }
    }
    
    void Update()
    {
        
    }
}
