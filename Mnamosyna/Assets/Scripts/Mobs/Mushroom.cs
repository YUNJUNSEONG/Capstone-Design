using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mushroom : Monster
{
    public enum State
    {
        Idle,
        Chase,
        Attack,
        Skill,
        Die
    }
    private State state = State.Idle;

    public float chaseDis = 150.0f;
    public float attackDis = 2.0f;
    private bool isDead = false;
    private void Start()
    {
        StartCoroutine(this.CheckState());
        StartCoroutine(this.CheckStateForAction());
    }

    IEnumerator CheckState()
    {
        while (!isDead)
        {
            yield return new WaitForSeconds(0.2f);

            float dist = Vector3.Distance(player.position, transform.position);

            if (dist <= attackDis)
            {
                state = State.Attack;
            }
            else if(dist <= chaseDis)
            {
                state = State.Chase;
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
            switch(state)
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
        if (State.Chase = true)
        {
            rigid.velocity = Vector3.zero;
            rigid.angularVelocity = Vector3.zero;
        }
    }

    #region HitPlayer
    // 몬스터의 플레이어 타게팅
    void Targeting()
    {
        float targetRadius = 1.5f;
        float targetRange = 3f;

        RaycastHit[] rayHits = Physics.SphereCastAll(transform.position, targetRadius, transform.forward, targetRange, LayerMask.GetMask("Player"));

        if (rayHits.Length > 0 && state != State.Attack && state != State.Skill)
        {
            StartCoroutine(Attack());
            //몬스터가 플레이어를 마주친 이후 부터 스킬 쿨타임 활성화
        }
    }

    IEnumerator Attack()
    {
        if (state == State.Skill)
            yield break;

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

    /* 스킬 사용 코루틴
    IEnumerator Skill()
    {
        if (state == State.Skill)
            yield break;

        state = State.Skill;
        PlayAnimation("isSkill");
        yield return new WaitForSeconds(WAIT_TIME);
        EnableCollider(skillArea);

        yield return new WaitForSeconds(mobStat.skill_anim - WAIT_TIME);
        DisableCollider(skillArea);
        PlayAnimation("isSkill", false);

        yield return new WaitForSeconds(mobStat.atk_speed);

        state = State.Chase;
    }

    // 스킬 쿨타임 코루틴
    IEnumerator SkillCool()
    {
        state = State.Skill;
        yield return new WaitForSeconds(mobStat.skill_colltime);
        state = State.Chase;
    }
    */
    // 애니메이션 재생 함수
    void PlayAnimation(string parameter, bool value = true)
    {
        anim.SetBool(parameter, value);
    }

    // 콜라이더 활성화 함수
    void EnableCollider(Collider collider)
    {
        collider.enabled = true;
    }

    // 콜라이더 비활성화 함수
    void DisableCollider(Collider collider)
    {
        collider.enabled = false;
    }

    #endregion
}
