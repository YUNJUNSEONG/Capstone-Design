using System.Collections;
using UnityEngine;

public class Bat : Monster
{
    // ���Ÿ� ���� ���� ����
    public GameObject projectilePrefab; // �߻�ü ������
    public float projectileSpeed = 10f; // �߻�ü �ӵ�
    public float attackRange = 10f; // ���� ��Ÿ�

    // ��ӹ��� Update �Լ� �������̵�
    protected override void Update()
    {
        base.Update(); // �θ� Ŭ������ Update �Լ� ȣ��

        // ���Ÿ� ���� ����
        RangedAttack();
    }

    // ���Ÿ� ���� ���� �Լ�
    void RangedAttack()
    {
        // �÷��̾ ���Ÿ� ��Ÿ� ���� ������ ���� ����
        if (Vector3.Distance(transform.position, player.transform.position) <= attackRange)
        {
            // �÷��̾� ���� �ٶ󺸵��� ȸ��
            RotateMonsterToCharacter();

            // �߻�ü ���� ��ġ ���
            Vector3 spawnPosition = transform.position + transform.forward * 1.5f; // �ٷ� ������ ��ġ ����

            // �߻�ü ���� �� �߻�
            GameObject projectile = Instantiate(projectilePrefab, spawnPosition, Quaternion.identity);
            Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();
            if (projectileRb != null)
            {
                Vector3 directionToPlayer = (player.transform.position - spawnPosition).normalized;
                projectileRb.velocity = directionToPlayer * projectileSpeed;
            }
        }
    }

    // ������ ���ݿ� ���� ������ ��ȯ �Լ� �������̵�
    public override int Damage(int skillIndex)
    {
        int damage = 0;

        switch (skillIndex)
        {
            case 0: // �ٰŸ� ����
                damage = ATK;
                break;
            case 1: // ���Ÿ� ����
                damage = Skill_ATK1;
                break;
            default:
                // �������� ���� ��ų�̸� damage 0
                break;
        }
        return damage;
    }
}
