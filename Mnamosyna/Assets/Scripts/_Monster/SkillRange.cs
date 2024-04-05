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
                    player.TakeDamage(monster.Damage(1)); //monster의 데미지 1번 = 스킬 공격
                }
            }
            else
            {
                if (other.gameObject.TryGetComponent<PlayerMovement>(out PlayerMovement player))
                {
                    player.TakeDamage(monster.Damage(0)); //monster의 데미지 0번 = 기본 공격
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
