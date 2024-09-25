using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossSpawner : MonoBehaviour
{
    public List<GameObject[]> waveMonsters = new List<GameObject[]>();
    public GameObject[] firstWaveMonsters;
    public GameObject[] secondWaveMonsters;
    public GameObject[] thirdWaveMonsters;    public List<int> numOfMonsters = new List<int>();


    public GameObject Upgrade;
    public Magic0[] magicComponents;
    private Collider spawnAreaCollider;

    public float waitTime;
    public int aliveCount;

    public delegate void CombatEndHandler();
    public event CombatEndHandler OnCombatEnd;

    public bool isCombatEnded { get; set; } = false;
    private bool hasSpawnedUpgrade = false;

    public delegate void AliveCountChangedHandler(int newAliveCount);
    public event AliveCountChangedHandler OnAliveCountChanged;

    // ���� �߰��� ����Ʈ�� �� ���̺��� ����ִ� ���͸� ����
    private List<GameObject> aliveMonsters = new List<GameObject>();

    void Awake()
    {
        spawnAreaCollider = GetComponent<Collider>();
        aliveCount = GetTotalNumOfMonsters();
        waveMonsters.Add(firstWaveMonsters);
        waveMonsters.Add(secondWaveMonsters);
        waveMonsters.Add(thirdWaveMonsters);
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
                Monster.bossSpawner = this;
            }

            // ������ ���͸� ����Ʈ�� �߰�
            aliveMonsters.Add(monster);
        }
    }

    // ù ��° ���̺갡 ����ϸ� �ٸ� ���͵� ��� ��� ó��
    public void CheckAliveCount()
    {
        if (aliveCount <= 0)
        {
            KillAllRemainingMonsters(); // ��� ���� ���͸� ��� ó��
            OnCombatEnd?.Invoke();
            Vector3 playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position;
            Vector3 playerForward = GameObject.FindGameObjectWithTag("Player").transform.forward;

            Vector3 spawnOffset = new Vector3(3.0f, 0, 0);
            Vector3 spawnPosition = playerPosition + playerForward * spawnOffset.x;
            spawnPosition.y = 1.0f;

            SpawnObject(spawnPosition);
            isCombatEnded = true;

            foreach (Magic0 magic in magicComponents)
            {
                magic.EnableComponents();
            }
        }
    }

    // �����ִ� ��� ���͸� �����ϴ� �޼���
    void KillAllRemainingMonsters()
    {
        foreach (var monster in aliveMonsters)
        {
            if (monster != null)
            {
                Destroy(monster); // ��� ���͸� �ı�
            }
        }
        aliveMonsters.Clear(); // ����Ʈ ����
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

        Debug.LogError("Collider ���� ��ġ�� ã�� ����");
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
