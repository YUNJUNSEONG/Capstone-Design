using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mimic : Monster
{

    public enum State
    {
        Idle,
        BattleIdle,
        Chase,
        Attack,
        Skill,
        GetHit,
        Die
    }
    private State state = State.Idle;

    public float chaseDis = 10.0f;
    public float attackDis = 0.1f;
    public float rotationSpeed = 5.0f;

    protected override IEnumerator CheckState()
    {
        while (!isDead)
        {
            yield return new WaitForSeconds(WAIT_TIME);

            float dist = Vector3.Distance(player.position, transform.position);

            if (isDamage)
            {
                state = State.GetHit;
            }
            else if (dist <= chaseDis && dist > attackDis && !isAttack)
            {
                state = State.Chase;
            }
            else if (isDamage)
            {
                state = State.GetHit;
                isDamage = false;
            }
            else if (dist <= attackDis)
            {
                state = State.Skill;
            }
            else if (dist > chaseDis)
            {
                state = State.Idle;
            }
            else
            {
                state = State.BattleIdle;
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
                    anim.SetBool("isIdle", true);
                    anim.SetBool("isChase", false);
                    nav.isStopped = true;
                    break;

                case State.BattleIdle:
                    anim.SetBool("isIdle", false);
                    nav.isStopped = false;
                    break;

                case State.Chase:
                    nav.destination = player.position;
                    anim.SetBool("isChase", true);
                    anim.SetBool("isIdle", false);
                    nav.isStopped = false;
                    break;

                case State.GetHit:
                    anim.SetBool("isChase", false);
                    anim.SetTrigger("isGetHit");
                    break;

                case State.Attack:
                    anim.SetBool("isIdle", false);
                    anim.SetBool("isChase", false);
                    nav.isStopped = false;
                    yield return StartCoroutine(Attack());
                    break;

                case State.Skill:
                    anim.SetBool("isIdle", false);
                    anim.SetBool("isChase", false);
                    nav.isStopped = false;
                    yield return StartCoroutine(Skill());
                    skillCool = mobStat.skill_colltime;
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
        float targetRange = 2f;

        RaycastHit[] rayHits = Physics.SphereCastAll(transform.position, targetRadius, transform.forward, targetRange, LayerMask.GetMask("Player"));

        if (rayHits.Length > 0 && !isAttack)
        {
            if (skillCool <= 0)
            {
                // 스킬 사용
                state = State.Skill;
            }
            else
            {
                // 일반 공격
                state = State.Attack;
            }
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
        Debug.Log("미믹 공격");

        yield return new WaitForSeconds(mobStat.atk_anim);
        attackArea.enabled = false;
        anim.SetBool("isAttack", false);

        yield return new WaitForSeconds(0.5f);
        isAttack = false;
        isChase = true;
        nav.destination = player.position;
    }

    IEnumerator Skill()
    {
        isChase = false;
        isAttack = true;
        Vector3 direction = (player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);

        anim.SetBool("isSkill", true); // 스킬 애니메이션 시작
        yield return new WaitForSeconds(0.1f);
        attackArea.enabled = true;

        yield return new WaitForSeconds(mobStat.skill_anim);
        attackArea.enabled = false;
        anim.SetBool("isSkill", false);

        yield return new WaitForSeconds(0.5f);
        isAttack = false;
        isChase = true;
        nav.destination = player.position;
    }
}
