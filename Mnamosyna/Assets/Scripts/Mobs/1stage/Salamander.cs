using System.Collections;
using UnityEngine;

public class Salamander : Monster
{
    // 투사체 프리팹
    public GameObject projectilePrefab;

    // 투사체 발사 위치 오프셋
    public Vector3 projectileSpawnOffset = new Vector3(0, 1.0f, 0);

    // 화염 공격 중 투사체 발사 간격
    public float projectileSpawnInterval = 0.5f;

    private Coroutine fireAttackCoroutine;

    void Start()
    {
        Skill2CanUse = 0f; // 초기 스킬2의 쿨타임을 0으로 설정
    }

    protected override void Attack()
    {
        // 쿨타임이 0이하인 공격 중 랜덤하게 출력
        int skillIndex = Random.Range(0, NumberOfSkills);

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
            case 1: // 화염 방사
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

    // 이 메서드는 애니메이션 이벤트에서 호출됩니다.
    public void FireAttack()
    {
        if (currentState == MonsterState.Die || player == null)
        {
            return;
        }

        // 화염 공격 코루틴 시작
        if (fireAttackCoroutine == null)
        {
            fireAttackCoroutine = StartCoroutine(FireAttackCoroutine());
        }
    }

    // 이 메서드는 애니메이션 이벤트에서 호출됩니다.
    public void EndFireAttack()
    {
        if (fireAttackCoroutine != null)
        {
            StopCoroutine(fireAttackCoroutine);
            fireAttackCoroutine = null;
        }
    }

    private IEnumerator FireAttackCoroutine()
    {
        while (true)
        {
            // 몬스터의 전방 방향 벡터를 얻어옴
            Vector3 forwardDirection = transform.forward;

            // 투사체 발사 위치 계산
            Vector3 spawnPosition = transform.position + forwardDirection * projectileSpawnOffset.z + projectileSpawnOffset;

            // 플레이어 방향으로 투사체 발사
            Vector3 direction = (player.transform.position - spawnPosition).normalized;
            GameObject projectile = Instantiate(projectilePrefab, spawnPosition, Quaternion.LookRotation(direction));

            // 투사체의 Projectile 컴포넌트 설정
            Projectile projectileComponent = projectile.GetComponent<Projectile>();
            projectileComponent.damage = ATK2; // 몬스터의 공격력을 투사체 데미지로 설정

            // 지정된 간격 동안 대기 후 다음 투사체 발사
            yield return new WaitForSeconds(projectileSpawnInterval);
        }
    }
}
