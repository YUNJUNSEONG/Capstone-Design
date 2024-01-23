using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMobStat : ScriptableObject
{
    public string MonsterName;

    [SerializeField]
    private int Max_HP; // 몬스터 최대 체력
    public int Cur_HP; // 몬스터 현재 체력

    [SerializeField]
    public int ATK; // 몬스터의 기본 공격 데미지
    [SerializeField]
    public int Skill_ATK; // 몬스터의 특수 공격 데미지
    [SerializeField]
    public float DEF; // 몬스터의 방어력 n%
    [SerializeField]
    public float ATK_Speed; // 공격 속도
    [SerializeField]
    public float Move_Speed; // 이동 속도
    [SerializeField]
    public int Skill_Cooltime; // 특수 공격 쿨타임

}
