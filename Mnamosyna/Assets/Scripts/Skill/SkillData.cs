using JetBrains.Annotations;
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
    public enum Element { Fire, Earth, Water, Air }

    [Header("# Skill Type")]
    public SkillType skillType;

    [Header("# Element")]
    public Element element;
    [ShowWhen("skillType", SkillType.Link)]
    public Element linkElement;

    [Header("# Main Info")]
    public int Id;
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
    public string Command;
    public string AnimationTrigger;
    public float AnimationTime;

    [Header("Damage")]
    public float damagePercent;
    public float addDmg;
    public float useStamina;


    [Header("# Passive Skill Data")]

    [Header("Health Related")]
    public int MaxHPBonus;
    public float HealthRecoverBonus;

    [Header("Stamina Related")]
    public float StaminaRecoverBonus;

    [Header("Attack Related")]
    public float ATKBonus;
    public float CritChanceBonus;
    public float CriticalBonus;

    [Header("Defense")]
    public float DefenseBonus;

    [Header("Speed Related")]
    public float AttackSpeedMultiplier;
    public float MoveSpeedMultiplier;


    [Header("# Link Skill Data")]
    [Header("Health Related")]
    public int LinkMaxHPBonus;
    public float LinkHealthRecoverBonus;

    [Header("Stamina Related")]
    public float LinkStaminaRecoverBonus;

    [Header("Attack Related")]
    public float LinkATKBonus;
    public float LinkCritChanceBonus;
    public float LinkCriticalBonus;

    [Header("Defense")]
    public float LinkDefenseBonus;

    [Header("Speed Related")]
    public float LinkAttackSpeedMultiplier;
    public float LinkMoveSpeedMultiplier;

}