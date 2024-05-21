using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockSkill : MonoBehaviour
{
    public GameObject LevelupUI;
    public StoryManager storyManager;
    private SkillManager skillManager; // �浹�� ������Ʈ���� ��ų �Ŵ��� ������Ʈ ��������

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
                // Ʃ�丮�� �޽��� ����Ʈ ���� �� ���丮 �Ŵ��� ȣ��
                List<string> skillUnlockTutorialMessages = new List<string>
                {
                    "��ų�� �����ϼ���.",
                    "��ų ���� �� ��� �����ϼ���."
                };
                storyManager.StartTutorial(StoryManager.TutorialType.SkillUnlock, skillUnlockTutorialMessages);

                // ��ų ��� UI ���� �� ��ų ��� �޼��� ȣ��
                OpenUnlockUpUI(skillManager);
            }
        }
    }

    public void OpenUnlockUpUI(SkillManager skillManager)
    {
        // ��ų �Ŵ����� Unlock �޼��带 ȣ���մϴ�.
        skillManager.Unlock();

        // ������Ʈ �ı�
        Destroy(gameObject);
    }
}
