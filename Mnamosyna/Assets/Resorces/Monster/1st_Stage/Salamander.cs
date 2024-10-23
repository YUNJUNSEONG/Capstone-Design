using System.Collections;
using UnityEngine;

public class Salamander : BaseMonster
{
    // 투사체 프리팹
    public GameObject projectilePrefab;

    // 투사체 발사 위치 오프셋
    public Vector3 projectileSpawnOffset = new Vector3(0, 1.0f, 0);

    // 화염 공격 중 투사체 발사 간격
    public float projectileSpawnInterval = 0.5f;

    private Coroutine fireAttackCoroutine;



    // 이 메서드는 애니메이션 이벤트에서 호출됩니다.
    public void FireAttack()
    {
        if (currentState == State.Die || player == null)
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
            projectileComponent.damage = Skill01; // 몬스터의 공격력을 투사체 데미지로 설정

            // 지정된 간격 동안 대기 후 다음 투사체 발사
            yield return new WaitForSeconds(projectileSpawnInterval);
        }
    }
}
