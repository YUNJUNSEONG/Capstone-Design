using UnityEngine;

public class RangeMonster : Monster
{
    // ����ü ������
    public GameObject projectilePrefab;

    // ����ü �߻� ��ġ ������
    public Vector3 projectileSpawnOffset = Vector3.forward;

    // ����ü �߻� �ӵ�
    public float projectileSpeed = 10.0f;

    // ����ü ��Ÿ�
    public float projectileRange = 10.0f;

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

        // ������ ���� ���� ���͸� ����
        Vector3 forwardDirection = transform.forward;

        // ����ü �߻� ��ġ ���
        Vector3 spawnPosition = transform.position + forwardDirection * projectileSpawnOffset.magnitude;

        // �÷��̾� �������� ����ü �߻�
        Vector3 direction = (player.transform.position - spawnPosition).normalized;
        GameObject projectile = Instantiate(projectilePrefab, spawnPosition, Quaternion.identity);
        Rigidbody projectileRigidbody = projectile.GetComponent<Rigidbody>();
        projectileRigidbody.velocity = direction * projectileSpeed;

        // ���� �ð� �� ����ü ����
        Destroy(projectile, projectileRange / projectileSpeed);
    }

    // ����ü �浹 ó��
    private void OnCollisionEnter(Collision collision)
    {
        // �÷��̾�� �浹���� ��
        if (collision.gameObject.CompareTag("Player"))
        {
            // �÷��̾�� ������ ������
            collision.gameObject.GetComponent<Player>().TakeDamage(Damage(1));

            // �浹�� ����ü ����
            Destroy(gameObject);
        }
    }


}
