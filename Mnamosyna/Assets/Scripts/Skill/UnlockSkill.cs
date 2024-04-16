using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UnlockSkill : MonoBehaviour
{
    public GameObject UnlockUI;
    private void OnTriggerEnter(Collider other)
    {
        Player player = other.GetComponent<Player>(); // �浹�� ������Ʈ���� �÷��̾� ������Ʈ ��������
        SkillManager skillManager = other.GetComponent<SkillManager>(); // �浹�� ������Ʈ���� ��ų �Ŵ��� ������Ʈ ��������
        if (player != null && skillManager != null)
        {
            OpenUnlockUpUI(skillManager);
        }

    }

    public void OpenUnlockUpUI(SkillManager skillManager)
    {
        // ��ų �Ŵ����� LevelUp �޼��带 ȣ���մϴ�.
        skillManager.Unlock();
    }
}