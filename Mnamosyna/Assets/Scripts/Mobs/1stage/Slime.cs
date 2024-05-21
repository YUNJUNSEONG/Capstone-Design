using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : Monster
{

    public GameObject babySlime;

    // ����� ó���ϴ� �Լ�
    protected override void Die()
    {
        if (isDead) return;

        isDead = true;
        ChangeState(MonsterState.Die);
        anim.SetTrigger(DieHash);
        nav.isStopped = true;
        anim.SetBool(RunHash, false);
        //Knockback();
        Invoke("Spawn", 1.0f);
        Invoke("DestroyObject", 2.0f);
        Collider collider = GetComponent<Collider>();
        if (collider != null) { collider.enabled = false; }

        
        if (spawner != null)
        {
            spawner.aliveCount--;
            spawner.CheckAliveCount();
            Debug.Log("���� ����:"+spawner.aliveCount);
        }
        else
        {
            //Debug.LogError("���� ��ũ��Ʈ���� ���� ������ �� ã�ƿ�");
        }

    }

    void Spawn()
    {
        
        Vector3 spawnPosition = transform.position; // ������ ���� ��ġ�� �������� ��ȯ�մϴ�.
        Instantiate(babySlime, spawnPosition, Quaternion.identity);
        Instantiate(babySlime, spawnPosition, Quaternion.identity); // �� ������ ��ȯ�մϴ�.
        spawner.aliveCount += 2;
    }

}
