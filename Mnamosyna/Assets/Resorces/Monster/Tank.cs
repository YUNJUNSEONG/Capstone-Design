using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank : BaseMonster
{
    public float knockbackForce = 10f; // �˹� ��
    public float knockbackDuration = 0.5f; // �˹� ���� �ð�

    private GameObject playerObject; // �÷��̾� ��ü ĳ��

    protected override void Awake()
    {
        base.Awake();
        playerObject = GameObject.FindGameObjectWithTag("Player"); // �÷��̾� ��ü ĳ��
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
                StartCoroutine(PerformAttackAfterDelay()); // ���� �ִϸ��̼� �� �˹� ����
                break;
        }
    }

    private IEnumerator PerformAttackAfterDelay()
    {
        float attackDuration = GetAnimationLength(attack02Hash); // ���� �ִϸ��̼� ���� ��������
        yield return new WaitForSeconds(attackDuration); // �ִϸ��̼� ���� ������ ��ٸ�
        PerformAttack(); // �˹� ����
    }

    public void PerformAttack()
    {
        // �÷��̾ �˹��Ű�� ���� �÷��̾� ��ü�� ���
        if (playerObject != null)
        {
            Rigidbody playerRigidbody = playerObject.GetComponent<Rigidbody>();
            if (playerRigidbody != null)
            {
                // ���Ϳ��� �÷��̾� ���������� ���� ���
                Vector3 directionToPlayer = (playerObject.transform.position - transform.position).normalized;

                // �˹� ���� ���⿡ �����Ͽ� �÷��̾ �о
                playerRigidbody.AddForce(directionToPlayer * knockbackForce, ForceMode.Impulse);

                // ���� �ð� �Ŀ� �˹��� �����Ͽ� �÷��̾ �ٽ� ������ �� �ֵ��� ��
                StartCoroutine(DisableKnockback(playerRigidbody));
            }
        }
    }

    // �˹� ���� �ڷ�ƾ
    private IEnumerator DisableKnockback(Rigidbody playerRigidbody)
    {
        yield return new WaitForSeconds(knockbackDuration);

        // �˹� �Ŀ� ������ �ӵ��� �ڿ������� ���ߵ��� ��
        // playerRigidbody.velocity = Vector3.zero; // ���� ���� ��� �ڿ������� ó���� �� ����
    }
}
