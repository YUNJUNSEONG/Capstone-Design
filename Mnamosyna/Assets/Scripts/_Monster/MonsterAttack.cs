using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAttack : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        Monster monster = transform.parent.GetComponentInParent<Monster>();
        
        if(monster.isAttack)
        {
            other.gameObject.GetComponent<Player>().GetHit();
        }
    }
}
