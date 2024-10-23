using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSkillRange : MonoBehaviour
{
    public Boss boss;
    public Player player;
    public CapsuleCollider attackDistance;

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player")) // �浹�� ��ü�� �÷��̾����� Ȯ��
        {
            boss.ChangeState(Boss.State.Attack);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) // �浹�� ��ü�� �÷��̾����� Ȯ��
        {
            boss.ChangeState(Boss.State.Chase);
        }
    }

    void Awake()
    {
        SetAttackDistance();
    }

    void SetAttackDistance()
    {
        attackDistance.radius = boss.attack_radius;
    }

}
