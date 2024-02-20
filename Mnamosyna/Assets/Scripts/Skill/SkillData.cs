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
    public float useStamina;


    [Header("# Passive Skill Data")]

    [Header("Health Related")]
    [ShowWhen("element", Element.Earth)]
    public int MaxHPBonus;
    [ShowWhen("element", Element.Water)]
    public float HealthRecoverBonus;

    [Header("Stamina Related")]
    [ShowWhen("element", Element.Water)]
    public float StaminaRecoverBonus;

    [Header("Attack Related")]
    [ShowWhen("element", Element.Fire)]
    public float ATKBonus;
    [ShowWhen("element", Element.Fire)]
    public float CritChanceBonus;
    [ShowWhen("element", Element.Fire)]
    public float CriticalBonus;

    [Header("Defense")]
    [ShowWhen("element", Element.Earth)]
    public float DefenseBonus;

    [Header("Speed Related")]
    [ShowWhen("element", Element.Air)]
    public float AttackSpeedMultiplier;
    [ShowWhen("element", Element.Air)]
    public float MoveSpeedMultiplier;


    [Header("# Link Skill Data")]

    [Header("Health Related")]
    [ShowWhen("skillType", SkillType.Link, "linkElement", Element.Earth)]
    public int LinkMaxHPBonus;
    [ShowWhen("skillType", SkillType.Link, "linkElement", Element.Water)]
    public float LinkHealthRecoverBonus;

    [Header("Stamina Related")]
    [ShowWhen("skillType", SkillType.Link, "linkElement", Element.Water)]
    public float LinkStaminaRecoverBonus;

    [Header("Attack Related")]
    [ShowWhen("skillType", SkillType.Link, "linkElement", Element.Fire)]
    public float LinkATKBonus;
    [ShowWhen("skillType", SkillType.Link, "linkElement", Element.Fire)]
    public float LinkCritChanceBonus;
    [ShowWhen("skillType", SkillType.Link, "linkElement", Element.Fire)]
    public float LinkCriticalBonus;

    [Header("Defense")]
    [ShowWhen("skillType", SkillType.Link, "linkElement", Element.Earth)]
    public float LinkDefenseBonus;

    [Header("Speed Related")]
    [ShowWhen("skillType", SkillType.Link, "linkElement", Element.Air)]
    public float LinkAttackSpeedMultiplier;
    [ShowWhen("skillType", SkillType.Link, "linkElement", Element.Air)]
    public float LinkMoveSpeedMultiplier;

}