using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Charge : BaseMonster
{
    public float knockbackForce = 5f; // �˹� ��
    public float knockbackDuration = 0.3f; // �˹� ���� �ð�

    public float chargeDuration = 1.0f; // ���� ���� �ð� (��)
    public float chargeSpeed = 10.0f; // ���� �ӵ�
    public float chargeDistance = 10.0f; // ���� �Ÿ�

    protected override void Awake()
    {
        base.Awake();
        ResetCoolTime();
    }

    void ResetCoolTime()
    {
        Attack01CanUse = 0;
        Attack02CanUse = 0;
    }

    protected override void Attack()
    {

        int skillIndex = random.Next(0, 2); // NumberOfSkills�� 2�� ����

        switch (skillIndex)
        {
            case 0:  // �⺻ ����
                TryAttack(ref Attack01CanUse, attack1Radius, SkillCoolTime1, Attack1);
                break;
            case 1: // ��ų ����
                TryAttack(ref Attack02CanUse, attack2Radius, SkillCoolTime2, Attack2, true);
                break;
        }
    }

    protected override void Attack2()
    {
        anim.SetTrigger(Attack02Hash);
        float delay = GetAnimationLength(attack02Hash); // ���� �ִϸ��̼��� ����

        // Attack2�� �ִϸ��̼� ���� �� ������ �޼��带 ���� ȣ��
        StartCoroutine(DelayedAction(delay, OnSecondAttackAnimationEnd));
        StartCoroutine(PerformCharge());
    }

    private IEnumerator PerformCharge()
    {
        Vector3 startPosition = transform.position;
        Vector3 targetPosition = player.transform.position;

        Vector3 chargeDirection = (targetPosition - startPosition).normalized;
        Vector3 chargeTargetPosition = startPosition + chargeDirection * chargeDistance;

        float elapsedTime = 0f;
        float delay = GetAnimationLength(attack02Hash);
        while (elapsedTime < chargeDuration)
        {
            nav.speed = chargeSpeed;
            nav.SetDestination(chargeTargetPosition);

            // ��ǥ ��ġ�� �����ߴ��� Ȯ��
            if (Vector3.Distance(transform.position, chargeTargetPosition) < nav.stoppingDistance)
            {
                break;
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        nav.speed = Move_Speed; // ���� �ӵ��� ����
        StartCoroutine(DelayedAction(delay, OnSecondAttackAnimationEnd));
    }

    public void OnChargeAnimationEnd()
    {
        isAttack01 = false;
        isAttack02 = false;
    }
}
