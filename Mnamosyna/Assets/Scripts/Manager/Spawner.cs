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

    //int spawnCount = 4;

    public float waitTime; // 첫 소환 이후 대기시간
    public int aliveCount;

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
        if (aliveCount <= 0 )//&& spawnCount<=0)
        {
            Vector3 playerPosition = transform.position;
            Vector3 playerForward = transform.forward;

            // 플레이어의 앞쪽으로 이동할 위치를 계산합니다.
            Vector3 spawnPosition = playerPosition + playerForward * 1 + new Vector3(1, 1, 0);

            SpawnObject(spawnPosition);
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
        Instantiate(Upgrade, spawnPosition, Quaternion.identity);
    }
}
