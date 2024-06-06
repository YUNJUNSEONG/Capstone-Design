
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


    [Header("# Passive Skill Data")]

    [Header("Health Related")]
    [ShowWhen("element", Element.Earth)]
    public int MaxHPBonus;
    [ShowWhen("element", Element.Earth)]
    public int addMaxHPBonus;
    [ShowWhen("element", Element.Water)]
    public float HealthRecoverBonus;
    [ShowWhen("element", Element.Water)]
    public float addHealthRecoverBonus;

    [Header("Stamina Related")]
    [ShowWhen("element", Element.Water)]
    public float StaminaRecoverBonus;
    [ShowWhen("element", Element.Water)]
    public float addStaminaRecoverBonus;

    [Header("Attack Related")]
    [ShowWhen("element", Element.Fire)]
    public int ATKBonus;
    [ShowWhen("element", Element.Fire)]
    public int addATKBonus;
    [ShowWhen("element", Element.Fire)]
    public float CritChanceBonus;
    [ShowWhen("element", Element.Fire)]
    public float addCritChanceBonus;
    [ShowWhen("element", Element.Fire)]
    public float CriticalBonus;

    [Header("Defense")]
    [ShowWhen("element", Element.Earth)]
    public float DefenseBonus;
    [ShowWhen("element", Element.Earth)]
    public float addDefenseBonus;

    [Header("Speed Related")]
    [ShowWhen("element", Element.Air)]
    public float AttackSpeedMultiplier;
    [ShowWhen("element", Element.Air)]
    public float addAttackSpeedMultiplier;
    [ShowWhen("element", Element.Air)]
    public float MoveSpeedMultiplier;
    [ShowWhen("element", Element.Air)]
    public float addMoveSpeedMultiplier;


    [Header("# Dimerit Passive Skill Data")]

    [Header("Health Related")]
    [ShowWhen("element", Element.Air)]
    public int DimeritMaxHP;
    [ShowWhen("element", Element.Fire)]
    public float DimeritHealthRecover;

    [Header("Stamina Related")]
    [ShowWhen("element", Element.Fire)]
    public float DimeritStaminaRecover;

    [Header("Attack Related")]
    [ShowWhen("element", Element.Water)]
    public int DimeritATK;
    [ShowWhen("element", Element.Water)]
    public float DimeritCritChance;
    [ShowWhen("element", Element.Water)]
    public float DimeritCritical;

    [Header("Defense")]
    [ShowWhen("element", Element.Air)]
    public float DimeritDefense;

    [Header("Speed Related")]
    [ShowWhen("element", Element.Earth)]
    public float DimeritAttackSpeedMultiplier;
    [ShowWhen("element", Element.Earth)]
    public float DimeritMoveSpeedMultiplier;


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
    public int LinkATKBonus;
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

    public static implicit operator int(SkillData v)
    {
        throw new NotImplementedException();
    }

    public void Apply(Player player)
    {
        /*
        player.MAX_ATK = Mathf.RoundToInt(player.MAX_ATK * damageMultiplier);
        player.Defense *= defenseMultiplier;
        player.Move_Speed *= speedMultiplier;
        */
    }
}