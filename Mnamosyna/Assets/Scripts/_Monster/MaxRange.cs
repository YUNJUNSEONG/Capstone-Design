using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaxRange : MonoBehaviour
{
    public MeleeMonster monster; 
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            //Debug.Log("플레이어가 범위 밖으로 나감");
            monster.ChangeState(MeleeMonster.MonsterState.Patrol);
        }
    }
}
