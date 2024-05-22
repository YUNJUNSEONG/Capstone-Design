using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Spider : Monster
{
    // Charge �ִϸ��̼��� ���� (�� ����)
    public float chargeDuration = 1.0f; // ���� ���� �ð� (��)
    public float chargeSpeed = 10.0f; // ���� �ӵ�
    public float chargeDistance = 10.0f; // ���� �Ÿ�

    void Start()
    {
        Skill2CanUse = 0f;
    }

    protected override void Attack()
    {
        // ��Ÿ���� 0������ ���� �� �����ϰ� ���
        int skillIndex = random.Next(0, NumberOfSkills);

        switch (skillIndex)
        {
            case 0:  // �⺻ ����
                if (Skill1CanUse <= 0 && Vector3.Distance(transform.position, player.transform.position) <= attack1Radius)
                {
                    MonsterAttackStart();
                    Skill1();
                    Skill1CanUse = SkillCoolTime1;
                }
                else { anim.SetTrigger(BattleIdleHash); }
                break;
            case 1: // ���� ����
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

            // ��ǥ ��ġ�� �����ߴ��� Ȯ��
            if (Vector3.Distance(transform.position, chargeTargetPosition) < nav.stoppingDistance)
            {
                break;
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        nav.speed = Move_Speed; // ���� �ӵ��� ����
        Invoke("OnChargeAnimationEnd", secondAttackAnimationLength);
    }

    public void OnChargeAnimationEnd()
    {
        isAttack = false;
        isSkill = false;
    }
}
