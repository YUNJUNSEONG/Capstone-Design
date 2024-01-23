using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobStat : MonoBehaviour
{
    [Header("Monster Name")]
    [SerializeField]
    protected string Name; // 몬스터 이름

    [Header("체력 관련")]
    [SerializeField]
    protected int Max_HP; // 최대 체력
    [SerializeField]
    protected int Cur_HP; // 현재 체력

    [Header("공격 관련")]
    // 공격 관련 속성
    [SerializeField]
    public int ATK;
    [SerializeField]
    public int Skill_ATK;
    [SerializeField]
    protected int Skill_CoolTime;


    [Header("방어력")]
    [SerializeField]
    protected float Defense; //  방어력 n%

    [Header("속도 관련")]
    [SerializeField]
    protected float ATK_Speed; // 공격 속도
    [SerializeField]
    protected float Move_Speed; // 이동 속도

    public int max_hp { get { return Max_HP; } set { Max_HP = value; } }
    public int cur_hp { get { return Cur_HP; } set { Cur_HP = value; } }

    public int attack { get { return ATK; } set { ATK = value; } }
    public int skill_attack { get { return Skill_ATK; } set { Skill_ATK = value; } }
    public int skill_colltime { get { return Skill_CoolTime; } set { Skill_CoolTime = value; } }

    public float defense { get { return Defense; } set { Defense = value; } }
    public float atk_speed { get { return ATK_Speed; } set { ATK_Speed = value; } }
    public float move_speed { get { return Move_Speed; } set { Move_Speed = value; } }
}
