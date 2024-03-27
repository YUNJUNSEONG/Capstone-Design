using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dragon : Monster
{
    public enum State
    {
        Idle,
        Chase,
        Attack,
        meleeAttack,
        GetHit,
        Die
    }
    private State state = State.Idle;

    public float chaseDis = 30.0f;
    public float attackDis = 10.0f;
    public float meleeAttackDis = 0.5f;

    protected override IEnumerator CheckState()
    {
        while (!isDead)
        {
            yield return new WaitForSeconds(0.2f);

            float dist = Vector3.Distance(player.position, transform.position);

            if (dist <= attackDis && dist >= meleeAttackDis)
            {
                state = State.Attack;
            }
            else if (dist <= meleeAttackDis)
            {
                state = State.meleeAttack;
            }
            else if (dist <= chaseDis)
            {
                state = State.Chase;
            }
            else if (isDamage)
            {
                state = State.GetHit;
            }
            else
            {
                state = State.Idle;
            }
        }
    }

    protected override IEnumerator CheckStateForAction()
    {
        while (!isDead)
        {
            switch (state)
            {
                case State.Idle:
                    nav.isStopped = true;
                    anim.SetBool("isChase", false);
                    break;

                case State.Chase:
                    nav.destination = player.position;
                    nav.isStopped = false;
                    anim.SetBool("isChase", true);
                    break;

                case State.Attack:
                    nav.isStopped = true;
                    Targeting();
                    break;

            }

            yield return null;
        }
    }

    void FixedUpdate()
    {
        Targeting();
    }


    void Targeting()
    {
        float targetRadius = 1.5f;
        float targetRange = 3f;

        RaycastHit[] rayHits = Physics.SphereCastAll(transform.position, targetRadius, transform.forward, targetRange, LayerMask.GetMask("Player"));

        if (rayHits.Length > 0 && state == State.Attack)
        {
            StartCoroutine(Attack());
        }
    }

    IEnumerator Attack()
    {
        state = State.Attack;
        anim.SetTrigger("isAttack");
        yield return new WaitForSeconds(WAIT_TIME);
        attackArea.enabled = true;

        yield return new WaitForSeconds(mobStat.atk_anim - WAIT_TIME);
        attackArea.enabled = false;

        state = State.Chase;
        yield return new WaitForSeconds(mobStat.atk_speed);
    }
}
