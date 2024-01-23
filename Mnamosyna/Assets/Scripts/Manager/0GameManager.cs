using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Manager
{
    public class GameManager : Singleton<GameManager>
    {
        //public static GameManager instance;
        //public GameObject mobPrefab;

        //public List<Transform> spawnPointList;

        /*public List<BlackKnight> spawnedMobList;
        public Queue<BlackKnight> respawnMobQueue;

        public int kill;
        public int needKill;

        private void Awake()
        {
            if (instance == null)
                instance = this;
            spawnedMobList = new List<BlackKnight>();
            respawnMobQueue = new Queue<BlackKnight>();

            kill = 0;
            needKill = 5;

            StartCoroutine(SpawnMob());
        }

        private void Update()
        {
            CountKill();
        }

        private void CountKill()
        {
            if (kill >= needKill)
            {
                SkillManager.instance.LevelUp();
                needKill += 5;
            }
        }

        private IEnumerator SpawnMob()
        {
            while (true)
            {
                float delay = 3.0f;
                float curDelay = 0.0f;
                while (curDelay <= delay)
                {
                    curDelay += Time.deltaTime;
                    yield return null;
                }

                int randCoor = Random.Range(0, spawnPointList.Count);
                GameObject mob = Instantiate(mobPrefab);
                mob.transform.position = spawnPointList[randCoor].position;
            
                yield return null;
            }

        } 
            IEnumerator CheckPlayerInField()
    {
        while (true)
        {
            // �÷��̾�� ���� ���� �ʵ� ���� �Ÿ��� ���
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            // �÷��̾ ���� ���� �ȿ� ������ �� ���� ���� �� �������� Ŭ���� Ȯ��
            if (distanceToPlayer < 10f && !stageCleared)
            {
                if (currentMonsterCount < maxMonstersToSpawn)
                {
                    SpawnMonster();
                }
                else
                {
                    // �÷��̾ ������ ����ŭ ���͸� óġ�ϸ� �������� Ŭ����
                    if (PlayerClearedStage())
                    {
                        ClearStage();
                        stageCleared = true;
                    }
                }
            }

            yield return null;
        }
    }
    void SpawnMonster()
    {
        // ������̼� �޽� ���� ���� ���� ��ġ
        Instantiate(mobPrefab, spawnPoint.position, Quaternion.identity);
        currentMonsterCount++;
    }

    bool PlayerClearedStage()
    {
        // �÷��̾ ������ ����ŭ ���͸� óġ�ߴ��� Ȯ��
        // ���⿡ �÷��̾��� ���� �Ǵ� ���� ��� ���θ� Ȯ���ϴ� ������ �߰��� �� �ֽ��ϴ�.
        // ���� ���, ���Ͱ� ����� ������ ȣ��Ǵ� �ݹ� �Լ��� ����� �� �ֽ��ϴ�.

        // �ӽ÷� true�� ��ȯ�ϵ��� ����
        return true;
    }

    void ClearStage()
    {
        // �������� Ŭ���� �� ������ ������ ���⿡ �߰�
        // ���⿡�� ��� ���� ���� ���� �߰����� ���� ������ ������ �� �ֽ��ϴ�.
        // ���� ���, �����ִ� ���� ����, ���� ���������� �̵� ���� ������ �� �ֽ��ϴ�.

        // �����ִ� ��� ���� ����
        GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");
        foreach (var monster in monsters)
        {
            Destroy(monster);
        }

        // ���⿡ ���� ���������� �̵��ϴ� ������ �߰��� �� �ֽ��ϴ�.
        // SceneManager.LoadScene("NextLevel");

        Debug.Log("Stage Cleared!");
    }*/
    }
}

