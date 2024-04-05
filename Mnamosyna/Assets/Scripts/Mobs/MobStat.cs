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
    protected int ATK;
    [SerializeField]
    protected int Skill_ATK1;
    [SerializeField]
    protected int Skill_ATK2;
    [SerializeField]
    protected int NumberOfSkills;

    [Header("공격 쿨타임")]
    [SerializeField]
    protected float SkillCoolTime1;
    [SerializeField]
    protected float SkillCoolTime2;
    [SerializeField]
    protected float SkillCoolTime3;


    [Header("방어력")]
    [SerializeField]
    protected float Defense; //  방어력 n%

    [Header("속도 관련")]
    [SerializeField]
    protected float Move_Speed; // 이동 속도

    [Header("거리")]
    [SerializeField]
    protected float distance;
    [SerializeField]
    protected float patrolRadius = 20f;
    [SerializeField]
    protected float attack1Radius;
    [SerializeField]
    protected float attack2Radius;
    [SerializeField]
    protected float attack3Radius;


    public int max_hp { get { return Max_HP; } set { Max_HP = value; } }
    public int cur_hp { get { return Cur_HP; } set { Cur_HP = value; } }

    public int attack { get { return ATK; } set { ATK = value; } }
    public int skill_attack1 { get { return Skill_ATK1; } set { Skill_ATK1 = value; } }
    public int skill_attack2 { get { return Skill_ATK2; } set { Skill_ATK2 = value; } }
    public float skill_colltime1 { get { return SkillCoolTime1; } set { SkillCoolTime1 = value; } }
    public float skill_colltime2 { get { return SkillCoolTime2; } set { SkillCoolTime2 = value; } }
    public float skill_colltime3 { get { return SkillCoolTime3; } set { SkillCoolTime3 = value; } }

    public float defense { get { return Defense; } set { Defense = value; } }
    public float move_speed { get { return Move_Speed; } set { Move_Speed = value; } }

    public float patrol_radius { get { return patrolRadius; } set { patrolRadius = value; } }
    public float attack1_radius { get { return attack1Radius; } set { attack1Radius = value; } }
    public float attack2_radius { get { return attack2Radius; } set { attack3Radius = value; } }
    public float attack3_radius { get { return attack2Radius; } set { attack3Radius = value; } }
}
    

