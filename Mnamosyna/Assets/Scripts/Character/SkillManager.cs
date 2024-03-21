using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    private static SkillManager skillManager;

    // 스킬 배움 여부를 저장하는 변수
    public static bool skill1 = false; //스킬들 변수명 정해주세요.
    public static bool skill2 = false;
    public static bool skill3 = false;
    
    public static SkillManager Instance
    {
        get
        {
            if (skillManager == null)
            {
                skillManager = FindObjectOfType<SkillManager>();
                if (skillManager == null)
                {
                    GameObject obj = new GameObject("SkillManager");
                    skillManager = obj.AddComponent<SkillManager>();
                }
            }
            return skillManager;
        }
    }
    
    public static void SetLearnedSkill(ref bool skill, bool value)
    {
        skill = value;
    }
}
