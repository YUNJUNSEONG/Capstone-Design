using System.Collections;
using UnityEngine;

public class Salamander : BaseMonster
{
    // ����ü ������
    public GameObject projectilePrefab;

    // ����ü �߻� ��ġ ������
    public Vector3 projectileSpawnOffset = new Vector3(0, 1.0f, 0);

    // ȭ�� ���� �� ����ü �߻� ����
    public float projectileSpawnInterval = 0.5f;

    private Coroutine fireAttackCoroutine;



    // �� �޼���� �ִϸ��̼� �̺�Ʈ���� ȣ��˴ϴ�.
    public void FireAttack()
    {
        if (currentState == State.Die || player == null)
        {
            return;
        }

        // ȭ�� ���� �ڷ�ƾ ����
        if (fireAttackCoroutine == null)
        {
            fireAttackCoroutine = StartCoroutine(FireAttackCoroutine());
        }
    }

    // �� �޼���� �ִϸ��̼� �̺�Ʈ���� ȣ��˴ϴ�.
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
            // ������ ���� ���� ���͸� ����
            Vector3 forwardDirection = transform.forward;

            // ����ü �߻� ��ġ ���
            Vector3 spawnPosition = transform.position + forwardDirection * projectileSpawnOffset.z + projectileSpawnOffset;

            // �÷��̾� �������� ����ü �߻�
            Vector3 direction = (player.transform.position - spawnPosition).normalized;
            GameObject projectile = Instantiate(projectilePrefab, spawnPosition, Quaternion.LookRotation(direction));

            // ����ü�� Projectile ������Ʈ ����
            Projectile projectileComponent = projectile.GetComponent<Projectile>();
            projectileComponent.damage = Skill01; // ������ ���ݷ��� ����ü �������� ����

            // ������ ���� ���� ��� �� ���� ����ü �߻�
            yield return new WaitForSeconds(projectileSpawnInterval);
        }
    }
}
