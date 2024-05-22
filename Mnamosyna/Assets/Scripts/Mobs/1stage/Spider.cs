using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Spider : Monster
{
    // Charge 애니메이션의 길이 (초 단위)
    public float chargeDuration = 1.0f; // 돌진 지속 시간 (초)
    public float chargeSpeed = 10.0f; // 돌진 속도
    public float chargeDistance = 10.0f; // 돌진 거리

    void Start()
    {
        Skill2CanUse = 0f;
    }

    protected override void Attack()
    {
        // 쿨타임이 0이하인 공격 중 랜덤하게 출력
        int skillIndex = random.Next(0, NumberOfSkills);

        switch (skillIndex)
        {
            case 0:  // 기본 공격
                if (Skill1CanUse <= 0 && Vector3.Distance(transform.position, player.transform.position) <= attack1Radius)
                {
                    MonsterAttackStart();
                    Skill1();
                    Skill1CanUse = SkillCoolTime1;
                }
                else { anim.SetTrigger(BattleIdleHash); }
                break;
            case 1: // 돌진 공격
                if (Skill2CanUse <= 0 && Vector3.Distance(transform.position, player.transform.position) <= attack2Radius)
                {
                    MonsterAttackStart();
                    isSkill = true;
                    Skill2();
                    Skill2CanUse = SkillCoolTime2;
                }
                else { anim.SetTrigger(BattleIdleHash); }
                break;
        }
    }
    protected override void Skill2()
    {
        if (currentState == MonsterState.Die)
        {
            return;
        }
        anim.SetTrigger(Attack02Hash);
        Invoke("OnSecondAttackAnimationEnd", secondAttackAnimationLength);
        StartCoroutine(PerformCharge());
    }

    private IEnumerator PerformCharge()
    {
        Vector3 startPosition = transform.position;
        Vector3 targetPosition = player.transform.position;

        Vector3 chargeDirection = (targetPosition - startPosition).normalized;
        Vector3 chargeTargetPosition = startPosition + chargeDirection * chargeDistance;

        float elapsedTime = 0f;
        while (elapsedTime < chargeDuration)
        {
            nav.speed = chargeSpeed;
            nav.SetDestination(chargeTargetPosition);

            // 목표 위치에 도달했는지 확인
            if (Vector3.Distance(transform.position, chargeTargetPosition) < nav.stoppingDistance)
            {
                break;
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        nav.speed = Move_Speed; // 원래 속도로 복귀
        Invoke("OnChargeAnimationEnd", secondAttackAnimationLength);
    }

    public void OnChargeAnimationEnd()
    {
        isAttack = false;
        isSkill = false;
    }
}
