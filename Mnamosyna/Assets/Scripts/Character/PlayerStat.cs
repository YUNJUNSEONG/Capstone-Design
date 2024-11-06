using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : MonoBehaviour
{
    public static PlayerStat instance;

    [Header("체력 관련")]
    public int Max_HP = 150; // 최대 체력
    public int Cur_HP = 150; // 현재 체력
    public float HP_Recover = 0; // 1초 당 n 회복

    [Header("스태미나 관련")]
    public int Max_Stamina = 200;
    public int Cur_Stamina = 200;
    public float Stamina_Recover = 3;

    [Header("공격 관련")]
    public float MIN_ATK = 15;
    public float MAX_ATK = 20;
    public float Crit_Chance = 0;
    protected float Critical = 1.5f;

    [Header("방어력")]
    public float Defense = 0.0f; // 방어력 n%

    [Header("속도 관련")]
    public float ATK_Speed = 1.0f; // 공격 딜레이 속도
    protected float Move_Speed = 0.5f; // 이동 속도

    public float Dash_speed = 2.5f;

    public int max_hp { get { return Max_HP; } set { Max_HP = value; } }
    public int cur_hp { get { return Cur_HP; } set { Cur_HP = value; } }
    public float hp_recover { get { return HP_Recover; } set { HP_Recover = value; } }

    public int max_stamina { get { return Max_Stamina; } set { Max_Stamina = value; } }
    public int cur_stamina { get { return Cur_Stamina; } set { Cur_Stamina = value; } }
    public float stamina_recover { get { return Stamina_Recover; } set { Stamina_Recover = value; } }

    public float min_atk { get { return MIN_ATK; } set { MIN_ATK = value; } }
    public float max_atk { get { return MAX_ATK; } set { MAX_ATK = value; } }
    public float crit_chance { get { return Crit_Chance; } set { Crit_Chance = value; } }
    public float critical { get { return Critical; } set { Critical = value; } }

    public float defense { get { return Defense; } set { Defense = value; } }
    public float atk_speed { get { return ATK_Speed; } set { ATK_Speed = value; } }
    public float move_speed { get { return Move_Speed; } set { Move_Speed = value; } }

    [SerializeField]
    private List<SkillData> unlockSkills = new List<SkillData>();

    public List<SkillData> UnlockSkills
    {
        get { return unlockSkills; }
        set { unlockSkills = value; }
    }

    private void Awake()
    {
        DontDestroyOnLoad(this);
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
