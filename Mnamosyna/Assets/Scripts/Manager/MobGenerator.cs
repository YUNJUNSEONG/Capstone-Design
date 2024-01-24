using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MobGenerator : MonoBehaviour
{
    public static MobGenerator instance;
    public GameObject mobPrefab;
    public int maxMobToSpawn; // ������ ������ ��
    public int curMobSpawn = 0; // ������� ������ ���� ��

    public float spawnInterval; // ���� ���� ����(�ð�)
    public int spawnMonsterCount; //�ѹ��� �����ϴ� ���� 


    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        StartCoroutine(Generator());
    }

    IEnumerator Generator()
    {
        while (curMobSpawn < maxMobToSpawn)
        {
            for (int i = 0; i < spawnMonsterCount; i++)
            {
                SpawnMonster();
            }
            yield return new WaitForSeconds(spawnInterval);
        }
    }
    void SpawnMonster()
    {
        Instantiate(mobPrefab, transform.position, Quaternion.identity);
        curMobSpawn++;
    }
}
