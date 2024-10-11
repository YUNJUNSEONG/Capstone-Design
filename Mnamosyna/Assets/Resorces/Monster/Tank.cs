using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank : BaseMonster
{
    public float knockbackForce = 10f; // 넉백 힘
    public float knockbackDuration = 0.5f; // 넉백 지속 시간

    private GameObject playerObject; // 플레이어 객체 캐시

    protected override void Awake()
    {
        base.Awake();
        playerObject = GameObject.FindGameObjectWithTag("Player"); // 플레이어 객체 캐시
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
                StartCoroutine(PerformAttackAfterDelay()); // 공격 애니메이션 후 넉백 적용
                break;
        }
    }

    private IEnumerator PerformAttackAfterDelay()
    {
        float attackDuration = GetAnimationLength(attack02Hash); // 공격 애니메이션 길이 가져오기
        yield return new WaitForSeconds(attackDuration); // 애니메이션 끝날 때까지 기다림
        PerformAttack(); // 넉백 적용
    }

    public void PerformAttack()
    {
        // 플레이어를 넉백시키기 위해 플레이어 객체를 사용
        if (playerObject != null)
        {
            Rigidbody playerRigidbody = playerObject.GetComponent<Rigidbody>();
            if (playerRigidbody != null)
            {
                // 몬스터에서 플레이어 방향으로의 벡터 계산
                Vector3 directionToPlayer = (playerObject.transform.position - transform.position).normalized;

                // 넉백 힘을 방향에 적용하여 플레이어를 밀어냄
                playerRigidbody.AddForce(directionToPlayer * knockbackForce, ForceMode.Impulse);

                // 일정 시간 후에 넉백을 해제하여 플레이어가 다시 움직일 수 있도록 함
                StartCoroutine(DisableKnockback(playerRigidbody));
            }
        }
    }

    // 넉백 해제 코루틴
    private IEnumerator DisableKnockback(Rigidbody playerRigidbody)
    {
        yield return new WaitForSeconds(knockbackDuration);

        // 넉백 후에 물리적 속도를 자연스럽게 멈추도록 함
        // playerRigidbody.velocity = Vector3.zero; // 강제 멈춤 대신 자연스럽게 처리할 수 있음
    }
}
