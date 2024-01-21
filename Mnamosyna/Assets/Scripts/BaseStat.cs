using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseStat : MonoBehaviour
{
    [Header("체력 관련")]
    [SerializeField]
    protected int Max_HP; // 최대 체력
    [SerializeField]
    protected int Cur_HP; // 현재 체력

    [Header("방어력")]
    [SerializeField]
    protected float Defense; //  방어력 n%

    [Header ("속도 관련")]
    [SerializeField]
    protected float ATK_Speed; // 공격 속도
    [SerializeField]
    protected float Move_Speed; // 이동 속도


    public int max_hp { get { return Max_HP; } set { Max_HP = value; } }
    public int cur_hp { get { return Cur_HP; } set { Cur_HP = value; } }
    public float defense { get { return Defense; } set { Defense = value; } }
    public float atk_speed { get { return ATK_Speed; } set { ATK_Speed = value; } }
    public float move_speed { get { return Move_Speed; } set { Move_Speed = value; } }

}
