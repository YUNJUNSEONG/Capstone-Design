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

    public float chaseDis = 150.0f;
    public float chargeDis = 10.0f;
    public float attackDis = 0.01f;
    public float rotationSpeed = 5.0f;

    public float chargeCool = 0;

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
            else if (dist <= chaseDis && dist > chargeDis && !isAttack)
            {
                state = State.Chase;
            }
            else if (isDamage)
            {
                state = State.GetHit;
            }
            else if (dist <= chargeDis && dist > attackDis)
            {
                state = State.Charge;
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
                    anim.SetBool("isChase", false);
                    break;

                case State.Chase:
                    nav.destination = player.position;
                    anim.SetBool("isChase", true);
                    break;

                case State.GetHit:
                    anim.SetBool("isChase", false);
                    anim.SetTrigger("isGetHit");
                    break;

                case State.Attack:
                    anim.SetBool("isChase", false);
                    yield return StartCoroutine(Attack());
                    break;

                case State.Charge:
                    anim.SetBool("isChase", false);
                    StartCoroutine(Charge());
                    chargeCool = mobStat.skill_colltime;
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
        float targetRadius = 0; ;
        float targetRange = 0;
        switch (state)
        {
            case State.Charge:
                targetRadius = 1f;
                targetRange = 12f;
                break;

            case State.Attack:
                targetRadius = 1.5f;
                targetRange = 2f;
                break;
        }
        RaycastHit[] rayHits = Physics.SphereCastAll(transform.position, targetRadius, transform.forward, targetRange, LayerMask.GetMask("Player"));

        if (rayHits.Length > 0 && !isAttack)
        {
            if (chargeCool <= 0)
            {
                // 스킬 사용
                state = State.Charge;
            }
            else
            {
                // 일반 공격
                state = State.Attack; ;
            }
        }
        else
        {
            state = State.Chase;
        }
    }

    IEnumerator Attack()
    {
        isChase = false;
        isAttack = true;
        Vector3 direction = (player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);

        yield return new WaitForSeconds(0.1f);
        anim.SetBool("isAttack", true); // 일반 공격 애니메이션 시작
        attackArea.enabled = true;
        Debug.Log("거미 공격");

        yield return new WaitForSeconds(mobStat.atk_anim - 0.1f);
        attackArea.enabled = false;
        anim.SetBool("isAttack", false);

        yield return new WaitForSeconds(1.0f);
        isAttack = false;
        isChase = true;
        nav.destination = player.position;
    }

    IEnumerator Charge()
    {
        isChase = false;
        isAttack = true;

        anim.SetBool("isCharge", true);
        yield return new WaitForSeconds(0.2f);
        rigid.AddForce(transform.forward * 20, ForceMode.Impulse);
        attackArea.enabled = true;

        yield return new WaitForSeconds(1.1f);
        attackArea.enabled = false;

        anim.SetBool("isCharge", false);

        yield return new WaitForSeconds(2.0f);

        isAttack = false;
        isChase = true;
    }

}
