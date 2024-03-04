using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerStat : MonoBehaviour
{
    public static PlayerStat instance;
    public SkillData skill;

    [Header("체력 관련")]
    [SerializeField]
    protected int Max_HP = 150; // 최대 체력
    [SerializeField]
    public int Cur_HP = 150; // 현재 체력
    [SerializeField]
    protected float HP_Recover = 0; // 1초 당 n 회복

    [Header("스태미나 관련")]
    // 스테미나 관련 속성
    [SerializeField]
    protected int Max_Stamina = 200;
    [SerializeField]
    protected int Cur_Stamina = 200;
    [SerializeField]
    protected float Stamina_Recover = 3;

    [Header("공격 관련")]
    // 공격 관련 속성
    [SerializeField]
    protected int MIN_ATK = 15;
    [SerializeField]
    protected int MAX_ATK = 20;
    [SerializeField]
    protected float Crit_Chance = 0;
    [SerializeField]
    protected float Critical = 1.5f;

    [Header("방어력")]
    [SerializeField]
    protected float Defense = 0.0f; //  방어력 n%

    [Header("속도 관련")]
    [SerializeField]
    protected float ATK_Speed = 2.0f; // 공격 딜레이 속도
    [SerializeField]
    protected float Move_Speed = 1.0f; // 이동 속도

    [Header("공격 애니메이션")]
    [SerializeField]
    protected float Left_ATK_Speed;
    [SerializeField]
    protected float Right_ATK_Speed;

    public float Dash_speed = 2.5f;

    public int max_hp { get { return Max_HP; } set { Max_HP = value; } }
    public int cur_hp { get { return Cur_HP; } set { Cur_HP = value; } }
    public float hp_recover { get { return HP_Recover; } set { HP_Recover = value; } }

    public int max_stamina { get { return Max_Stamina; } set { Max_Stamina= value; } }
    public int cur_stamina { get { return Cur_Stamina; } set { Cur_Stamina = value; } }
    public float stamina_recover { get { return Stamina_Recover; } set { Stamina_Recover = value; } }

    public int min_atk { get { return MIN_ATK; } set { MIN_ATK = value; } }
    public int max_atk { get { return MAX_ATK; } set { MAX_ATK = value; } }
    public float crit_chance { get { return Crit_Chance; } set { Crit_Chance = value; } }
    public float critical { get { return Critical; } set { Critical = value; } }    

    public float defense { get { return Defense; } set { Defense = value; } }
    public float atk_speed { get { return ATK_Speed; } set { ATK_Speed = value; } }
    public float move_speed { get { return Move_Speed; } set { Move_Speed = value; } }

    public float left_atk_speed{ get { return Left_ATK_Speed; } set { Left_ATK_Speed = value; } }
    public float right_atk_speed { get { return Right_ATK_Speed; } set { Right_ATK_Speed = value; } }

    public void ApplySkill()
    {
        max_hp += skill.MaxHPBonus;
        max_hp -= skill.DimeritMaxHP;
        max_hp += skill.LinkMaxHPBonus;

        hp_recover += skill.HealthRecoverBonus;
        hp_recover -= skill.DimeritHealthRecover;
        hp_recover = skill.LinkHealthRecoverBonus;

        stamina_recover += skill.StaminaRecoverBonus;
        stamina_recover -= skill.DimeritStaminaRecover;
        stamina_recover += skill.LinkStaminaRecoverBonus;

        min_atk += skill.ATKBonus;
        min_atk -= skill.DimeritATK;
        min_atk += skill.LinkATKBonus;

        max_atk += skill.ATKBonus;
        max_atk -= skill.DimeritATK;
        max_atk += skill.LinkATKBonus;

        crit_chance += skill.CritChanceBonus;
        crit_chance -= skill.DimeritCritChance;
        crit_chance += skill.LinkCritChanceBonus;

        critical += skill.CriticalBonus;
        critical -= skill.DimeritCritical;
        critical += skill.LinkCriticalBonus;

        defense += skill.DefenseBonus;
        defense -= skill.DimeritDefense;
        defense += skill.LinkDefenseBonus;

        atk_speed += skill.AttackSpeedMultiplier;
        atk_speed -= skill.DimeritAttackSpeedMultiplier;
        atk_speed += skill.LinkAttackSpeedMultiplier;

        move_speed += skill.MoveSpeedMultiplier;
        move_speed -= skill.DimeritMoveSpeedMultiplier;
        move_speed += skill.LinkMoveSpeedMultiplier;
    }

    public void Awake()
    {
        instance = this;
    }
    public void Update()
    {
        ApplySkill();
    }

}

