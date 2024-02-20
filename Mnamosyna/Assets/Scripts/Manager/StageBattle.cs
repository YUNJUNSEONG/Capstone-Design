using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using static UnityEngine.Rendering.DebugUI;

[System.Serializable]
public class MobSpawnInfo
{
    public GameObject mobPrefab; // ��ȯ�� ���� ������
    public float spawnInterval; // ���͸� ��ȯ�ϴ� ����
    public int maxMobToSpawn; // ������ ������ ��
    public int curMobSpawn = 0; // ������� ������ ���� ��
}

public class StageBattle : MonoBehaviour
{
    public static StageBattle instance;

    public List<GameObject> portal; // ��Ż�踮�� ���� ������Ʈ
    public List<MobSpawnInfo> mobTypes; // ��ȯ�� ������ ����, ���� �ð�, �ִ� ��ȯ ��

    public bool battleActive = false; // ��Ʋ�� Ȱ��ȭ ������ ����

    // �ʵ��� x�� z ������ �����մϴ�. �ʵ��� �ּ� �� �ִ� x�� z ���� ����մϴ�.
    public float minX; /* �ʵ��� �ּ� x �� */
    public float maxX;  /* �ʵ��� �ִ� x �� */
    public float minZ;  /* �ʵ��� �ּ� z �� */
    public float maxZ;  /* �ʵ��� �ִ� z �� */

    void Awake()
    {
        instance = this;
    }

    public void PlayerArrived()
    {
        Debug.Log("Player has arrived at the trigger zone!");
        battleActive = true;
        StartCoroutine(Generator()); // ���� ��ȯ �ڷ�ƾ ����
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
        // �ʵ� ������ ������ x�� z ��ǥ�� �����մϴ�.
        float randomX = Random.Range(minX, maxX);
        float randomZ = Random.Range(minZ, maxZ);

        Debug.Log("���� ��ȯ ��ġ" + minX + maxX + minZ + maxZ);
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

            // ��Ʋ �ʵ忡 �����ִ� ������ �� Ȯ��
            int remainingMonsters = CountRemainingMonsters();

            // ���� ��� ���Ͱ� ������ٸ� ��Ʋ ����
            if (remainingMonsters == 0)
            {
                Debug.Log("Battle Ended. All monsters are defeated.");
                battleActive = false;

                // ��Ż Ȱ��ȭ
                foreach (var barrier in portal)
                {
                    Debug.Log("portal barrier disapear");
                    barrier.SetActive(true);
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
