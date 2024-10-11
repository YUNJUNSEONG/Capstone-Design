using System.Collections;
using UnityEngine;

public class Salamander : Monster
{
    // ����ü ������
    public GameObject projectilePrefab;

    // ����ü �߻� ��ġ ������
    public Vector3 projectileSpawnOffset = new Vector3(0, 1.0f, 0);

    // ȭ�� ���� �� ����ü �߻� ����
    public float projectileSpawnInterval = 0.5f;

    private Coroutine fireAttackCoroutine;

    void Start()
    {
        Skill2CanUse = 0f; // �ʱ� ��ų2�� ��Ÿ���� 0���� ����
    }

    protected override void Attack()
    {
        // ��Ÿ���� 0������ ���� �� �����ϰ� ���
        int skillIndex = Random.Range(0, NumberOfSkills);

        switch (skillIndex)
        {
            case 0:  // �⺻ ����
                if (Skill1CanUse <= 0 && Vector3.Distance(transform.position, player.transform.position) <= attack1Radius)
                {
                    MonsterAttackStart();
                    Skill1();
                    Skill1CanUse = SkillCoolTime1;
                }
                else { anim.SetTrigger(BattleIdleHash); }
                break;
            case 1: // ȭ�� ���
                if (Skill2CanUse <= 0 && Vector3.Distance(transform.position, player.transform.position) <= attack2Radius)
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
    public void FireAttack()
    {
        if (currentState == MonsterState.Die || player == null)
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
            projectileComponent.damage = ATK2; // ������ ���ݷ��� ����ü �������� ����

            // ������ ���� ���� ��� �� ���� ����ü �߻�
            yield return new WaitForSeconds(projectileSpawnInterval);
        }
    }
}
