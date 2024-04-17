using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UnlockSkill : MonoBehaviour
{
    public GameObject LevelupUI;
    SkillManager skillManager; // �浹�� ������Ʈ���� ��ų �Ŵ��� ������Ʈ ��������

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>(); // �浹�� ������Ʈ���� �÷��̾� ������Ʈ ��������
            if (player != null)
            {
                OpenUnlockUpUI(skillManager);
            }
        }
    }

    public void OpenUnlockUpUI(SkillManager skillManager)
    {
        // ��ų �Ŵ����� LevelUp �޼��带 ȣ���մϴ�.
        skillManager.Unlock();
    }
}