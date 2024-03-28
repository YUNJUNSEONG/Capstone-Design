using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAttack : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        MeleeMonster meleeMonster = transform.parent.GetComponentInParent<MeleeMonster>();
        
        if(meleeMonster.isAttacking)
        {
            other.gameObject.GetComponent<PlayerMovement>().GetHit();
        }
    }
}
