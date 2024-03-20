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
    // ������ �÷��̾� Ÿ����
    void Targeting()
    {
        float targetRadius = 1.5f;
        float targetRange = 3f;

        RaycastHit[] rayHits = Physics.SphereCastAll(transform.position, targetRadius, transform.forward, targetRange, LayerMask.GetMask("Player"));

        if (rayHits.Length > 0 && state != State.Attack && state != State.Skill)
        {
            StartCoroutine(Attack());
            //���Ͱ� �÷��̾ ����ģ ���� ���� ��ų ��Ÿ�� Ȱ��ȭ
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

    /* ��ų ��� �ڷ�ƾ
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

    // ��ų ��Ÿ�� �ڷ�ƾ
    IEnumerator SkillCool()
    {
        state = State.Skill;
        yield return new WaitForSeconds(mobStat.skill_colltime);
        state = State.Chase;
    }
    */
    // �ִϸ��̼� ��� �Լ�
    void PlayAnimation(string parameter, bool value = true)
    {
        anim.SetBool(parameter, value);
    }

    // �ݶ��̴� Ȱ��ȭ �Լ�
    void EnableCollider(Collider collider)
    {
        collider.enabled = true;
    }

    // �ݶ��̴� ��Ȱ��ȭ �Լ�
    void DisableCollider(Collider collider)
    {
        collider.enabled = false;
    }

    #endregion
}
