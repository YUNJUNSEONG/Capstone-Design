using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
public class Dragon : Monster
{
    // 원거리 몬스터의 경우 ( 스킬 = 원거리 공격 )으로 설정 
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
    public float rotationSpeed = 5.0f;

    public bool ismeleeAttack;

    public GameObject Throw;

    protected override IEnumerator CheckState()
    {
        while (!isDead)
        {
            yield return new WaitForSeconds(0.2f);

            float dist = Vector3.Distance(player.position, transform.position);

            if (dist <= attackDis && dist > meleeAttackDis)
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
                isDamage = false;
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
                    anim.SetBool("isChase", false);
                    anim.SetTrigger("isGetHit");
                    break;

                case State.Attack:
                    anim.SetBool("isChase", false);
                    anim.SetBool("ismeleeAttack", false);
                    yield return StartCoroutine(Attack());
                    break;

                case State.meleeAttack:
                    anim.SetBool("isChase", false);
                    anim.SetBool("isAttack", false);
                    yield return StartCoroutine(meleeAttack());
                    break;

            }

            yield return null;
        }
    }

    void FixedUpdate()
    {
        Targeting();
        melee();
    }


    void Targeting()
    {
        float targetRadius = 0.5f;
        float targetRange = 12;


        RaycastHit[] rayHits = Physics.SphereCastAll(transform.position, targetRadius, transform.forward, targetRange, LayerMask.GetMask("Player"));

        // 플레이어를 감지한 경우 상태를 조정하여 공격 실행
        if (rayHits.Length > 0 && !isAttack && !ismeleeAttack)
        {
            state = State.Attack;
        }
        else
        {
            state = State.Chase;
        }
    }

    void melee()
    {
        float targetRadius = 1.5f;
        float targetRange = 2f;
        RaycastHit[] rayHits = Physics.SphereCastAll(transform.position, targetRadius, transform.forward, targetRange, LayerMask.GetMask("Player"));

        if (rayHits.Length > 0 && !isAttack && !ismeleeAttack)
        {
            state = State.meleeAttack;
        }
        else
        {
            state = State.Chase;
        }
    }

    IEnumerator Attack()
    {
        isChase = false;
        ismeleeAttack = false;
        isAttack = true;
        Vector3 direction = (player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);


        yield return new WaitForSeconds(0.5f);
        anim.SetBool("isAttack", true); // 일반 공격 애니메이션 시작
        yield return new WaitForSeconds(mobStat.skill_anim);
        GameObject Attack = Instantiate(Throw, transform.position, transform.rotation);
        Rigidbody rigidAttack = Attack.GetComponent<Rigidbody>();
        rigidAttack.velocity = transform.forward * 20;
        Debug.Log("드래곤 원거리 공격");

        yield return new WaitForSeconds(0.5f);
        anim.SetBool("isAttack", false);

        yield return new WaitForSeconds(2.0f);
        isAttack = false;
        isChase = true;
        nav.destination = player.position;
    }

    IEnumerator meleeAttack()
    {
        isChase = false;
        isAttack = false;
        ismeleeAttack = true;
        Vector3 direction = (player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);

        yield return new WaitForSeconds(0.1f);
        anim.SetBool("ismeleeAttack", true); // 일반 공격 애니메이션 시작
        attackArea.enabled = true;
        Debug.Log("드래곤 근거리 공격");

        yield return new WaitForSeconds(mobStat.atk_anim);
        attackArea.enabled = false;
        anim.SetBool("ismeleeAttack", false);

        yield return new WaitForSeconds(1.0f);
        isAttack = false;
        isChase = true;
        nav.destination = player.position;
    }

}*/
