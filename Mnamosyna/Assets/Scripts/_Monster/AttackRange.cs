using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}

//¿øº»
/*public class AttackRange : MonoBehaviour
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
