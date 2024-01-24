using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleClear : MonoBehaviour
{
    public static BattleClear Instance;
    public List<MobGenerator> generators;
    public List<GameObject> portalBarrier; // 포탈배리어 게임 오브젝트
    public bool battleActive = true; // 배틀이 활성화 중인지 여부

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        StartCoroutine(CheckBattleEnd());
    }

    IEnumerator CheckBattleEnd()
    {
        while (battleActive)
        {
            // 배틀 필드에 남아있는 몬스터의 수 확인
            int remainingMonsters = CountRemainingMonsters();

            // 만약 모든 몬스터가 사라졌다면 배틀 종료
            if (remainingMonsters == 0)
            {
                Debug.Log("Battle Ended. All monsters are defeated.");
                battleActive = false;

                // 포탈배리어 비활성화
                foreach (var barrier in portalBarrier)
                {
                    Debug.Log("portal barrier disapear");
                    barrier.SetActive(false);
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
