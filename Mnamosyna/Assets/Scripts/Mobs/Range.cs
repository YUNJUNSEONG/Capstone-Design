using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Range : BaseMonster
{
    // ����ü ������
    public GameObject projectilePrefab;

    // ����ü �߻� ��ġ ������
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

        int skillIndex = random.Next(0, 2); // NumberOfSkills�� 2�� ����

        switch (skillIndex)
        {
            case 0:  // �⺻ ����
                TryAttack(ref Attack01CanUse, attack1Radius, SkillCoolTime1, Attack1);
                break;
            case 1: // ��ų ����
                TryAttack(ref Attack02CanUse, attack2Radius, SkillCoolTime2, Attack2, true);
                break;
        }
    }

    // �� �޼���� �ִϸ��̼� �̺�Ʈ���� ȣ��˴ϴ�.
    public void FireProjectile()
    {
        if (currentState == State.Die || player == null)
        {
            return;
        }

        // ������ ���� ���� ���͸� ����
        Vector3 forwardDirection = transform.forward;

        // ����ü �߻� ��ġ ���
        Vector3 spawnPosition = transform.position + forwardDirection * projectileSpawnOffset.z + projectileSpawnOffset;

        // �÷��̾� �������� ����ü �߻�
        Vector3 direction = (player.transform.position - spawnPosition).normalized;
        GameObject projectile = Instantiate(projectilePrefab, spawnPosition, Quaternion.LookRotation(direction));

        // ����ü�� Projectile ������Ʈ ����
        Projectile projectileComponent = projectile.GetComponent<Projectile>();
        projectileComponent.damage = ATK2; // ������ ���ݷ��� ����ü �������� ����
    }
}
