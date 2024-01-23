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
            // 플레이어와 몬스터 생성 필드 간의 거리를 계산
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            // 플레이어가 일정 범위 안에 들어왔을 때 몬스터 생성 및 스테이지 클리어 확인
            if (distanceToPlayer < 10f && !stageCleared)
            {
                if (currentMonsterCount < maxMonstersToSpawn)
                {
                    SpawnMonster();
                }
                else
                {
                    // 플레이어가 설정한 값만큼 몬스터를 처치하면 스테이지 클리어
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
        // 내비게이션 메시 상의 몬스터 생성 위치
        Instantiate(mobPrefab, spawnPoint.position, Quaternion.identity);
        currentMonsterCount++;
    }

    bool PlayerClearedStage()
    {
        // 플레이어가 설정한 값만큼 몬스터를 처치했는지 확인
        // 여기에 플레이어의 공격 또는 몬스터 사망 여부를 확인하는 로직을 추가할 수 있습니다.
        // 예를 들어, 몬스터가 사망할 때마다 호출되는 콜백 함수를 사용할 수 있습니다.

        // 임시로 true를 반환하도록 설정
        return true;
    }

    void ClearStage()
    {
        // 스테이지 클리어 시 수행할 로직을 여기에 추가
        // 여기에는 모든 몬스터 삭제 등의 추가적인 게임 로직을 구현할 수 있습니다.
        // 예를 들어, 남아있는 몬스터 삭제, 다음 스테이지로 이동 등을 수행할 수 있습니다.

        // 남아있는 모든 몬스터 삭제
        GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");
        foreach (var monster in monsters)
        {
            Destroy(monster);
        }

        // 여기에 다음 스테이지로 이동하는 로직을 추가할 수 있습니다.
        // SceneManager.LoadScene("NextLevel");

        Debug.Log("Stage Cleared!");
    }*/
    }
}

