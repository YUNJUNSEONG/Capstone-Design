using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillRange : MonoBehaviour
{
    public Monster monster;
    public PlayerMovement player;
    public CapsuleCollider Distance;

    void OnTriggerStay(Collider other)
    {
        monster.ChangeState(Monster.MonsterState.Attack);

        if (monster.isAttack)
        {
            if (monster.isSkill)
            {
                if (other.gameObject.TryGetComponent<PlayerMovement>(out PlayerMovement player))
                {
                    player.TakeDamage(monster.Damage(1)); //monster�� ������ 1�� = ��ų ����
                }
            }
            else
            {
                if (other.gameObject.TryGetComponent<PlayerMovement>(out PlayerMovement player))
                {
                    player.TakeDamage(monster.Damage(0)); //monster�� ������ 0�� = �⺻ ����
                }
            }

        }

        //if (other.gameObject.tag == "Player"){monster.ChangeState(Monster.MonsterState.Attack);}

    }

    private void OnTriggerExit(Collider other)
    {
        monster.ChangeState(Monster.MonsterState.Chase);
        //if(other.gameObject.tag == "Player") {monster.ChangeState(Monster.MonsterState.Chase);}
    }

    void Awake()
    {
        Distance = GetComponent<CapsuleCollider>();
        SetAttackDistance();
    }

    void SetAttackDistance()
    {
        Distance.radius = monster.patrol_radius;
    }
}
