//using Eliot.AgentComponents;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlyaerSkill : MonoBehaviour
{
    public int maxSkillSlots = 30;
    public SkillData[] skillSlots;

    // 플레이어가 보유한 스킬 ID를 저장하는 리스트
    private List<int> unlockedSkillIds = new List<int>();
    // 플레이어가 보유한 스킬 데이터를 저장하는 리스트
    private List<SkillData> unlockedSkills = new List<SkillData>();

    private void Start()
    {
        // 초기화: 플레이어의 스킬 슬롯을 빈 스킬로 채우기
        skillSlots = new SkillData[maxSkillSlots];
    }

    public void UnlockSkill(SkillData skill)
    {
        if (!unlockedSkillIds.Contains(skill.Id))
        {
            // 해당 스킬을 언락한 스킬 ID 리스트에 추가
            unlockedSkillIds.Add(skill.Id);

            // 언락한 스킬 데이터를 스킬 슬롯에 추가
            AddSkillToSlot(skill);
            Debug.Log("Unlocked and added skill: " + skill.Name);
        }
        else
        {
            Debug.LogWarning("Skill already unlocked: " + skill.Name);
        }
    }

    // 스킬 슬롯에 스킬을 추가하는 메서드
    public void AddSkillToSlot(SkillData skill)
    {
        // 빈 슬롯을 찾아서 스킬 추가
        for (int i = 0; i < skillSlots.Length; i++)
        {
            if (skillSlots[i] == null)
            {
                skillSlots[i] = skill;
                Debug.Log("Skill " + skill.Name + " added to slot " + (i + 1));
                return;
            }
        }

        // 스킬 슬롯이 가득 찬 경우 메시지 출력
        Debug.LogWarning("Skill slots are full. Cannot add skill " + skill.Name);
    }
}
