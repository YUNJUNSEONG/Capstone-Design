using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossSpawner : MonoBehaviour
{
    public List<GameObject[]> waveMonsters = new List<GameObject[]>();
    public GameObject[] BossWaveMonsters;
    public GameObject[] firstWaveMonsters;
    public GameObject[] secondWaveMonsters;
    public List<int> numOfMonsters = new List<int>();
    public Canvas BossHp;

    public GameObject Upgrade;
    public Magic0[] magicComponents;
    private Collider spawnAreaCollider;

    public float waitTime;
    public int aliveCount;

    public GameObject bossMonsterPrefab;
    public Transform bossSpawnPoint;
    private GameObject bossMonsterInstance;

    public Camera mainCamera;
    public Transform bossCameraPosition;
    private Vector3 initialCameraPosition;
    private Quaternion initialCameraRotation;

    public float cameraShowDuration = 2f; // 카메라가 보스를 보여주는 시간

    public delegate void CombatEndHandler();
    public event CombatEndHandler OnCombatEnd;

    public bool isCombatEnded { get; set; } = false;
    private bool hasSpawnedUpgrade = false;

    public delegate void AliveCountChangedHandler(int newAliveCount);
    public event AliveCountChangedHandler OnAliveCountChanged;

    void Awake()
    {
        spawnAreaCollider = GetComponent<Collider>();
        aliveCount = GetTotalNumOfMonsters();
        waveMonsters.Add(BossWaveMonsters);
        waveMonsters.Add(firstWaveMonsters);
        waveMonsters.Add(secondWaveMonsters);
        BossHp = GetComponent<Canvas>();
        BossHp.gameObject.SetActive(true);

        initialCameraPosition = mainCamera.transform.position;
        initialCameraRotation = mainCamera.transform.rotation;
    }

    public void SpawnBossAtStart()
    {
        bossMonsterInstance = Instantiate(bossMonsterPrefab, bossSpawnPoint.position, Quaternion.identity);
        bossMonsterInstance.GetComponent<Monster>().OnDeath += HandleBossDeath;
        StartCoroutine(ShowBossAndStartCombat());
    }

    IEnumerator ShowBossAndStartCombat()
    {
        yield return MoveCameraToBoss();
        yield return new WaitForSeconds(cameraShowDuration);
        ResetCamera();
        SpawnWaves();
    }

    IEnumerator MoveCameraToBoss()
    {
        float elapsedTime = 0f;
        Vector3 startPos = mainCamera.transform.position;
        Quaternion startRot = mainCamera.transform.rotation;

        while (elapsedTime < 1f)
        {
            mainCamera.transform.position = Vector3.Lerp(startPos, bossCameraPosition.position, elapsedTime);
            mainCamera.transform.rotation = Quaternion.Lerp(startRot, bossCameraPosition.rotation, elapsedTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        mainCamera.transform.position = bossCameraPosition.position;
        mainCamera.transform.rotation = bossCameraPosition.rotation;
    }

    void ResetCamera()
    {
        mainCamera.transform.position = initialCameraPosition;
        mainCamera.transform.rotation = initialCameraRotation;
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

            var Monster = monster.GetComponent<Monster>();
            if (Monster != null)
            {
                Monster.OnDeath += HandleMonsterDeath;
            }
        }
    }

    void HandleBossDeath()
    {
        DestroyAllRegularMonsters();
    }

    void DestroyAllRegularMonsters()
    {
        foreach (GameObject monster in GameObject.FindGameObjectsWithTag("Monster"))
        {
            Destroy(monster);
        }
    }

    void HandleMonsterDeath()
    {
        aliveCount--;
        NotifyAliveCountChanged();
        CheckAliveCount();
    }

    public void CheckAliveCount()
    {
        if (aliveCount <= 0)
        {
            OnCombatEnd?.Invoke();
            Vector3 playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position;
            Vector3 playerForward = GameObject.FindGameObjectWithTag("Player").transform.forward;

            Vector3 spawnOffset = new Vector3(3.0f, 0, 0);
            Vector3 spawnPosition = playerPosition + playerForward * spawnOffset.x;
            spawnPosition.y = 1.0f;

            SpawnObject(spawnPosition);
            isCombatEnded = true;
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

        Debug.LogError("Failed to find point within collider bounds");
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
