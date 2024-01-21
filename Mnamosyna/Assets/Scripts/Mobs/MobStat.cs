using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobStat : MonoBehaviour
{
    [Header ("Monster Name")]
    [SerializeField]
    protected string MonsterName;

    [Header("ü�� ����")]
    [SerializeField]
    protected int Max_HP; // �ִ� ü��
    [SerializeField]
    public int Cur_HP; // ���� ü��

    [Header("����")]
    [SerializeField]
    protected float Defense; //  ���� n%

    [Header ("���� ����")]
    [SerializeField]
    protected int Attack; // ������ �⺻ ���� ������
    [SerializeField]
    protected int Skill_Attack; // ������ Ư�� ���� ������
    [SerializeField]
    protected int Skill_Cooltime; // Ư�� ���� ��Ÿ��

    [Header("�ӵ� ����")]
    [SerializeField]
    protected float ATK_Speed; // ���� �ӵ�
    [SerializeField]
    protected float Move_Speed; // �̵� �ӵ�

    public int max_hp { get { return Max_HP; } set { Max_HP = value; } }
    public int cur_hp { get { return Cur_HP; } set { Cur_HP = value; } }
    public float defense { get { return Defense; } set { Defense = value; } }
    public float atk_speed { get { return ATK_Speed; } set { ATK_Speed = value; } }
    public float move_speed { get { return Move_Speed; } set { Move_Speed = value; } }
    public int attack { get { return Attack; } set { Attack = value; } }
    public int skill_attack { get { return Skill_Attack; } set { Skill_Attack = value; } }
    public int skill_colltime { get { return Skill_Cooltime; } set { Skill_Cooltime = value; } }
}
