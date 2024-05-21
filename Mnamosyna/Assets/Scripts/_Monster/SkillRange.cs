using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillRange : MonoBehaviour
{
    public Monster monster;
    public Player player;
    public CapsuleCollider attackDistance;
    public CapsuleCollider skillDistance;

    void OnTriggerStay(Collider other)
    {
        monster.ChangeState(Monster.MonsterState.Attack);

    }

    private void OnTriggerExit(Collider other)
    {
        monster.ChangeState(Monster.MonsterState.Chase);
    }

    void Awake()
    {
        attackDistance = GetComponent<CapsuleCollider>();
        skillDistance = GetComponent<CapsuleCollider>();
        SetAttackDistance();
        SetSkillDistance();
    }

    void SetAttackDistance()
    {
        attackDistance.radius = monster.attack1_radius;
    }

    void SetSkillDistance()
    {
        skillDistance.radius = monster.attack2_radius;
    }
}
