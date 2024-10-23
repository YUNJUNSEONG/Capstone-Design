using System;
using System.Collections;
using UnityEngine;
/*
public class Sword : MonoBehaviour
{
    public Player player;
    public Collider attackArea;
    //public TrailRenderer trailEffect;

    private void Awake()
    {
        attackArea = GetComponentInChildren<Collider>();
    }

    /*
    public void Use(float attackTime, int Atk)
    {
        StartCoroutine(Attack(attackTime, Atk));
    }

    private IEnumerator Attack(float attackTime, int Atk)
    {
        attackArea.enabled = true;
        //trailEffect.enabled = true;

        yield return new WaitForSeconds(attackTime / 1.5f);

        attackArea.enabled = false;
        //trailEffect.enabled = false;
    }


    private void OnTriggerStay(Collider other)
    {

        if (player.isAttack)
        {
            if (other.gameObject.TryGetComponent<Monster>(out Monster monster))
            {
                monster.TakeDamage(player.Damage()); 
            }
            else {Debug.Log("몬스터정보를 가져올수 없음");}
        }
        /*
        if (player.isAttack && other.CompareTag("Monster"))
        {
            Monster monster = other.GetComponent<Monster>();
            if (monster != null)
            {
                monster.TakeDamage(player.Damage());
            }
        }
    }
    public void EnableSwordCollider() { player.isAttack = true; }
    public void DisableSwordCollider() { player.isAttack = false; }
}    */
