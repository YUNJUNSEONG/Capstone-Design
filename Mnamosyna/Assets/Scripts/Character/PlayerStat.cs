using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerStat : MonoBehaviour
{
    public static PlayerStat instance;

    [Header("ü�� ����")]
    [SerializeField]
    protected int Max_HP = 150; // �ִ� ü��
    [SerializeField]
    protected int Cur_HP = 150; // ���� ü��
    [SerializeField]
    protected int HP_Recover = 0; // 1�� �� n ȸ��

    [Header("���¹̳� ����")]
    // ���׹̳� ���� �Ӽ�
    [SerializeField]
    protected int Max_Stamina = 200;
    [SerializeField]
    protected int Cur_Stamina = 200;
    [SerializeField]
    protected float Stamina_Recover = 3;

    [Header("���� ����")]
    // ���� ���� �Ӽ�
    [SerializeField]
    protected int MIN_ATK = 15;
    [SerializeField]
    protected int MAX_ATK = 20;
    [SerializeField]
    protected float Crit_Chance = 0;
    [SerializeField]
    protected float Critical = 1.5f;

    [Header("����")]
    [SerializeField]
    protected float Defense = 0.0f; //  ���� n%

    [Header("�ӵ� ����")]
    [SerializeField]
    protected float ATK_Speed = 2.0f; // ���� ������ �ӵ�
    [SerializeField]
    protected float Move_Speed = 1.0f; // �̵� �ӵ�

    [Header("���� �ִϸ��̼�")]
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
        // �нú� ��ų�� ������ �÷��̾� ���ȿ� ����
        // ��: passiveSkill���� ������ ������ �÷��̾� ���� ����
        // ��: Max_HP += passiveSkill.MaxHPBonus;
        // ��: ATK_Speed *= passiveSkill.AttackSpeedMultiplier;
    }

    public void Awake()
    {
        instance = this;
    }

}

