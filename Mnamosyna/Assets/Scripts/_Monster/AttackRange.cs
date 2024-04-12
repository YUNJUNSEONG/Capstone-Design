using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
public class AttackRange : MonoBehaviour
{
    public MeleeMonster monster;
    public PlayerMovement player;
    void OnTriggerStay(Collider other)
    {
        monster.ChangeState(MeleeMonster.MonsterState.Attack);
        
        if (monster.isAttacking)
        {
            if (other.gameObject.TryGetComponent<PlayerMovement>(out PlayerMovement player)) 
            {player.TakeDamage(15);}
        }
        
        //if (other.gameObject.tag == "Player"){monster.ChangeState(MeleeMonster.MonsterState.Attack);}
        
    }

    private void OnTriggerExit(Collider other)
    {
        monster.ChangeState(MeleeMonster.MonsterState.Chase);
        //if(other.gameObject.tag == "Player") {monster.ChangeState(MeleeMonster.MonsterState.Chase);}
    }
}*/


public class AttackRange : MonoBehaviour
{
    public Monster monster;
    public PlayerMovement player;
    public CapsuleCollider Distance;

    void OnTriggerStay(Collider other)
    {
        monster.ChangeState(Monster.MonsterState.Attack);

        if (monster.isAttack)
        {
            if (other.gameObject.TryGetComponent<PlayerMovement>(out PlayerMovement player))
            {
                player.TakeDamage(monster.Damage(0)); //monster의 데미지 0번 = 일반 공격
            }
        }

        if (other.gameObject.tag == "Player"){monster.ChangeState(Monster.MonsterState.Attack);}

    }

    private void OnTriggerExit(Collider other)
    {
        monster.ChangeState(Monster.MonsterState.Chase);
        if(other.gameObject.tag == "Player") {monster.ChangeState(Monster.MonsterState.Chase);}
    }

    void Awake()
    {
        Distance = GetComponent<CapsuleCollider>();
        SetMaxDistance();
    }

    void SetMaxDistance()
    {
        Distance.radius = monster.patrol_radius;
    }
}
