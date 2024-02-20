using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using static UnityEngine.Rendering.DebugUI;

[System.Serializable]
public class MobSpawnInfo
{
    public GameObject mobPrefab; // 소환할 몬스터 프리팹
    public float spawnInterval; // 몬스터를 소환하는 간격
    public int maxMobToSpawn; // 생성할 몬스터의 수
    public int curMobSpawn = 0; // 현재까지 생성한 몬스터 수
}

public class StageBattle : MonoBehaviour
{
    public static StageBattle instance;

    public List<GameObject> portal; // 포탈배리어 게임 오브젝트
    public List<MobSpawnInfo> mobTypes; // 소환할 몬스터의 종류, 스폰 시간, 최대 소환 수

    public bool battleActive = false; // 배틀이 활성화 중인지 여부

    // 필드의 x와 z 범위를 결정합니다. 필드의 최소 및 최대 x와 z 값을 사용합니다.
    public float minX; /* 필드의 최소 x 값 */
    public float maxX;  /* 필드의 최대 x 값 */
    public float minZ;  /* 필드의 최소 z 값 */
    public float maxZ;  /* 필드의 최대 z 값 */

    void Awake()
    {
        instance = this;
    }

    public void PlayerArrived()
    {
        Debug.Log("Player has arrived at the trigger zone!");
        battleActive = true;
        StartCoroutine(Generator()); // 몬스터 소환 코루틴 시작
        StartCoroutine(CheckBattleEnd());
    }

    IEnumerator Generator()
    {
        foreach (MobSpawnInfo mobType in mobTypes)
        {
            while (mobType.curMobSpawn < mobType.maxMobToSpawn)
            {
                SpawnMonster(mobType);
                yield return new WaitForSeconds(mobType.spawnInterval);
            }
        }
    }

    void SpawnMonster(MobSpawnInfo mobType)
    {
        // 필드 내에서 랜덤한 x와 z 좌표를 선택합니다.
        float randomX = Random.Range(minX, maxX);
        float randomZ = Random.Range(minZ, maxZ);

        Debug.Log("몬스터 소환 위치" + minX + maxX + minZ + maxZ);
        Debug.Log("Monster Generation!");
        Vector3 spawnPosition = new Vector3(randomX, 1, randomZ);
        Instantiate(mobType.mobPrefab, spawnPosition, Quaternion.identity);
        mobType.curMobSpawn++;
    }

    IEnumerator CheckBattleEnd()
    {
        while (battleActive)
        {
            yield return new WaitForSeconds(3.0f);

            // 배틀 필드에 남아있는 몬스터의 수 확인
            int remainingMonsters = CountRemainingMonsters();

            // 만약 모든 몬스터가 사라졌다면 배틀 종료
            if (remainingMonsters == 0)
            {
                Debug.Log("Battle Ended. All monsters are defeated.");
                battleActive = false;

                // 포탈 활성화
                foreach (var barrier in portal)
                {
                    Debug.Log("portal barrier disapear");
                    barrier.SetActive(true);
                }

                // 여기에 필요한 종료 처리 로직 추가
                // 예를 들어, 게임 종료 처리나 다음 단계로 전환하는 로직을 추가할 수 있습니다.
            }

            yield return null;
        }
    }

    int CountRemainingMonsters()
    {
        // "Monster" 태그를 가진 모든 몬스터를 찾아 수를 반환
        GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");
        return monsters.Length;
    }
}
