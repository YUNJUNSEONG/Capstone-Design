using Eliot.BehaviourEditor;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.Rendering.DebugUI;

public class Mushroom : Monster
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

    public float chaseDis = 15.0f;
    public float attackDis = 2.0f;
    public bool isAttack;

    // 추가: 코루틴 정지를 위한 변수
    private Coroutine stateCoroutine;

    private void Start()
    {
        // 변경: 코루틴 시작을 메서드로 호출
        StartStateCoroutines();
    }

    // 변경: 코루틴을 시작하는 메서드
    void StartStateCoroutines()
    {
        stateCoroutine = StartCoroutine(CheckState());
        StartCoroutine(CheckStateForAction());
    }

    // 변경: 코루틴 정지하는 메서드
    void StopStateCoroutines()
    {
        if (stateCoroutine != null)
            StopCoroutine(stateCoroutine);
    }

    IEnumerator CheckState()
    {
        while (!isDead)
        {
            yield return new WaitForSeconds(WAIT_TIME);

            float dist = Vector3.Distance(player.position, transform.position);

            if (dist <= attackDis)
            {
                state = State.Attack;
            }
            else if (dist <= chaseDis && dist >=attackDis)
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
                case State.GetHit:
                    nav.Stop();
                    anim.SetBool("isChase", false);
                    anim.SetTrigger("isGetHit");
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


    // 변경: 게임 오브젝트가 파괴될 때 코루틴 정지
    void OnDestroy()
    {
        StopStateCoroutines();
    }
}
