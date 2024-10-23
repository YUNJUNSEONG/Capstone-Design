using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobStat : MonoBehaviour
{
    [Header("Monster Name")]
    [SerializeField]
    protected string Name; // ���� �̸�

    [Header("ü�� ����")]
    [SerializeField]
    public int Max_HP; // �ִ� ü��
    [SerializeField]
    public int Cur_HP; // ���� ü��

    [Header("���� ����")]
    // ���� ���� �Ӽ�
    [SerializeField]
    protected int ATK;
    [SerializeField]
    protected int Skill01;
    [SerializeField]
    protected int NumberOfSkills;

    [Header("���� ��Ÿ��")]
    [SerializeField]
    protected float AttackCoolTime;
    [SerializeField]
    protected float SkillCoolTime1;


    [Header("����")]
    [SerializeField]
    protected float Defense; //  ���� n%

    [Header("�ӵ� ����")]
    [SerializeField]
    protected float Move_Speed; // �̵� �ӵ�

    [Header("�Ÿ�")]
    [SerializeField]
    protected float ApproachRadius;
    [SerializeField]
    protected float DetectionRadius;
    [SerializeField]
    protected float AttackRadius;
    [SerializeField]
    protected float Skill01Radius;


    public int max_hp { get { return Max_HP; } set { Max_HP = value; } }
    public int cur_hp { get { return Cur_HP; } set { Cur_HP = value; } }

    public int skill_attack1 { get { return ATK; } set { ATK = value; } }
    public int skill_attack2 { get { return Skill01; } set { Skill01 = value; } }
    public float skill_colltime1 { get { return AttackCoolTime; } set { AttackCoolTime = value; } }
    public float skill_colltime2 { get { return SkillCoolTime1; } set { SkillCoolTime1 = value; } }

    public float defense { get { return Defense; } set { Defense = value; } }
    public float move_speed { get { return Move_Speed; } set { Move_Speed = value; } }

    public float approach { get { return ApproachRadius; } set { ApproachRadius = value; } }
    public float detection { get { return DetectionRadius; } set { DetectionRadius = value; } }
    public float attack_radius { get { return AttackRadius; } set { AttackRadius = value; } }
    public float skill01_radius { get { return Skill01Radius; } set { Skill01Radius = value; } }
}

    

