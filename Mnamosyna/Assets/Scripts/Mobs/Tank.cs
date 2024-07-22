using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank : BaseMonster
{
    public float knockbackForce = 10f; // �˹� ��
    public float knockbackDuration = 0.5f; // �˹� ���� �ð�

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
                PerformAttack();
                break;
        }
    }

    public void PerformAttack()
    {
        // �÷��̾ �˹��Ű�� ���� �÷��̾� ��ü�� ã�ƿ�
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
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

        // �˹� ���� �ʱ�ȭ�Ͽ� �÷��̾ �ٽ� ������ �� �ֵ��� ��
        playerRigidbody.velocity = Vector3.zero;
    }
}
