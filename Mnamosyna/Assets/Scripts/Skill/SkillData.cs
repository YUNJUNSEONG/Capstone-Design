
using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal.Internal;

[CreateAssetMenu(fileName = "skill", menuName = "ScriptableObject/skillData")]
public class SkillData : ScriptableObject
{
    public enum SkillType { Combo, Passive, Link }
    public enum Element { Fire, Air, Water, Earth }

    public enum Tier { Tier1, Tier2, Tier3, Tier4 }

    [Header("# Skill Type")]
    public SkillType skillType;

    [Header("# Element")]
    public Element element;
    [ShowWhen("skillType", SkillType.Link)]
    public Element linkElement;

    [Header("# Skill Tear")]
    public Tier tier;

    [Header("# Main Info")]
    public int Id;
    //public int ParentId;
    public string Name;
    public Sprite Image;
    [TextArea(3, 5)]
    public string Description;
    public bool isUnlock;
    public SkillData[] childSkill;


    [Header("# Level Data")]
    public int Level;
    public int maxLevel;

    [Header("# Combo Skill Data")]

    [Header("command & anim")]
    [ShowWhen("skillType", SkillType.Combo)]
    public string Command;
    [ShowWhen("skillType", SkillType.Combo)]
    public string AnimationTrigger;
    [ShowWhen("skillType", SkillType.Combo)]
    public float AnimationTime;

    [Header("Damage")]
    [ShowWhen("skillType", SkillType.Combo)]
    public float damagePercent;
    [ShowWhen("skillType", SkillType.Combo)]
    public float addDmg;
    [ShowWhen("skillType", SkillType.Combo)]
    public int useStamina;


    public static implicit operator int(SkillData v)
    {
        throw new NotImplementedException();
    }
}