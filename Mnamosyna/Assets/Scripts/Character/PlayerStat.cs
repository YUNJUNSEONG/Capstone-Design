using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerStat : MonoBehaviour
{
    public static PlayerStat instance;

    [Header("체력 관련")]
    [SerializeField]
    protected int Max_HP = 150; // 최대 체력
    [SerializeField]
    protected int Cur_HP = 150; // 현재 체력
    [SerializeField]
    protected int HP_Recover = 0; // 1초 당 n 회복

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
    public int hp_recover { get { return HP_Recover; } set { HP_Recover = value; } }

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

    public void ApplySkill(SkillData Skill)
    {
        // 패시브 스킬의 영향을 플레이어 스탯에 적용
        // 예: passiveSkill에서 가져온 정보로 플레이어 스탯 조정
        // 예: Max_HP += passiveSkill.MaxHPBonus;
        // 예: ATK_Speed *= passiveSkill.AttackSpeedMultiplier;
    }

    public void Awake()
    {
        instance = this;
    }

}

