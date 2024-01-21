using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobStat : MonoBehaviour
{
    [Header ("Monster Name")]
    [SerializeField]
    protected string MonsterName;

    [Header("체력 관련")]
    [SerializeField]
    protected int Max_HP; // 최대 체력
    [SerializeField]
    public int Cur_HP; // 현재 체력

    [Header("방어력")]
    [SerializeField]
    protected float Defense; //  방어력 n%

    [Header ("공격 관련")]
    [SerializeField]
    protected int Attack; // 몬스터의 기본 공격 데미지
    [SerializeField]
    protected int Skill_Attack; // 몬스터의 특수 공격 데미지
    [SerializeField]
    protected int Skill_Cooltime; // 특수 공격 쿨타임

    [Header("속도 관련")]
    [SerializeField]
    protected float ATK_Speed; // 공격 속도
    [SerializeField]
    protected float Move_Speed; // 이동 속도

    public int max_hp { get { return Max_HP; } set { Max_HP = value; } }
    public int cur_hp { get { return Cur_HP; } set { Cur_HP = value; } }
    public float defense { get { return Defense; } set { Defense = value; } }
    public float atk_speed { get { return ATK_Speed; } set { ATK_Speed = value; } }
    public float move_speed { get { return Move_Speed; } set { Move_Speed = value; } }
    public int attack { get { return Attack; } set { Attack = value; } }
    public int skill_attack { get { return Skill_Attack; } set { Skill_Attack = value; } }
    public int skill_colltime { get { return Skill_Cooltime; } set { Skill_Cooltime = value; } }
}
