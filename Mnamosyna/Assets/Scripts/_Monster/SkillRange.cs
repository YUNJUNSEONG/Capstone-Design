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
        monster.ChangeState(Monster.MonsterState.Attack);

        if (monster.isAttack)
        {
            if (monster.isSkill)
            {
                if (other.gameObject.TryGetComponent(out Player player))
                {
                    player.TakeDamage(monster.Damage(1)); //monster의 데미지 1번 = 스킬 공격
                }
            }
            else
            {
                if (other.gameObject.TryGetComponent(out Player player))
                {
                    player.TakeDamage(monster.Damage(0)); //monster의 데미지 0번 = 기본 공격
                }
            }

        }

    }

    private void OnTriggerExit(Collider other)
    {
        monster.ChangeState(Monster.MonsterState.Chase);
    }

    void Awake()
    {
        attackDistance = GetComponent<CapsuleCollider>();
        skillDistance = GetComponent<CapsuleCollider>();
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
