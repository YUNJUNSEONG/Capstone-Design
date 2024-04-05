//using Eliot.AgentComponents;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlyaerSkill : MonoBehaviour
{
    public int maxSkillSlots = 30;
    public SkillData[] skillSlots;

    // �÷��̾ ������ ��ų ID�� �����ϴ� ����Ʈ
    private List<int> unlockedSkillIds = new List<int>();
    // �÷��̾ ������ ��ų �����͸� �����ϴ� ����Ʈ
    private List<SkillData> unlockedSkills = new List<SkillData>();

    private void Start()
    {
        // �ʱ�ȭ: �÷��̾��� ��ų ������ �� ��ų�� ä���
        skillSlots = new SkillData[maxSkillSlots];
    }

    public void UnlockSkill(SkillData skill)
    {
        if (!unlockedSkillIds.Contains(skill.Id))
        {
            // �ش� ��ų�� ����� ��ų ID ����Ʈ�� �߰�
            unlockedSkillIds.Add(skill.Id);

            // ����� ��ų �����͸� ��ų ���Կ� �߰�
            AddSkillToSlot(skill);
            Debug.Log("Unlocked and added skill: " + skill.Name);
        }
        else
        {
            Debug.LogWarning("Skill already unlocked: " + skill.Name);
        }
    }

    // ��ų ���Կ� ��ų�� �߰��ϴ� �޼���
    public void AddSkillToSlot(SkillData skill)
    {
        // �� ������ ã�Ƽ� ��ų �߰�
        for (int i = 0; i < skillSlots.Length; i++)
        {
            if (skillSlots[i] == null)
            {
                skillSlots[i] = skill;
                Debug.Log("Skill " + skill.Name + " added to slot " + (i + 1));
                return;
            }
        }

        // ��ų ������ ���� �� ��� �޽��� ���
        Debug.LogWarning("Skill slots are full. Cannot add skill " + skill.Name);
    }
}
