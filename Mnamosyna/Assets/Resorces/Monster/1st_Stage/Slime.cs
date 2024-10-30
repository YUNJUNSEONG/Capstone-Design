using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : BaseMonster
{

    // ����� ó���ϴ� �Լ�
    protected override void Die()
    {
        if (isDead) return;

        isDead = true;
        nav.isStopped = true;
        gameObject.layer = 11;
        ChangeState(State.Die);
        anim.SetTrigger(DieHash);
        anim.SetBool(RunHash, false);

        nav.isStopped = true;
        Invoke("DestroyObject", 2.0f);
        Collider collider = GetComponent<Collider>();
        if (collider != null) { collider.enabled = false; }

        // �����ʿ��� �Ʊ� ������ ��ȯ
        if (spawner != null)
        {
            spawner.aliveCount--; // ���� �������� �׾����Ƿ� aliveCount ����
            spawner.CheckAliveCount();

            StartCoroutine(WaitTwoSeconds());
            // �Ʊ� ������ ��ȯ
            Vector3 spawnPosition = transform.position; // ���� �������� ��ġ�� �������� ��ȯ
            spawner.SpawnBabySlime(spawnPosition); // �������� SpawnBabySlime �޼��� ȣ��
            spawner.SpawnBabySlime(spawnPosition);

            Debug.Log("���� ����:" + spawner.aliveCount);
        }
        else
        {
            //Debug.LogError("���� ��ũ��Ʈ���� ���� ������ �� ã�ƿ�");
        }
    }
    IEnumerator WaitTwoSeconds()
    {
        // 2�� ���
        yield return new WaitForSeconds(2.0f);
        // 2�� �� ������ ����
        Debug.Log("2�� ��� �� ����");
    }
}
