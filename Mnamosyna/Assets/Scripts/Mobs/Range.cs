using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Range : BaseMonster
{
    // 투사체 프리팹
    public GameObject projectilePrefab;

    // 투사체 발사 위치 오프셋
    public Vector3 projectileSpawnOffset = new Vector3(0, 1.0f, 0);

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

    // 이 메서드는 애니메이션 이벤트에서 호출됩니다.
    public void FireProjectile()
    {
        if (currentState == State.Die || player == null)
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
        projectileComponent.damage = ATK2; // 몬스터의 공격력을 투사체 데미지로 설정
    }
}
