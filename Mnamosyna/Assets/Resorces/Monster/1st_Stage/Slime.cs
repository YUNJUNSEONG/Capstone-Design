using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : BaseMonster
{

    // 사망을 처리하는 함수
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

        // 스포너에서 아기 슬라임 소환
        if (spawner != null)
        {
            spawner.aliveCount--; // 현재 슬라임이 죽었으므로 aliveCount 감소
            spawner.CheckAliveCount();

            StartCoroutine(WaitSeconds());

            Debug.Log("남은 몬스터:" + spawner.aliveCount);
        }
        else
        {
            //Debug.LogError("몬스터 스크립트에서 몬스터 스포너 못 찾아옴");
        }
    }
    IEnumerator WaitSeconds()
    {

        yield return new WaitForSeconds(3.0f);

        Debug.Log("3초 대기 후 실행");
        // 아기 슬라임 소환
        Vector3 spawnPosition = transform.position; // 현재 슬라임의 위치를 기준으로 소환
        spawner.SpawnBabySlime(spawnPosition); // 스포너의 SpawnBabySlime 메서드 호출
        spawner.SpawnBabySlime(spawnPosition);
    }
}
