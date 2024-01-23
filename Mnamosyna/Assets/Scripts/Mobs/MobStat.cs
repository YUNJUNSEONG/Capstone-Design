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
    protected int Max_HP; // �ִ� ü��
    [SerializeField]
    protected int Cur_HP; // ���� ü��

    [Header("���� ����")]
    // ���� ���� �Ӽ�
    [SerializeField]
    public int ATK;
    [SerializeField]
    public int Skill_ATK;
    [SerializeField]
    protected int Skill_CoolTime;


    [Header("����")]
    [SerializeField]
    protected float Defense; //  ���� n%

    [Header("�ӵ� ����")]
    [SerializeField]
    protected float ATK_Speed; // ���� �ӵ�
    [SerializeField]
    protected float Move_Speed; // �̵� �ӵ�

    public int max_hp { get { return Max_HP; } set { Max_HP = value; } }
    public int cur_hp { get { return Cur_HP; } set { Cur_HP = value; } }

    public int attack { get { return ATK; } set { ATK = value; } }
    public int skill_attack { get { return Skill_ATK; } set { Skill_ATK = value; } }
    public int skill_colltime { get { return Skill_CoolTime; } set { Skill_CoolTime = value; } }

    public float defense { get { return Defense; } set { Defense = value; } }
    public float atk_speed { get { return ATK_Speed; } set { ATK_Speed = value; } }
    public float move_speed { get { return Move_Speed; } set { Move_Speed = value; } }
}
