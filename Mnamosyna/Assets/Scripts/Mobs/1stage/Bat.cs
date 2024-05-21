using UnityEngine;

public class Bat : Monster
{
    // ����ü ������
    public GameObject projectilePrefab;

    // ����ü �߻� ��ġ ������
    public Vector3 projectileSpawnOffset = new Vector3(0, 1.0f, 0);

    protected override void Awake()
    {
        base.Awake();
    }

    void Start()
    {
        Skill2CanUse = 0f; // �ʱ� ��ų2�� ��Ÿ���� 0���� ����
    }

    protected override void Attack()
    {
        // ��Ÿ���� 0������ ���� �� �����ϰ� ���
        int skillIndex = random.Next(0, NumberOfSkills);

        switch (skillIndex)
        {
            case 0:  // �⺻ ����
                if (Skill1CanUse <= 0)
                {
                    MonsterAttackStart();
                    Skill1();
                    Skill1CanUse = SkillCoolTime1;
                }
                else { anim.SetTrigger(BattleIdleHash); }
                break;
            case 1: // ���Ÿ� ����
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

    // �� �޼���� �ִϸ��̼� �̺�Ʈ���� ȣ��˴ϴ�.
    public void FireProjectile()
    {
        if (currentState == MonsterState.Die || player == null)
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
        projectileComponent.damage = Skill_ATK1; // ������ ���ݷ��� ����ü �������� ����
    }
}
