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
        nav = GetComponent<NavMeshAgent>();
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
        nav.speed = chargeSpeed;
        nav.isStopped = false; // �̵� ����

        while (elapsedTime < chargeDuration)
        {
            nav.SetDestination(chargeTargetPosition);

            // �÷��̾���� �Ÿ� Ȯ�� �� ���� ó��
            if (Vector3.Distance(transform.position, player.transform.position) < nav.stoppingDistance + 0.5f)
            {
                // �˹� ó�� �� ���� ����
                ApplyKnockback();
                nav.isStopped = true; // �浹 �� �̵� ����
                break;
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        nav.speed = Move_Speed; // ���� �ӵ��� ����
        nav.isStopped = true; // ������ ���� �� �̵� ����
        StartCoroutine(DelayedAction(GetAnimationLength(attack02Hash), OnSecondAttackAnimationEnd)); // ��Ÿ�� ����
    }

    private void ApplyKnockback()
    {
        Vector3 knockbackDir = (player.transform.position - transform.position).normalized;
        player.GetComponent<Rigidbody>().AddForce(knockbackDir * knockbackForce, ForceMode.Impulse);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) // �÷��̾�� �浹 ��
        {
            ApplyKnockback(); // �˹� ó��
            nav.isStopped = true; // �浹 �� �̵� ����
        }
    }


    public void OnChargeAnimationEnd()
    {
        isAttack01 = false;
        isAttack02 = false;
    }
}
