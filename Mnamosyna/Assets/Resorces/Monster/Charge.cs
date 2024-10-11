using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Charge : BaseMonster
{
    public float knockbackForce = 5f; // 넉백 힘
    public float knockbackDuration = 0.3f; // 넉백 지속 시간

    public float chargeDuration = 1.0f; // 돌진 지속 시간 (초)
    public float chargeSpeed = 10.0f; // 돌진 속도
    public float chargeDistance = 10.0f; // 돌진 거리

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

        int skillIndex = random.Next(0, 2); // NumberOfSkills는 2로 설정

        switch (skillIndex)
        {
            case 0:  // 기본 공격
                TryAttack(ref Attack01CanUse, attack1Radius, SkillCoolTime1, Attack1);
                break;
            case 1: // 스킬 공격
                TryAttack(ref Attack02CanUse, attack2Radius, SkillCoolTime2, Attack2, true);
                break;
        }
    }


    protected override void Attack2()
    {
        anim.SetTrigger(Attack02Hash);
        float delay = GetAnimationLength(attack02Hash); // 공격 애니메이션의 길이

        // Attack2의 애니메이션 종료 후 실행할 메서드를 지연 호출
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
        nav.isStopped = false; // 이동 시작

        while (elapsedTime < chargeDuration)
        {
            nav.SetDestination(chargeTargetPosition);

            // 플레이어와의 거리 확인 및 정지 처리
            if (Vector3.Distance(transform.position, player.transform.position) < nav.stoppingDistance + 0.5f)
            {
                // 넉백 처리 및 돌진 중지
                ApplyKnockback();
                nav.isStopped = true; // 충돌 시 이동 멈춤
                break;
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        nav.speed = Move_Speed; // 원래 속도로 복귀
        nav.isStopped = true; // 돌진이 끝난 후 이동 멈춤
        StartCoroutine(DelayedAction(GetAnimationLength(attack02Hash), OnSecondAttackAnimationEnd)); // 쿨타임 관리
    }

    private void ApplyKnockback()
    {
        Vector3 knockbackDir = (player.transform.position - transform.position).normalized;
        player.GetComponent<Rigidbody>().AddForce(knockbackDir * knockbackForce, ForceMode.Impulse);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) // 플레이어와 충돌 시
        {
            ApplyKnockback(); // 넉백 처리
            nav.isStopped = true; // 충돌 후 이동 멈춤
        }
    }


    public void OnChargeAnimationEnd()
    {
        isAttack01 = false;
        isAttack02 = false;
    }
}
