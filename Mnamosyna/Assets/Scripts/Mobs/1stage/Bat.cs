using System.Collections;
using UnityEngine;

public class Bat : Monster
{
    // 원거리 공격 관련 변수
    public GameObject projectilePrefab; // 발사체 프리팹
    public float projectileSpeed = 10f; // 발사체 속도
    public float attackRange = 10f; // 공격 사거리

    // 상속받은 Update 함수 오버라이드
    protected override void Update()
    {
        base.Update(); // 부모 클래스의 Update 함수 호출

        // 원거리 공격 실행
        RangedAttack();
    }

    // 원거리 공격 실행 함수
    void RangedAttack()
    {
        // 플레이어가 원거리 사거리 내에 있으면 공격 실행
        if (Vector3.Distance(transform.position, player.transform.position) <= attackRange)
        {
            // 플레이어 쪽을 바라보도록 회전
            RotateMonsterToCharacter();

            // 발사체 생성 위치 계산
            Vector3 spawnPosition = transform.position + transform.forward * 1.5f; // 바로 앞으로 위치 지정

            // 발사체 생성 및 발사
            GameObject projectile = Instantiate(projectilePrefab, spawnPosition, Quaternion.identity);
            Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();
            if (projectileRb != null)
            {
                Vector3 directionToPlayer = (player.transform.position - spawnPosition).normalized;
                projectileRb.velocity = directionToPlayer * projectileSpeed;
            }
        }
    }

    // 몬스터의 공격에 따른 데미지 반환 함수 오버라이드
    public override int Damage(int skillIndex)
    {
        int damage = 0;

        switch (skillIndex)
        {
            case 0: // 근거리 공격
                damage = ATK;
                break;
            case 1: // 원거리 공격
                damage = Skill_ATK1;
                break;
            default:
                // 지정되지 않은 스킬이면 damage 0
                break;
        }
        return damage;
    }
}
