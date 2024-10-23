using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Range : BaseMonster
{
    // 투사체 프리팹
    public GameObject projectilePrefab;

    // 투사체 발사 위치 오프셋
    public Vector3 projectileSpawnOffset = new Vector3(0, 1.0f, 0);

    protected override void Awake()
    {
        base.Awake();
        nav = GetComponent<NavMeshAgent>();
        ResetCoolTime();
    }

    void ResetCoolTime()
    {
        AttackCanUse = 0;
        Skill01CanUse = 0;
    }

    protected override void Attack()
    {
        int skillIndex = random.Next(0, 2); // NumberOfSkills는 2로 설정

        switch (skillIndex)
        {
            case 0:  // 기본 공격
                TryAttack(ref AttackCanUse, AttackRadius, AttackCoolTime, Attack01);
                break;
            case 1: // 스킬 공격
                TryAttack(ref Skill01CanUse, Skill01Radius, SkillCoolTime1, Attack02, true);
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

        // 투사체 발사 위치 계산 (로컬 좌표 기준)
        Vector3 spawnPosition = transform.TransformPoint(projectileSpawnOffset);

        // 플레이어 방향으로 투사체 발사
        Vector3 direction = (player.transform.position - spawnPosition).normalized;
        GameObject projectile = Instantiate(projectilePrefab, spawnPosition, Quaternion.LookRotation(direction));

        // 투사체의 Projectile 컴포넌트 설정
        Projectile projectileComponent = projectile.GetComponent<Projectile>();
        projectileComponent.damage = Skill01; // 몬스터의 공격력을 투사체 데미지로 설정

        // 투사체에 속도 추가 (Rigidbody가 있는 경우)
        Rigidbody projectileRigidbody = projectile.GetComponent<Rigidbody>();
        if (projectileRigidbody != null)
        {
            float projectileSpeed = 20.0f; // 예시로 설정한 투사체 속도
            projectileRigidbody.velocity = direction * projectileSpeed;
        }
    }
}
