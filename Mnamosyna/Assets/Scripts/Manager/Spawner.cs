using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public List<GameObject[]> waveMonsters = new();
    public GameObject[] firstWaveMonsters;
    public GameObject[] secondWaveMonsters;
    public GameObject[] thirdWaveMonsters;
    public GameObject[] forthWaveMonsters;
    public List<int> numOfMonsters = new();

    public GameObject Upgrade;
    public Magic0[] magicComponents;
    public Collider spawnAreaCollider;

    public float waitTime;
    public int aliveCount;

    public GameObject babySlimePrefab; // babySlime 프리팹 추가

    public delegate void CombatEndHandler();
    public event CombatEndHandler OnCombatEnd;

    public bool IsCombatEnded { get; set; } = false;
    private bool hasSpawnedUpgrade = false;

    public delegate void AliveCountChangedHandler(int newAliveCount);
    public event AliveCountChangedHandler OnAliveCountChanged;

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

            var Monster = monster.GetComponent<BaseMonster>();
            if (Monster != null)
            {
                Monster.spawner = this;
            }
        }
    }

    public void SpawnBabySlime(Vector3 position)
    {
        if (babySlimePrefab != null)
        {
            GameObject babySlime = Instantiate(babySlimePrefab, position, Quaternion.identity);
            var monster = babySlime.GetComponent<BaseMonster>();
            if (monster != null)
            {
                monster.spawner = this;
                aliveCount++; // babySlime 소환 시 aliveCount 증가
            }
        }
    }

    public void CheckAliveCount()
    {
        if (aliveCount <= 0)
        {
            OnCombatEnd?.Invoke();
            Vector3 playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position;
            Vector3 playerForward = GameObject.FindGameObjectWithTag("Player").transform.forward;

            Vector3 spawnOffset = new(3.0f, 0, 0);
            Vector3 spawnPosition = playerPosition + playerForward * spawnOffset.x;
            spawnPosition.y = playerPosition.y + 0.8f;

            SpawnObject(spawnPosition);
            IsCombatEnded = true;
            foreach (Magic0 magic in magicComponents) { magic.EnableComponents(); }
        }
    }

    public void NotifyAliveCountChanged()
    {
        OnAliveCountChanged?.Invoke(aliveCount);
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
            Vector3 randomPoint = new(
                Random.Range(collider.bounds.min.x, collider.bounds.max.x),
                5f,
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
        if (!hasSpawnedUpgrade)
        {
            Instantiate(Upgrade, spawnPosition, Quaternion.identity);
            hasSpawnedUpgrade = true;
        }
    }
}
