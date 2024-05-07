using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HealSpace : MonoBehaviour
{
    public int healPlayer = 100;
    SkillManager skillManager;

    public Magic0[] magicComponents;

    private void Start()
    {
        skillManager = FindObjectOfType<SkillManager>(); // scene���� SkillManager ������Ʈ�� ã�� �Ҵ�
        if (skillManager == null)
        {
            Debug.LogError("SkillManager�� ã�� �� �����ϴ�.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            HealPlayer(other.gameObject);
        }
    }

    public void HealPlayer(GameObject playerObject)
    {
        PlayerStat player = playerObject.GetComponent<PlayerStat>();

        if (player != null)
        {
            Debug.Log("Player Heal!");
            player.cur_hp += healPlayer;
            player.cur_stamina += healPlayer;   

            OpenUnlockUpUI(skillManager, () =>
            {
                // Unlock�� �Ϸ�Ǹ� LevelUp ȣ��
                OpenLevelUpUI(skillManager);
            });

            Destroy(gameObject);

            foreach (Magic0 magic in magicComponents) { magic.EnableComponents(); }
        }
    }

    public void OpenUnlockUpUI(SkillManager skillManager, Action onUnlockComplete)
    {
        // ��ų �Ŵ����� Unlock �޼��带 ȣ���ϰ�, �Ϸ�Ǹ� �ݹ� ȣ��
        skillManager.Unlock(() =>
        {
            if (onUnlockComplete != null)
                onUnlockComplete.Invoke(); // �ݹ� ȣ��
        });
    }


    public void OpenLevelUpUI(SkillManager skillManager)
    {
        // ��ų �Ŵ����� LevelUp �޼��带 ȣ���մϴ�.
        skillManager.LevelUp();
    }
}
