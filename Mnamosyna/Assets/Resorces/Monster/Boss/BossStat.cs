using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStat : MonoBehaviour
{

    [Header("Boss Name")]
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
    protected int Skill02;
    [SerializeField]
    protected int Skill03;
    [SerializeField]
    protected int NumberOfSkills;

    [Header("���� ��Ÿ��")]
    [SerializeField]
    protected float AttackCoolTime;
    [SerializeField]
    protected float SkillCoolTime1;
    [SerializeField]
    protected float SkillCoolTime2;
    [SerializeField]
    protected float SkillCoolTime3;


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

    public int max_hp { get { return Max_HP; } set { Max_HP = value; } }
    public int cur_hp { get { return Cur_HP; } set { Cur_HP = value; } }

    public int attack { get { return ATK; } set { ATK = value; } }
    public int skill01 { get { return Skill01; } set { Skill01 = value; } }
    public int skill02 { get { return Skill02; } set { Skill02 = value; } }
    public int skill03 { get { return Skill03; } set { Skill03 = value; } }

    public float attack_colltime { get { return AttackCoolTime; } set { AttackCoolTime = value; } }
    public float skill_colltime01 { get { return SkillCoolTime1; } set { SkillCoolTime1 = value; } }
    public float skill_colltime02 { get { return SkillCoolTime2; } set { SkillCoolTime2 = value; } }
    public float skill_colltime03 { get { return SkillCoolTime3; } set { SkillCoolTime3 = value; } }


    public float defense { get { return Defense; } set { Defense = value; } }
    public float move_speed { get { return Move_Speed; } set { Move_Speed = value; } }


    public float approach { get { return ApproachRadius; } set { ApproachRadius = value; } }
    public float detection { get { return DetectionRadius; } set { DetectionRadius = value; } }
    public float attackRadius { get { return AttackRadius; } set { AttackRadius = value; } }

}
