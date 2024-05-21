using UnityEngine;

public class Bat : Monster
{
    // 투사체 프리팹
    public GameObject projectilePrefab;

    // 투사체 발사 위치 오프셋
    public Vector3 projectileSpawnOffset = new Vector3(0, 1.0f, 0);

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
                    Skill2();
                    Skill2CanUse = SkillCoolTime2;
                }
                else { anim.SetTrigger(BattleIdleHash); }
                break;
        }
    }

    // 이 메서드는 애니메이션 이벤트에서 호출됩니다.
    public void FireProjectile()
    {
        if (currentState == MonsterState.Die || player == null)
        {
            return;
        }

        // 몬스터의 전방 방향 벡터를 얻어옴
        Vector3 forwardDirection = transform.forward;

        // 투사체 발사 위치 계산
        Vector3 spawnPosition = transform.position + forwardDirection * projectileSpawnOffset.z + projectileSpawnOffset;

        // 플레이어 방향으로 투사체 발사
        Vector3 direction = (player.transform.position - spawnPosition).normalized;
        GameObject projectile = Instantiate(projectilePrefab, spawnPosition, Quaternion.LookRotation(direction));

        // 투사체의 Projectile 컴포넌트 설정
        Projectile projectileComponent = projectile.GetComponent<Projectile>();
        projectileComponent.damage = Skill_ATK1; // 몬스터의 공격력을 투사체 데미지로 설정
    }
}
