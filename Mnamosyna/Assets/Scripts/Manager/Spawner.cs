using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public List<GameObject[]> waveMonsters = new List<GameObject[]>(); // 웨이브별 몬스터 배열 리스트
    public GameObject[] firstWaveMonsters;
    public GameObject[] secondWaveMonsters;
    public GameObject[] thirdWaveMonsters;
    public GameObject[] forthWaveMonsters;
    public List<int> numOfMonsters = new List<int>(); // 웨이브별 몬스터 수 리스트

    public GameObject Upgrade;
    public Magic0[] magicComponents;
    private Collider spawnAreaCollider;

    public float waitTime; // 첫 소환 이후 대기시간
    public int aliveCount;

    // 전투 종료 이벤트 델리게이트 및 이벤트
    public delegate void CombatEndHandler();
    public event CombatEndHandler OnCombatEnd;

    public bool isCombatEnded { get; set; } = false;
    private bool hasSpawnedUpgrade = false;

    void Awake()
    {
        spawnAreaCollider = GetComponent<Collider>();
        aliveCount = GetTotalNumOfMonsters();
        waveMonsters.Add(firstWaveMonsters);
        waveMonsters.Add(secondWaveMonsters);
        waveMonsters.Add(thirdWaveMonsters);
        waveMonsters.Add(forthWaveMonsters);
    }

    public void SpawnWaves()
    {
        StartCoroutine(SpawnWavesCoroutine());
    }

    IEnumerator SpawnWavesCoroutine()
    {
        for (int i = 0; i < waveMonsters.Count; i++)
        {
            yield return new WaitForSeconds(0.5f);
            SpawnWave(waveMonsters[i], numOfMonsters[i]);
            yield return new WaitForSeconds(waitTime);
        }
    }

    void SpawnWave(GameObject[] waveMonsters, int numOfMonster)
    {
        for (int i = 0; i < numOfMonster; i++)
        {
            Vector3 randomPoint = GetRandomPointInCollider(spawnAreaCollider);
            GameObject selectedMonster = waveMonsters[Random.Range(0, waveMonsters.Length)];
            GameObject monster = Instantiate(selectedMonster, randomPoint, Quaternion.identity);

            // 몬스터죽으면 어라이브카운트 줄이는 함수를 몬스터쪽에서 불러오기 위한것
            var Monster = monster.GetComponent<Monster>();
            if (Monster != null)
            {
                Monster.spawner = this;
            }
        }
    }

    public void CheckAliveCount()
    {
        //aliveCount--;
        if (aliveCount <= 0)
        {
            OnCombatEnd?.Invoke(); // 모든 몬스터가 죽었을 때 이벤트 호출
            Vector3 playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position;
            Vector3 playerForward = GameObject.FindGameObjectWithTag("Player").transform.forward;

            // 플레이어의 바로 앞 위치를 계산합니다.
            Vector3 spawnOffset = new Vector3(2.0f, 0, 0); // y 값을 포함하지 않음
            Vector3 spawnPosition = playerPosition + playerForward * spawnOffset.x;
            spawnPosition.y = 1.0f; // y 값을 1로 설정

            SpawnObject(spawnPosition);
            isCombatEnded = true;
            foreach (Magic0 magic in magicComponents) { magic.EnableComponents(); }
        }
    }

    int GetTotalNumOfMonsters()
    {
        int total = 0;
        foreach (int num in numOfMonsters)
        {
            total += num;
        }
        return total;
    }

    Vector3 GetRandomPointInCollider(Collider collider)
    {
        int maximumAttempts = 30;
        for (int attempts = 0; attempts < maximumAttempts; attempts++)
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

    void SpawnObject(Vector3 spawnPosition)
    {
        // Upgrade를 생성했는지 확인하고, 한 번 생성되었다면 더 이상 생성하지 않습니다.
        if (!hasSpawnedUpgrade)
        {
            Instantiate(Upgrade, spawnPosition, Quaternion.identity);
            hasSpawnedUpgrade = true; // Upgrade를 생성했음을 표시
        }
    }
}
