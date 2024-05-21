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
        if (other.CompareTag("Player")) // �浹�� ��ü�� �÷��̾����� Ȯ��
        {
            monster.ChangeState(Monster.MonsterState.Attack);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) // �浹�� ��ü�� �÷��̾����� Ȯ��
        {
            monster.ChangeState(Monster.MonsterState.Chase);
        }
    }

    void Awake()
    {
        // ������Ʈ�� �� ���� CapsuleCollider�� �ִٰ� �����ϰ� �ùٸ��� �Ҵ�
        CapsuleCollider[] colliders = GetComponents<CapsuleCollider>();
        if (colliders.Length == 2)
        {
            attackDistance = colliders[0];
            skillDistance = colliders[1];
        }
        else
        {
            Debug.LogError("CapsuleCollider ������Ʈ�� �� ������ �մϴ�");
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
