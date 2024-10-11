using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
public class DetectionRange : MonoBehaviour
{
    public MeleeMonster monster; 
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            //Debug.Log("플레이어가 범위 안에 들어옴");
            monster.ChangeState(MeleeMonster.MonsterState. );
        }
    }
}*/


public class DetectionRange : MonoBehaviour
{
    public BaseMonster monster;
    public Boss boss;
    public CapsuleCollider Distance;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (monster != null)
            {
                // If monster exists, change its state to chase
                monster.ChangeState(BaseMonster.State.Chase);
            }
            else if (boss != null)
            {
                // If boss exists, change its state to chase
                boss.ChangeState(Boss.State.Chase);
            }
        }
    }

    void Awake()
    {
        Distance = GetComponent<CapsuleCollider>();
        SetDetectionDistance();
    }

    void SetDetectionDistance()
    {
        if (monster != null)
        {
            // Set detection distance based on monster's detection range
            Distance.radius = monster.detection;
        }
        else if (boss != null)
        {
            // Set detection distance based on boss's detection range
            Distance.radius = boss.detection;
        }
    }
}