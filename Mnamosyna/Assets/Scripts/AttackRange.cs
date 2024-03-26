using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRange : MonoBehaviour
{
    public MeleeMonster monster; 
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            //Debug.Log("공격범위");
            monster.ChangeState(MeleeMonster.MonsterState.Attack);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Player") {monster.ChangeState(MeleeMonster.MonsterState.Chase);}
    }
}
