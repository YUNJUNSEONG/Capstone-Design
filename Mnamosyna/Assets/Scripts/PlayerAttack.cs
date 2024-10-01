using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

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
    }

    public void SetActiveSword(int swordNumber)
    {
        // ���� ��� ���� ������ �浹ü ����
        switch (swordNumber)
        {
            case 1:
                waterCollider = player.waterCollider;
                break;
            case 2:
                fireCollider = player.fireCollider;
                break;
            case 3:
                air1Collider = player.air1Collider;
                air2Collider = player.air2Collider;
                break;
            case 4:
                // �� ���� �浹ü�� ó��
                earth1Collider = player.earth1Collider;
                earth2Collider = player.earth2Collider;
                break;
            default:
                Debug.LogWarning("Invalid sword number");
                return;
        }
    }

    public void EnableSwordCollider()
    {
        isAttacking = true;
        if (waterCollider != null)
        {
            waterCollider.enabled = true;  // ���õ� ���� �浹ü Ȱ��ȭ
        }
        if (fireCollider != null)
        {
            fireCollider.enabled = true;  // ���õ� ���� �浹ü Ȱ��ȭ
        }
        // earth,air ������ ��� �� ���� �浹ü ��� Ȱ��ȭ
        if (earth1Collider != null && earth2Collider != null)
        {
            earth1Collider.enabled = true;
            earth2Collider.enabled = true;
        }
        if (air1Collider != null && air2Collider != null)
        {
            air1Collider.enabled = true;
            air2Collider.enabled = true;
        }
    }

    public void DisableSwordCollider()
    {
        isAttacking = false;
        if (waterCollider != null)
        {
            waterCollider.enabled = false;  // ���õ� ���� �浹ü Ȱ��ȭ
        }
        if (fireCollider != null)
        {
            fireCollider.enabled = false;  // ���õ� ���� �浹ü Ȱ��ȭ
        }
        // earth ������ ��� �� ���� �浹ü ��� ��Ȱ��ȭ
        if (earth1Collider != null && earth2Collider != null)
        {
            earth1Collider.enabled = false;
            earth2Collider.enabled = false;
        }
        if(air1Collider != null && air2Collider != null)
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



