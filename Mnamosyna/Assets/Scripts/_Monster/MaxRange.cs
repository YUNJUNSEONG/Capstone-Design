using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

//원본
/*public class MaxRange : MonoBehaviour
{
    public Monster monster;
    public CapsuleCollider Distance;

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            //Debug.Log("플레이어가 범위 밖으로 나감");
            monster.ChangeState(Monster.MonsterState.Patrol);
        }
    }
    void Awake()
    {
        Distance = GetComponent<CapsuleCollider>();
        SetMaxDistance();
    }

    void SetMaxDistance()
    {
        Distance.radius = monster.patrol_radius;
    }
}*/