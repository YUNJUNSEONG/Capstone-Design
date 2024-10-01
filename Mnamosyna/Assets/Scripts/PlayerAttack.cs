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
        // 현재 사용 중인 무기의 충돌체 선택
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
                // 두 개의 충돌체를 처리
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
            waterCollider.enabled = true;  // 선택된 무기 충돌체 활성화
        }
        if (fireCollider != null)
        {
            fireCollider.enabled = true;  // 선택된 무기 충돌체 활성화
        }
        // earth,air 무기의 경우 두 개의 충돌체 모두 활성화
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
            waterCollider.enabled = false;  // 선택된 무기 충돌체 활성화
        }
        if (fireCollider != null)
        {
            fireCollider.enabled = false;  // 선택된 무기 충돌체 활성화
        }
        // earth 무기의 경우 두 개의 충돌체 모두 비활성화
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

    // 몬스터와의 충돌을 감지하는 메서드
    private void OnTriggerEnter(Collider other)
    {
        if (isAttacking && other.CompareTag("Monster"))
        {
            BaseMonster monster = other.GetComponent<BaseMonster>();
            if (monster != null)
            {
                monster.TakeDamage(player.Damage());  // 몬스터에게 데미지 적용
            }
        }
    }
}



