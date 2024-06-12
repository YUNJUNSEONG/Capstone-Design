using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public Animator anim;
    public Player player;
    public bool isAttacking = false;

    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
        player = GetComponentInChildren<Player>();
    }


    public void EnableSwordCollider() { isAttacking = true; }
    public void DisableSwordCollider() { isAttacking = false; }
}

