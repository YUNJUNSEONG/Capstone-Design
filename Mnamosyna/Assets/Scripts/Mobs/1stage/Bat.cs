using UnityEngine;

public class RangeMonster : Monster
{
    // 투사체 프리팹
    public GameObject projectilePrefab;

    // 투사체 발사 위치 오프셋
    public Vector3 projectileSpawnOffset = Vector3.forward;

    // 투사체 발사 속도
    public float projectileSpeed = 10.0f;

    // 투사체 사거리
    public float projectileRange = 10.0f;

    protected override void Awake()
    {
        base.Awake();
    }

    void Start()
    {
        Skill2CanUse = 0f; // 초기 스킬2의 쿨타임을 0으로 설정
    }

    protected override void Attack()
    {
        // 쿨타임이 0이하인 공격 중 랜덤하게 출력
        int skillIndex = random.Next(0, NumberOfSkills);

        switch (skillIndex)
        {
            case 0:  // 기본 공격
                if (Skill1CanUse <= 0)
                {
                    MonsterAttackStart();
                    Skill1();
                    Skill1CanUse = SkillCoolTime1;
                }
                else { anim.SetTrigger(BattleIdleHash); }
                break;
            case 1: // 원거리 공격
                if (Skill2CanUse <= 0)
                {
                    MonsterAttackStart();
                    isSkill = true;
                    FireProjectile();
                    Skill2CanUse = SkillCoolTime2;
                }
                else { anim.SetTrigger(BattleIdleHash); }
                break;
        }
    }

    void FireProjectile()
    {
        if (currentState == MonsterState.Die || player == null)
        {
            return;
        }

        // 몬스터의 전방 방향 벡터를 얻어옴
        Vector3 forwardDirection = transform.forward;

        // 투사체 발사 위치 계산
        Vector3 spawnPosition = transform.position + forwardDirection * projectileSpawnOffset.magnitude;

        // 플레이어 방향으로 투사체 발사
        Vector3 direction = (player.transform.position - spawnPosition).normalized;
        GameObject projectile = Instantiate(projectilePrefab, spawnPosition, Quaternion.identity);
        Rigidbody projectileRigidbody = projectile.GetComponent<Rigidbody>();
        projectileRigidbody.velocity = direction * projectileSpeed;

        // 일정 시간 후 투사체 삭제
        Destroy(projectile, projectileRange / projectileSpeed);
    }

    // 투사체 충돌 처리
    private void OnCollisionEnter(Collision collision)
    {
        // 플레이어와 충돌했을 때
        if (collision.gameObject.CompareTag("Player"))
        {
            // 플레이어에게 데미지 입히기
            collision.gameObject.GetComponent<Player>().TakeDamage(Damage(1));

            // 충돌한 투사체 삭제
            Destroy(gameObject);
        }
    }


}
