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
    public int Max_HP; // 최대 체력
    [SerializeField]
    public int Cur_HP; // 현재 체력

    [Header("공격 관련")]
    // 공격 관련 속성
    [SerializeField]
    protected int ATK1;
    [SerializeField]
    protected int ATK2;
    [SerializeField]
    protected int NumberOfSkills;

    [Header("공격 쿨타임")]
    [SerializeField]
    protected float SkillCoolTime1;
    [SerializeField]
    protected float SkillCoolTime2;


    [Header("방어력")]
    [SerializeField]
    protected float Defense; //  방어력 n%

    [Header("속도 관련")]
    [SerializeField]
    protected float Move_Speed; // 이동 속도

    [Header("거리")]
    [SerializeField]
    protected float ApproachRadius;
    [SerializeField]
    protected float DetectionRadius;
    [SerializeField]
    protected float attack1Radius;
    [SerializeField]
    protected float attack2Radius;


    public int max_hp { get { return Max_HP; } set { Max_HP = value; } }
    public int cur_hp { get { return Cur_HP; } set { Cur_HP = value; } }

    public int skill_attack1 { get { return ATK1; } set { ATK1 = value; } }
    public int skill_attack2 { get { return ATK2; } set { ATK2 = value; } }
    public float skill_colltime1 { get { return SkillCoolTime1; } set { SkillCoolTime1 = value; } }
    public float skill_colltime2 { get { return SkillCoolTime2; } set { SkillCoolTime2 = value; } }

    public float defense { get { return Defense; } set { Defense = value; } }
    public float move_speed { get { return Move_Speed; } set { Move_Speed = value; } }

    public float approach { get { return ApproachRadius; } set { ApproachRadius = value; } }
    public float detection { get { return DetectionRadius; } set { DetectionRadius = value; } }
    public float attack1_radius { get { return attack1Radius; } set { attack1Radius = value; } }
    public float attack2_radius { get { return attack2Radius; } set { attack2Radius = value; } }
}

    

