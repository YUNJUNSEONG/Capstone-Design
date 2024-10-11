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
        DisableAllColliders();  // 시작 시 모든 충돌체를 비활성화
    }


    // 모든 충돌체를 비활성화하는 메서드
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
        // 모든 무기 충돌체 비활성화
        DisableAllColliders();

        // 현재 사용 중인 무기의 충돌체 선택 및 활성화
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

        // 현재 선택된 무기의 충돌체 활성화 (이미 활성화된 상태일 경우 중복 활성화 방지)
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

        // 현재 선택된 무기의 충돌체 비활성화
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
