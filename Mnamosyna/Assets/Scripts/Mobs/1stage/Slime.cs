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
    public float chaseDis = 15.0f;
    public float attackDis = 0.5f;
    public bool isAttack;
    int skillCool = 0;



    protected override IEnumerator CheckState()
    {
        while (!isDead)
        {
            yield return new WaitForSeconds(WAIT_TIME);

            float dist = Vector3.Distance(player.position, transform.position);

            if (dist <= attackDis && skillCool != 0)
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
            else if (dist <= attackDis && skillCool == 0)
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

                case State.Skill:
                    nav.isStopped = true;
                    UseSkill();
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
            // ����: ���� ó���� �ߴ����� ����
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

    void UseSkill()
    {
        float targetRadius = 1.5f;
        float targetRange = 2f;

        RaycastHit[] rayHits = Physics.SphereCastAll(transform.position, targetRadius, transform.forward, targetRange, LayerMask.GetMask("Player"));

        if (rayHits.Length > 0 && !isAttack)
        {
            StartCoroutine(Skill());
        }
    }

    IEnumerator Skill()
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

    // ����� ó���ϴ� �Լ�
    public override void Death(Vector3 reactVec)
    {

        anim.SetTrigger("isDead");
        gameObject.layer = 11; // ����� ������ ���̾� ����
        isChase = false; // ���� ����
        nav.enabled = false; // �׺���̼� ��Ȱ��ȭ
        reactVec = reactVec.normalized;
        reactVec += Vector3.up;
        rigid.AddForce(reactVec * 5, ForceMode.Impulse);

        Invoke("Spawn", 1.5f);

        Destroy(gameObject, 1); // ���� �ð� �� ���� ������Ʈ ����
        OnDestroy();
    }

    void Spawn()
    {
        
        Vector3 spawnPosition = transform.position; // ������ ���� ��ġ�� �������� ��ȯ�մϴ�.
        Instantiate(babySlime, spawnPosition, Quaternion.identity);
        Instantiate(babySlime, spawnPosition, Quaternion.identity); // �� ������ ��ȯ�մϴ�.
    }

}
