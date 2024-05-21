using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : Monster
{

    public GameObject babySlime;

    // 사망을 처리하는 함수
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
            Debug.Log("남은 몬스터:"+spawner.aliveCount);
        }
        else
        {
            //Debug.LogError("몬스터 스크립트에서 몬스터 스포너 못 찾아옴");
        }

    }

    void Spawn()
    {
        
        Vector3 spawnPosition = transform.position; // 몬스터의 현재 위치를 기준으로 소환합니다.
        Instantiate(babySlime, spawnPosition, Quaternion.identity);
        Instantiate(babySlime, spawnPosition, Quaternion.identity); // 두 마리를 소환합니다.
        spawner.aliveCount += 2;
    }

}
