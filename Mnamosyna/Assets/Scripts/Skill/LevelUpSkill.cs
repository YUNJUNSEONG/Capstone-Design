using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class LevelUpSkill : MonoBehaviour
{
    public GameObject LevelupUI;
    SkillManager skillManager; // �浹�� ������Ʈ���� ��ų �Ŵ��� ������Ʈ ��������

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
            Player player = other.GetComponent<Player>(); // �浹�� ������Ʈ���� �÷��̾� ������Ʈ ��������
            if (player != null)
            {
                OpenLevelUpUI(skillManager);
            }
        }
    }

    public void OpenLevelUpUI(SkillManager skillManager)
    {
        // ��ų �Ŵ����� LevelUp �޼��带 ȣ���մϴ�.
        skillManager.LevelUp();

        Destroy(gameObject);
    }
}
