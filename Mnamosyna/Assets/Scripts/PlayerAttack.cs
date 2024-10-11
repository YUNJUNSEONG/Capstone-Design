using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerAttack : MonoBehaviour
{
    public Player player;
    public bool isAttacking = false;

    public Collider waterCollider;
    public Collider fireCollider;
    public Collider air1Collider;
    public Collider air2Collider;
    public Collider earth1Collider;
    public Collider earth2Collider;

    private void Start()
    {
        player = GetComponentInChildren<Player>();
        DisableAllColliders();  // ���� �� ��� �浹ü�� ��Ȱ��ȭ
    }


    // ��� �浹ü�� ��Ȱ��ȭ�ϴ� �޼���
    private void DisableAllColliders()
    {
        if (waterCollider != null) waterCollider.enabled = false;
        if (fireCollider != null) fireCollider.enabled = false;
        if (air1Collider != null) air1Collider.enabled = false;
        if (air2Collider != null) air2Collider.enabled = false;
        if (earth1Collider != null) earth1Collider.enabled = false;
        if (earth2Collider != null) earth2Collider.enabled = false;
    }

    public void SetActiveSword(int swordNumber)
    {
        // ��� ���� �浹ü ��Ȱ��ȭ
        DisableAllColliders();

        // ���� ��� ���� ������ �浹ü ���� �� Ȱ��ȭ
        switch (swordNumber)
        {
            case 1:
                waterCollider = player.waterCollider;
                waterCollider.enabled = true;
                break;
            case 2:
                fireCollider = player.fireCollider;
                fireCollider.enabled = true;
                break;
            case 3:
                air1Collider = player.air1Collider;
                air2Collider = player.air2Collider;
                air1Collider.enabled = true;
                air2Collider.enabled = true;
                break;
            case 4:
                earth1Collider = player.earth1Collider;
                earth2Collider = player.earth2Collider;
                earth1Collider.enabled = true;
                earth2Collider.enabled = true;
                break;
            default:
                Debug.LogWarning("Invalid sword number");
                return;
        }
    }

    public void EnableSwordCollider()
    {
        isAttacking = true;

        // ���� ���õ� ������ �浹ü Ȱ��ȭ (�̹� Ȱ��ȭ�� ������ ��� �ߺ� Ȱ��ȭ ����)
        if (waterCollider != null && !waterCollider.enabled)
        {
            waterCollider.enabled = true;
        }
        if (fireCollider != null && !fireCollider.enabled)
        {
            fireCollider.enabled = true;
        }
        if (earth1Collider != null && !earth1Collider.enabled)
        {
            earth1Collider.enabled = true;
            earth2Collider.enabled = true;
        }
        if (air1Collider != null && !air1Collider.enabled)
        {
            air1Collider.enabled = true;
            air2Collider.enabled = true;
        }
    }

    public void DisableSwordCollider()
    {
        isAttacking = false;

        // ���� ���õ� ������ �浹ü ��Ȱ��ȭ
        if (waterCollider != null)
        {
            waterCollider.enabled = false;
        }
        if (fireCollider != null)
        {
            fireCollider.enabled = false;
        }
        if (earth1Collider != null && earth2Collider != null)
        {
            earth1Collider.enabled = false;
            earth2Collider.enabled = false;
        }
        if (air1Collider != null && air2Collider != null)
        {
            air1Collider.enabled = false;
            air2Collider.enabled = false;
        }
    }

    // ���Ϳ��� �浹�� �����ϴ� �޼���
    private void OnTriggerEnter(Collider other)
    {
        if (isAttacking && other.CompareTag("Monster"))
        {
            BaseMonster monster = other.GetComponent<BaseMonster>();
            if (monster != null)
            {
                monster.TakeDamage(player.Damage());  // ���Ϳ��� ������ ����
            }
        }
    }
}
