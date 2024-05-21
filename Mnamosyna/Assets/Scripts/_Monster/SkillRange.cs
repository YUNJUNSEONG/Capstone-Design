using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillRange : MonoBehaviour
{
    public Monster monster;
    public Player player;
    public CapsuleCollider attackDistance;
    public CapsuleCollider skillDistance;

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player")) // 충돌한 객체가 플레이어인지 확인
        {
            monster.ChangeState(Monster.MonsterState.Attack);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) // 충돌한 객체가 플레이어인지 확인
        {
            monster.ChangeState(Monster.MonsterState.Chase);
        }
    }

    void Awake()
    {
        // 오브젝트에 두 개의 CapsuleCollider가 있다고 가정하고 올바르게 할당
        CapsuleCollider[] colliders = GetComponents<CapsuleCollider>();
        if (colliders.Length == 2)
        {
            attackDistance = colliders[0];
            skillDistance = colliders[1];
        }
        else
        {
            Debug.LogError("CapsuleCollider 컴포넌트가 두 개여야 합니다");
        }
        SetAttackDistance();
        SetSkillDistance();
    }

    void SetAttackDistance()
    {
        attackDistance.radius = monster.attack1_radius;
    }

    void SetSkillDistance()
    {
        skillDistance.radius = monster.attack2_radius;
    }
}
