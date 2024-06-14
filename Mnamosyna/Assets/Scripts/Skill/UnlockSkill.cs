using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockSkill : MonoBehaviour
{
    public GameObject UnlockUI;
    public StoryManager storyManager;
    private SkillManager skillManager; // �浹�� ������Ʈ���� ��ų �Ŵ��� ������Ʈ ��������
    public GameObject Trigger;

    public bool UnlockSkillSelectEnd = false;
    private static bool storyShown = false;

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
                // ��ų ��� UI ���� �� ��ų ��� �޼��� ȣ��
                OpenUnlockUpUI(skillManager);
            }
        }
    }

    public void OpenUnlockUpUI(SkillManager skillManager)
    {
        // ��ų �Ŵ����� Unlock �޼��带 ȣ���մϴ�.
        skillManager.Unlock();

        Invoke("selectEnd", 1.0f);

        Destroy(gameObject);
    }

    void selectEnd()
    {
        if(skillManager.EndUnlockSkillChoice == true)
        {
            UnlockSkillSelectEnd = true;
        }
    }
}
