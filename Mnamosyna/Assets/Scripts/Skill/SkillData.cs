using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "skill", menuName = "ScriptableObject/skillData")]
public class SkillData : ScriptableObject
{
    [Header("# Main Info")]
    public int skillId;
    public string skillName;
    public Sprite skillImage;
    public string skillDescription;
    public string skillCommand;
    public bool isUnlock;
    public string skillAnimationTrigger;
    public float skillAnimationTime;
    public SkillData[] childSkill;

    [Header("# Level Data")]
    public int skillLevel;
    public int maxLevel;
    public float baseDamage;
    public int ad;
}