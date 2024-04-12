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
    public Monster monster; 
    public CapsuleCollider Distance;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            //Debug.Log("플레이어가 범위 안에 들어옴");
            monster.ChangeState(Monster.MonsterState.Chase);
        }
    }

    void Awake()
    {
        Distance = GetComponent<CapsuleCollider>();
        SetDetectionDistance();
    }

    void SetDetectionDistance()
    {
        Distance.radius = monster.patrol_radius;
    }
}
