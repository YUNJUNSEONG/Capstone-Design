using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Salamander : Monster
{
    public enum State
    {
        Idle,
        Chase,
        Attack,
        Skill,
        GetHit,
        Die
    }

    private State state = State.Idle;

    public float chaseDis = 30.0f;
    public float attackDis = 0.1f;
    public bool isAttack;


    protected override IEnumerator CheckState()
    {
        while (!isDead)
        {
            yield return new WaitForSeconds(WAIT_TIME);

            float dist = Vector3.Distance(player.position, transform.position);

            if (dist <= attackDis)
            {
                state = State.Attack;
            }
            else if (dist <= chaseDis && dist >attackDis)
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
                case State.GetHit:
                    nav.isStopped = true;
                    anim.SetBool("isChase", false);
                    anim.SetTrigger("isGetHit");
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
        FreezeVelocity();
        Targeting();
    }

    void FreezeVelocity()
    {
        if (state == State.Chase)
        {
            // 변경: 물리 처리를 중단하지 않음
            rigid.velocity = Vector3.zero;
            rigid.angularVelocity = Vector3.zero;
        }
    }

    void Targeting()
    {
        float targetRadius = 1.5f;
        float targetRange = 2f;

        RaycastHit[] rayHits = Physics.SphereCastAll(transform.position, targetRadius, transform.forward, targetRange, LayerMask.GetMask("Player"));

        if (rayHits.Length > 0 && !isAttack)
        {
            StartCoroutine(Attack());
        }
    }

    IEnumerator Attack()
    {
        isChase = false;
        isAttack = true;
        anim.SetBool("isAttack", true);
        yield return new WaitForSeconds(0.2f);
        attackArea.enabled = true;

        yield return new WaitForSeconds(1.0f);
        attackArea.enabled = false;

        yield return new WaitForSeconds(2.0f);

        isAttack = false;
        isChase = true;
        anim.SetBool("isAttack", false);
    }

}
