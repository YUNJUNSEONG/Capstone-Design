using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAttack : MonoBehaviour
{
    public Collider SwordCollider;
    public PlayerAttack playerAttack;
    private void Awake()
    {
        SwordCollider = GetComponentInChildren<Collider>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (playerAttack.isAttacking)
        {
            if (other.gameObject.TryGetComponent<Monster>(out Monster monster)) {monster.TakeDamage(15);}
            //else {Debug.Log("몬스터정보를 가져올수 없음");}
        }
    }
    
}
