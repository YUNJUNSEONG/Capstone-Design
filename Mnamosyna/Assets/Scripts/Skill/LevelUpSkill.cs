using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class LevelUpSkill : MonoBehaviour
{
    public GameObject LevelupUI;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>(); // �浹�� ������Ʈ���� �÷��̾� ������Ʈ ��������
            SkillManager skillManager = other.GetComponent<SkillManager>(); // �浹�� ������Ʈ���� ��ų �Ŵ��� ������Ʈ ��������
            if (player != null && skillManager != null)
            {
                OpenLevelUpUI(skillManager);
            }
        }
    }

    public void OpenLevelUpUI(SkillManager skillManager)
    {
        // ��ų �Ŵ����� LevelUp �޼��带 ȣ���մϴ�.
        skillManager.LevelUp();
    }
}
