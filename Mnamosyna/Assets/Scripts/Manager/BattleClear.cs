using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleClear : MonoBehaviour
{
    public static BattleClear Instance;
    public List<MobGenerator> generators;
    public List<GameObject> portalBarrier; // ��Ż�踮�� ���� ������Ʈ
    public bool battleActive = true; // ��Ʋ�� Ȱ��ȭ ������ ����

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
            // ��Ʋ �ʵ忡 �����ִ� ������ �� Ȯ��
            int remainingMonsters = CountRemainingMonsters();

            // ���� ��� ���Ͱ� ������ٸ� ��Ʋ ����
            if (remainingMonsters == 0)
            {
                Debug.Log("Battle Ended. All monsters are defeated.");
                battleActive = false;

                // ��Ż�踮�� ��Ȱ��ȭ
                foreach (var barrier in portalBarrier)
                {
                    Debug.Log("portal barrier disapear");
                    barrier.SetActive(false);
                }

                // ���⿡ �ʿ��� ���� ó�� ���� �߰�
                // ���� ���, ���� ���� ó���� ���� �ܰ�� ��ȯ�ϴ� ������ �߰��� �� �ֽ��ϴ�.
            }

            yield return null;
        }
    }

    int CountRemainingMonsters()
    {
        // "Monster" �±׸� ���� ��� ���͸� ã�� ���� ��ȯ
        GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");
        return monsters.Length;
    }
}
