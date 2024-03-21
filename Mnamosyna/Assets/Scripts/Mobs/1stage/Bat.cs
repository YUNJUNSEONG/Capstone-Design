using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Bat : Monster
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

    public float chaseDis = 150.0f;
    public float attackDis = 10.0f;
    public float meleeAttackDis = 2.0f;

    // �߰�: �ڷ�ƾ ������ ���� ����
    private Coroutine stateCoroutine;

    private void Start()
    {
        // ����: �ڷ�ƾ ������ �޼���� ȣ��
        StartStateCoroutines();
    }

    // ����: �ڷ�ƾ�� �����ϴ� �޼���
    void StartStateCoroutines()
    {
        stateCoroutine = StartCoroutine(CheckState());
        StartCoroutine(CheckStateForAction());
    }

    // ����: �ڷ�ƾ �����ϴ� �޼���
    void StopStateCoroutines()
    {
        if (stateCoroutine != null)
            StopCoroutine(stateCoroutine);
    }

    IEnumerator CheckState()
    {
        while (!isDead)
        {
            yield return new WaitForSeconds(0.2f);

            float dist = Vector3.Distance(player.position, transform.position);

            if (dist <= attackDis && dist >= meleeAttackDis)
            {
                state = State.Attack;
            }
            else if(dist <= meleeAttackDis)
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

    IEnumerator CheckStateForAction()
    {
        while (!isDead)
        {
            switch (state)
            {
                case State.Idle:
                    nav.Stop();
                    anim.SetBool("isChase", false);
                    break;

                case State.Chase:
                    nav.destination = player.position;
                    nav.Resume();
                    anim.SetBool("isChase", true);
                    break;

                case State.Attack:
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
            // ����: ���� ó���� �ߴ����� ����
            rigid.velocity = Vector3.zero;
            rigid.angularVelocity = Vector3.zero;
        }
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
        PlayAnimation("isAttack");
        yield return new WaitForSeconds(WAIT_TIME);
        EnableCollider(attackArea);

        yield return new WaitForSeconds(mobStat.atk_anim - WAIT_TIME);
        DisableCollider(attackArea);
        PlayAnimation("isAttack", false);

        state = State.Chase;
        yield return new WaitForSeconds(mobStat.atk_speed);
    }

    void PlayAnimation(string parameter, bool value = true)
    {
        anim.SetBool(parameter, value);
    }

    void EnableCollider(Collider collider)
    {
        collider.enabled = true;
    }

    void DisableCollider(Collider collider)
    {
        collider.enabled = false;
    }

    // ����: ���� ������Ʈ�� �ı��� �� �ڷ�ƾ ����
    void OnDestroy()
    {
        StopStateCoroutines();
    }

}
