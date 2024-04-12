using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    Animator anim;

    public bool isAttacking = false;
    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }

    /*void OnLeftAttack()
    {
        if (!isAttack){anim.SetTrigger("LeftAttack");}
        
    }*/
    public void EnableSwordCollider() {isAttacking = true;}
    public void DisableSwordCollider() {isAttacking = false;}
}
