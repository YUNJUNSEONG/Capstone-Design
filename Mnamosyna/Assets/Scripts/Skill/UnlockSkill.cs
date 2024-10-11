using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockSkill : MonoBehaviour
{
    public GameObject UnlockUI;
    public StoryManager storyManager;
    private SkillManager skillManager;
    public GameObject Trigger;

    public bool UnlockSkillSelectEnd = false;
    private static bool storyShown = false;

    // SkillSelectionUI�� ����
    public SkillSelectionUI skillSelectionUI;

    private void Start()
    {
        skillManager = FindObjectOfType<SkillManager>();
        if (skillManager == null)
        {
            Debug.LogError("SkillManager�� ã�� �� �����ϴ�.");
        }

        // ��ų ���� UI�� �Ҵ���� �ʾҴٸ� ã��
        if (skillSelectionUI == null)
        {
            skillSelectionUI = FindObjectOfType<SkillSelectionUI>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                // ��ų ��� UI ���� �� ��ų ��� �޼��� ȣ��
                OpenUnlockUpUI(skillManager);
            }
        }
    }

    public void OpenUnlockUpUI(SkillManager skillManager)
    {
        // ��ų ���� UI�� ������ ������ ����
        //skillSelectionUI.OpenSkillSelection();

        // ��ų �Ŵ����� Unlock �޼��带 ȣ���մϴ�.
        skillManager.Unlock();

        Invoke("SelectEnd", 1.0f); // ��ų ������ ������ ����
        Destroy(gameObject);
    }

    void SelectEnd()
    {
        if (skillManager.EndUnlockSkillChoice == true)
        {
            UnlockSkillSelectEnd = true;

            // ��ų ������ ������ ���� ���
            //skillSelectionUI.CloseSkillSelection();
        }
    }
}
