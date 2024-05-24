using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : MonoBehaviour
{
    public static PlayerStat instance;

    [Header("체력 관련")]
    public int Max_HP = 150; // 최대 체력
    public int Cur_HP = 150; // 현재 체력
    public float HP_Recover = 0; // 1초 당 n 회복

    [Header("스태미나 관련")]
    public int Max_Stamina = 200;
    public int Cur_Stamina = 200;
    public float Stamina_Recover = 3;

    [Header("공격 관련")]
    public int MIN_ATK = 15;
    public int MAX_ATK = 20;
    public float Crit_Chance = 0;
    protected float Critical = 1.5f;

    [Header("방어력")]
    public float Defense = 0.0f; // 방어력 n%

    [Header("속도 관련")]
    public float ATK_Speed = 2.0f; // 공격 딜레이 속도
    protected float Move_Speed = 1.0f; // 이동 속도

    [Header("공격 애니메이션")]
    protected float Left_ATK_Speed;
    protected float Right_ATK_Speed;

    public float Dash_speed = 2.5f;

    public int max_hp { get { return Max_HP; } set { Max_HP = value; } }
    public int cur_hp { get { return Cur_HP; } set { Cur_HP = value; } }
    public float hp_recover { get { return HP_Recover; } set { HP_Recover = value; } }

    public int max_stamina { get { return Max_Stamina; } set { Max_Stamina = value; } }
    public int cur_stamina { get { return Cur_Stamina; } set { Cur_Stamina = value; } }
    public float stamina_recover { get { return Stamina_Recover; } set { Stamina_Recover = value; } }

    public int min_atk { get { return MIN_ATK; } set { MIN_ATK = value; } }
    public int max_atk { get { return MAX_ATK; } set { MAX_ATK = value; } }
    public float crit_chance { get { return Crit_Chance; } set { Crit_Chance = value; } }
    public float critical { get { return Critical; } set { Critical = value; } }

    public float defense { get { return Defense; } set { Defense = value; } }
    public float atk_speed { get { return ATK_Speed; } set { ATK_Speed = value; } }
    public float move_speed { get { return Move_Speed; } set { Move_Speed = value; } }

    public float left_atk_speed { get { return Left_ATK_Speed; } set { Left_ATK_Speed = value; } }
    public float right_atk_speed { get { return Right_ATK_Speed; } set { Right_ATK_Speed = value; } }

    public List<SkillData> unlockedSkills = new List<SkillData>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        ApplySkills();
    }

    private void Update()
    {
        // Apply player's skills every frame (optional: remove if not needed every frame)
        ApplySkills();
    }

    private void ApplySkills()
    {
        // Reset stats to base values
        Max_HP = 150;
        HP_Recover = 0;
        Max_Stamina = 200;
        Stamina_Recover = 3;
        MIN_ATK = 15;
        MAX_ATK = 20;
        Crit_Chance = 0;
        Critical = 1.5f;
        Defense = 0.0f;
        ATK_Speed = 2.0f;
        Move_Speed = 1.0f;

        // Apply each unlocked skill's bonuses
        foreach (SkillData skill in unlockedSkills)
        {
            if (skill.isUnlock)
            {
                ApplySkill(skill);
            }
        }

        // Clamp current HP and stamina to their new max values
        Cur_HP = Mathf.Min(Cur_HP, Max_HP);
        Cur_Stamina = Mathf.Min(Cur_Stamina, Max_Stamina);
    }

    private void ApplySkill(SkillData skill)
    {
        switch (skill.element)
        {
            case SkillData.Element.Fire:
                MIN_ATK += skill.ATKBonus;
                MAX_ATK += skill.ATKBonus;
                Crit_Chance += skill.CritChanceBonus;
                Critical += skill.CriticalBonus;
                break;
            case SkillData.Element.Air:
                ATK_Speed += skill.AttackSpeedMultiplier;
                Move_Speed += skill.MoveSpeedMultiplier;
                break;
            case SkillData.Element.Water:
                HP_Recover += skill.HealthRecoverBonus;
                Stamina_Recover += skill.StaminaRecoverBonus;
                break;
            case SkillData.Element.Earth:
                Max_HP += skill.MaxHPBonus;
                Defense += skill.DefenseBonus;
                break;
        }

        // Apply link skill specific bonuses
        if (skill.skillType == SkillData.SkillType.Link)
        {
            switch (skill.linkElement)
            {
                case SkillData.Element.Fire:
                    MIN_ATK += skill.LinkATKBonus;
                    MAX_ATK += skill.LinkATKBonus;
                    Crit_Chance += skill.LinkCritChanceBonus;
                    Critical += skill.LinkCriticalBonus;
                    break;
                case SkillData.Element.Air:
                    ATK_Speed += skill.LinkAttackSpeedMultiplier;
                    Move_Speed += skill.LinkMoveSpeedMultiplier;
                    break;
                case SkillData.Element.Water:
                    HP_Recover += skill.LinkHealthRecoverBonus;
                    Stamina_Recover += skill.LinkStaminaRecoverBonus;
                    break;
                case SkillData.Element.Earth:
                    Max_HP += skill.LinkMaxHPBonus;
                    Defense += skill.LinkDefenseBonus;
                    break;
            }
        }
    }
}
