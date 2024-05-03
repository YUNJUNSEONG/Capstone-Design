using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealSpace : MonoBehaviour
{
    public int healPlayer = 100;
    SkillManager skillManager;

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

            OpenLevelUpUI(skillManager);

            OpenUnlockUpUI(skillManager);


            Destroy(gameObject);
        }
    }

    public void OpenUnlockUpUI(SkillManager skillManager)
    {
        // ��ų �Ŵ����� Unlock�޼��带 ȣ���մϴ�.
        skillManager.Unlock();
    }

    public void OpenLevelUpUI(SkillManager skillManager)
    {
        // ��ų �Ŵ����� LevelUp �޼��带 ȣ���մϴ�.
        skillManager.LevelUp();
    }
}
