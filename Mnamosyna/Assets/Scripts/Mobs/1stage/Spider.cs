using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spider : Monster
{
    public enum State
    {
        Idle,
        Chase,
        Attack,
        Charge,
        GetHit,
        Die
    }

    private State state = State.Idle;

    public float chaseDis = 14.0f;
    public float chargeDis = 10.0f;
    public float attackDis = 1.0f;
    public bool isAttack;


    protected override IEnumerator CheckState()
    {
        while (!isDead)
        {
            yield return new WaitForSeconds(WAIT_TIME);

            float dist = Vector3.Distance(player.position, transform.position);

            if (dist <= chargeDis)
            {
                state = State.Attack;
            }
            else if (dist <= chaseDis && dist > attackDis)
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
        float targetRadius = 1f;
        float targetRange = 12f;

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
        anim.SetBool("isCharge", true);

        yield return new WaitForSeconds(0.2f);
        rigid.AddForce(transform.forward * 20, ForceMode.Impulse);
        attackArea.enabled = true;

        yield return new WaitForSeconds(0.5f);
        rigid.velocity = Vector3.zero;
        attackArea.enabled = false;

        yield return new WaitForSeconds(4.0f);

        isAttack = false;
        isChase = true;
        anim.SetBool("isCharge", false);

        yield return new WaitForSeconds(4.0f);
    }

}
