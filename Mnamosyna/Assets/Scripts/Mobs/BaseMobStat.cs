using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMobStat : ScriptableObject
{
    public string MonsterName;

    [SerializeField]
    private int Max_HP; // ���� �ִ� ü��
    public int Cur_HP; // ���� ���� ü��

    [SerializeField]
    public int ATK; // ������ �⺻ ���� ������
    [SerializeField]
    public int Skill_ATK; // ������ Ư�� ���� ������
    [SerializeField]
    public float DEF; // ������ ���� n%
    [SerializeField]
    public float ATK_Speed; // ���� �ӵ�
    [SerializeField]
    public float Move_Speed; // �̵� �ӵ�
    [SerializeField]
    public int Skill_Cooltime; // Ư�� ���� ��Ÿ��

}
