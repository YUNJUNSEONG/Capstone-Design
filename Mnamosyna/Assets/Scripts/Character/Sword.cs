using System;
using System.Collections;
using UnityEngine;

public class Sword : MonoBehaviour
{
    public PlayerStat stat;
    public BoxCollider attackArea;
    public TrailRenderer trailEffect;

    Coroutine attackCoroutine;
    private int atk;

    public void Use(float attackTime, int Atk)
    {
        atk = Atk;
        if (attackCoroutine != null)
        {
            StopCoroutine(attackCoroutine);
        }

        attackCoroutine = StartCoroutine(Attack(attackTime, Atk));
    }


    IEnumerator Attack(float attackTime, int Atk)
    {
        atk = Atk;
        yield return new WaitForSeconds(0.1f);
        attackArea.enabled = true;
        trailEffect.enabled = true;

        yield return new WaitForSeconds(attackTime-0.1f);
        attackArea.enabled = false;
        trailEffect.enabled = false;
    }
}
