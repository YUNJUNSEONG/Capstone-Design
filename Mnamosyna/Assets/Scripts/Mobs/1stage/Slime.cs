using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : Monster
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

    public GameObject babySlime;
    public float chaseDis = 150.0f;
    public float attackDis = 0.01f;
    public float rotationSpeed = 5.0f;

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
            else if (dist <= chaseDis && dist > attackDis && !isAttack)
            {
                state = State.Chase;
            }
            else if (isDamage)
            {
                state = State.GetHit;
                isDamage = false;
            }
            else if (dist <= attackDis )
            {
                state = State.Skill;
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

                case State.Skill:
                    anim.SetBool("isChase", false);
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

        yield return new WaitForSeconds(0.2f);
        anim.SetBool("isAttack", true); // 일반 공격 애니메이션 시작
        attackArea.enabled = true;
        Debug.Log("슬라임 공격");

        yield return new WaitForSeconds(0.84f);
        attackArea.enabled = false;
        anim.SetBool("isAttack", false);

        yield return new WaitForSeconds(1.0f);
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

        yield return new WaitForSeconds(0.84f);
        attackArea.enabled = false;
        anim.SetBool("isSkill", false);

        yield return new WaitForSeconds(1.0f);
        isAttack = false;
        isChase = true;
        nav.destination = player.position;
    }

    // 사망을 처리하는 함수
    public override void Death(Vector3 reactVec)
    {
        anim.SetTrigger("isDead");
        gameObject.layer = 11; // 사망한 몬스터의 레이어 변경
        isChase = false; // 추적 중지
        nav.enabled = false; // 네비게이션 비활성화
        reactVec = reactVec.normalized;
        reactVec += Vector3.up;
        rigid.AddForce(reactVec * 5, ForceMode.Impulse);

        Invoke("Spawn", 0.5f);

        Destroy(gameObject, 1); // 일정 시간 후 게임 오브젝트 삭제

        OnDestroy();
    }

    void Spawn()
    {
        
        Vector3 spawnPosition = transform.position; // 몬스터의 현재 위치를 기준으로 소환합니다.
        Instantiate(babySlime, spawnPosition, Quaternion.identity);
        Instantiate(babySlime, spawnPosition, Quaternion.identity); // 두 마리를 소환합니다.
    }

}
