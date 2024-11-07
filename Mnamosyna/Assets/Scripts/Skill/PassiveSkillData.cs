using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal.Internal;

[CreateAssetMenu(fileName = "skill", menuName = "ScriptableObject/PassiveskillData")]
public class PassiveSkillData : SkillData
{
    public enum PassiveSkillType { None, AOE, Blast, Missile, Beam }

    public PassiveSkillType passiveSkillType;
    public GameObject effectPrefab;
    public float effectInterval; // 공격 주기
    private float currentEffectInterval;
    public int effectDamage;
    private bool isEffectAttack = false;

    private Coroutine effectCoroutine;

    public void StartEffect(GameObject player)
    {
        if (effectCoroutine == null)
            effectCoroutine = player.GetComponent<MonoBehaviour>().StartCoroutine(EffectRoutine(player));
    }

    public void StopEffect(GameObject player)
    {
        if (effectCoroutine != null)
        {
            player.GetComponent<MonoBehaviour>().StopCoroutine(effectCoroutine);
            effectCoroutine = null;
        }
    }

    private IEnumerator EffectRoutine(GameObject player)
    {
        while (true)
        {
            TriggerEffect(player);
            yield return new WaitForSeconds(effectInterval);
        }
    }

    public void TriggerEffect(GameObject player)
    {
        // Player 스크립트 가져오기
        Player playerStats = player.GetComponent<Player>();

        if (playerStats != null && !playerStats.isStatApplied)
        {
            // 스탯이 한 번만 적용되도록 체크
            switch (passiveSkillType)
            {
                case PassiveSkillType.AOE:
                    SpawnAOEEffect(player);
                    ApplyStatBoost(player, Level); // 첫 번째 호출 시에만 스탯 적용
                    break;
                case PassiveSkillType.Blast:
                    SpawnBlastEffect(player);
                    ApplyStatBoost(player, Level); // 첫 번째 호출 시에만 스탯 적용
                    break;
                case PassiveSkillType.Missile:
                    SpawnMissileEffect(player);
                    ApplyStatBoost(player, Level); // 첫 번째 호출 시에만 스탯 적용
                    break;
                case PassiveSkillType.Beam:
                    SpawnBeamEffect(player);
                    ApplyStatBoost(player, Level); // 첫 번째 호출 시에만 스탯 적용
                    break;
                case PassiveSkillType.None:
                    ApplyStatBoost(player, Level); // 첫 번째 호출 시에만 스탯 적용
                    break;
            }

            // 스탯이 적용된 후, isStatApplied를 true로 설정
            playerStats.isStatApplied = true;
        }
        else
        {
            // 스탯은 한 번만 적용되므로, 이후에는 이펙트만 업데이트
            switch (passiveSkillType)
            {
                case PassiveSkillType.AOE:
                    SpawnAOEEffect(player);
                    break;
                case PassiveSkillType.Blast:
                    SpawnBlastEffect(player);
                    break;
                case PassiveSkillType.Missile:
                    SpawnMissileEffect(player);
                    break;
                case PassiveSkillType.Beam:
                    SpawnBeamEffect(player);
                    break;
                case PassiveSkillType.None:
                    break;
            }
        }
    }



    public void SpawnAOEEffect(GameObject player)
    {
        // 이펙트 간격 타이머 감소
        currentEffectInterval -= Time.deltaTime;
        if (!isEffectAttack)
        {
            float yOffset = 0.5f; // y축으로 올리고 싶은 높이

            // 플레이어 주변에 원형 범위 이펙트 생성 위치 설정
            Vector3 effectPosition = player.transform.position + Vector3.up * yOffset;
            GameObject effect = Instantiate(effectPrefab, effectPosition, Quaternion.identity);

            effect.transform.localScale = new Vector3(5f, 5f, 5f); // 원하는 범위 크기 조정
            Destroy(effect, 1f); // 이펙트를 1초 후에 자동 삭제

            // 범위 내 적들에게 데미지 적용 (y축 1만큼 올린 위치 사용)
            Collider[] hitColliders = Physics.OverlapSphere(effectPosition, 5f); // 범위 반경 5
            foreach (var hitCollider in hitColliders)
            {
                BaseMonster enemy = hitCollider.GetComponent<BaseMonster>();
                if (enemy != null)
                {
                    enemy.TakeDamage(effectDamage); // 적에게 데미지 전달
                }
            }
            currentEffectInterval = effectInterval;
            isEffectAttack = true;
        }

        currentEffectInterval -= Time.deltaTime;
        if (currentEffectInterval <= 0)
        {
            isEffectAttack = false;
        }
    }



    public void SpawnBlastEffect(GameObject player)
    {
        // 이펙트 간격 타이머 감소
        currentEffectInterval -= Time.deltaTime;

        // 타이머가 0 이하가 되면 이펙트가 실행되고 타이머를 리셋
        if (currentEffectInterval <= 0 && !isEffectAttack)
        {
            float yOffset = 1.5f; // y축으로 올리고 싶은 높이

            // 플레이어 앞쪽으로 나타나는 이펙트 위치 설정
            Vector3 blastPosition = player.transform.position + player.transform.forward * 2.0f + Vector3.up * yOffset;
            GameObject effect = Instantiate(effectPrefab, blastPosition, player.transform.rotation);

            Destroy(effect, 1.5f); // 이펙트를 1.5초 후에 자동 삭제

            // 범위 내 적들에게 데미지 적용 (앞쪽으로 3 단위 반경 설정)
            Collider[] hitColliders = Physics.OverlapSphere(blastPosition, 3f); // 범위 반경 3
            foreach (var hitCollider in hitColliders)
            {
                BaseMonster enemy = hitCollider.GetComponent<BaseMonster>();
                if (enemy != null)
                {
                    enemy.TakeDamage(effectDamage); // 적에게 데미지 전달
                }
            }

            // 이펙트 발동 상태 설정
            isEffectAttack = true;
            currentEffectInterval = effectInterval; // 타이머 리셋
        }

        // 이펙트 발동 상태를 해제
        if (isEffectAttack && currentEffectInterval <= 0)
        {
            isEffectAttack = false;
        }
    }



    public void SpawnMissileEffect(GameObject player)
    {
        // 빔이 나갈 네 가지 방향 정의 (앞, 뒤, 오른쪽, 왼쪽)
        Vector3[] directions = {
        player.transform.forward,         // 앞
        -player.transform.forward,        // 뒤
        player.transform.right,           // 오른쪽
        -player.transform.right           // 왼쪽
    };
        // 이펙트 간격 타이머 감소
        currentEffectInterval -= Time.deltaTime;

        if (!isEffectAttack)
        {
            float yOffset = 1.0f; // y축으로 올리고 싶은 높이
            Vector3 spawnPosition = player.transform.position + Vector3.up * yOffset; // y축 보정된 위치

            // Instantiate a single missile at the player's position
            GameObject missile = Instantiate(effectPrefab, spawnPosition, Quaternion.identity);
            Rigidbody rb = missile.GetComponent<Rigidbody>();
            if (rb == null)
            {
                rb = missile.AddComponent<Rigidbody>();
            }
            rb.useGravity = false; // Disable gravity if you don't want gravity to affect the missile
            rb.velocity = player.transform.forward * 10f; // Set missile speed in the chosen direction

            // If the missile doesn't have a Collider, add one and set it as a trigger
            Collider col = missile.GetComponent<Collider>();
            if (col == null)
            {
                col = missile.AddComponent<SphereCollider>();
            }
            col.isTrigger = true;

            // Add the MissileTriggerHandler to handle trigger events
            MissileTriggerHandler triggerHandler = missile.AddComponent<MissileTriggerHandler>();
            triggerHandler.SetSkillData(this); // Set reference to PassiveSkillData

            // Destroy the missile after 4 seconds
            Destroy(missile, 4f);
            currentEffectInterval = effectInterval;
            isEffectAttack = true;
        }

        if (currentEffectInterval <= 0)
        {
            isEffectAttack = false;
        }
    }



    // MissileTriggerHandler 클래스 정의
    public class MissileTriggerHandler : MonoBehaviour
    {
        private PassiveSkillData skillData;

        public void SetSkillData(PassiveSkillData data)
        {
            skillData = data;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (skillData == null) return;

            BaseMonster enemy = other.GetComponent<BaseMonster>();
            if (enemy != null)
            {
                enemy.TakeDamage(skillData.effectDamage); // PassiveSkillData의 effectDamage 사용
                Destroy(gameObject); // 충돌 후 투사체 삭제
            }
        }
    }
    public void SpawnBeamEffect(GameObject player)
    {
        // 빔이 나갈 네 가지 방향 정의 (앞, 뒤, 오른쪽, 왼쪽)
        Vector3[] directions = {
        player.transform.forward,         // 앞
        -player.transform.forward,        // 뒤
        player.transform.right,           // 오른쪽
        -player.transform.right           // 왼쪽
    };
        // 이펙트 간격 타이머 감소
        currentEffectInterval -= Time.deltaTime;
        if (!isEffectAttack)
        {
            foreach (var direction in directions)
            {
                float yOffset = 1.0f; // y축으로 올리고 싶은 높이
                // Beam 효과를 플레이어 위치에서 생성
                GameObject beamEffect = Instantiate(effectPrefab, player.transform.position, Quaternion.LookRotation(direction));

                // Beam을 2초 후에 자동 삭제
                Destroy(beamEffect, 1.5f);

                // yOffset을 적용하여 범위 중앙 위치를 위로 올림
                Vector3 boxCenter = player.transform.position + direction * 2.5f + Vector3.up * yOffset;

                // Beam 효과의 Collider를 통해 범위 내 적들에게 데미지 적용
                Collider[] hitColliders = Physics.OverlapBox(
                    boxCenter,                               // 범위 중앙 위치
                    new Vector3(1f, 1f, 5f),                 // 범위 크기 (폭과 높이)
                    Quaternion.LookRotation(direction)       // 범위 방향
                );

                foreach (var hitCollider in hitColliders)
                {
                    BaseMonster enemy = hitCollider.GetComponent<BaseMonster>();
                    if (enemy != null)
                    {
                        enemy.TakeDamage(effectDamage); // 적에게 데미지 전달
                    }
                }
                currentEffectInterval = effectInterval;
                isEffectAttack = true;
            }
        }

        if (currentEffectInterval <= 0)
        {
            isEffectAttack = false;
        }


    }


    public void ApplyStatBoost(GameObject player, int Level)
    {
        Player playerStats = player.GetComponent<Player>();
        if (playerStats != null)
        {
            // 기본 체력 보너스 적용
            playerStats.max_hp += MaxHPBonus;
            playerStats.cur_hp += MaxHPBonus;
            playerStats.max_hp += LinkMaxHPBonus;
            playerStats.cur_hp += LinkMaxHPBonus;
            // 스킬 레벨에 따라 추가 체력 보너스 적용
            if (Level > 1) // 예: 스킬 레벨이 2 이상일 때 강화 보너스 적용
            {
                playerStats.max_hp += addMaxHPBonus * (Level - 1);
                playerStats.cur_hp += addMaxHPBonus * (Level - 1);
                playerStats.max_hp += addLinkMaxHPBonus * (Level - 1);
                playerStats.cur_hp += addLinkMaxHPBonus * (Level - 1);
            }

            // 다른 스탯 보너스도 같은 방식으로 적용
            playerStats.max_atk *= (1 + ATKBonus);
            playerStats.min_atk *= (1 + ATKBonus);
            playerStats.max_atk *= (1 + LinkATKBonus);
            playerStats.min_atk *= (1 + LinkATKBonus);
            if (Level > 1)
            {
                playerStats.max_atk *= (1 + addATKBonus * (Level - 1));
                playerStats.min_atk *= (1 + addATKBonus * (Level - 1));
                playerStats.max_atk *= (1 + addLinkATKBonus * (Level - 1));
                playerStats.min_atk *= (1 + addLinkATKBonus * (Level - 1));
            }

            playerStats.crit_chance += CritChanceBonus;
            playerStats.crit_chance += LinkCritChanceBonus;
            if (Level > 1)
            {
                playerStats.crit_chance += addCritChanceBonus * (Level - 1);
                playerStats.crit_chance += addLinkCritChanceBonus * (Level - 1);
            }

            playerStats.critical += CriticalBonus;
            playerStats.critical += LinkCriticalBonus;
            if (Level > 1)
            {
                playerStats.critical += addCriticalBonus * (Level - 1);
                playerStats.critical += addLinkCriticalBonus * (Level - 1);
            }

            // 방어력 및 속도 보너스도 같은 방식으로 적용
            playerStats.defense += DefenseBonus;
            playerStats.defense += LinkDefenseBonus;
            if (Level > 1)
            {
                playerStats.defense += addDefenseBonus * (Level - 1);
                playerStats.defense += addLinkDefenseBonus * (Level - 1);
            }

            playerStats.atk_speed -= AttackSpeedMultiplier;
            playerStats.atk_speed -= LinkAttackSpeedMultiplier;
            if (Level > 1)
            {
                playerStats.atk_speed -= addAttackSpeedMultiplier *(Level - 1);
                playerStats.atk_speed -= addLinkAttackSpeedMultiplier *(Level - 1);
            }

            playerStats.move_speed += MoveSpeedMultiplier;
            playerStats.move_speed += LinkMoveSpeedMultiplier; ;
            if (Level > 1)
            {
                playerStats.move_speed += addMoveSpeedMultiplier * (Level - 1);
                playerStats.move_speed += addLinkMoveSpeedMultiplier * (Level - 1);
            }

            // 체력 및 스태미너 회복량
            playerStats.hp_recover += HealthRecoverBonus;
            playerStats.hp_recover += LinkHealthRecoverBonus;
            if (Level > 1)
            {
                playerStats.hp_recover += addHealthRecoverBonus * (Level - 1);
                playerStats.hp_recover += addLinkHealthRecoverBonus * (Level - 1);
            }

            playerStats.stamina_recover += StaminaRecoverBonus;
            playerStats.stamina_recover += LinkStaminaRecoverBonus;
            if (Level > 1)
            {
                playerStats.stamina_recover += addStaminaRecoverBonus * (Level - 1);
                playerStats.stamina_recover += addLinkStaminaRecoverBonus * (Level - 1);
            }
        }
    }


    [Header("# Passive Skill Data")]

    [Header("Health Related")]
    [ShowWhen("element", Element.Earth)]
    public int MaxHPBonus;
    [ShowWhen("element", Element.Earth)]
    public int addMaxHPBonus;
    [ShowWhen("element", Element.Water)]
    public float HealthRecoverBonus;
    [ShowWhen("element", Element.Water)]
    public float addHealthRecoverBonus;

    [Header("Stamina Related")]
    [ShowWhen("element", Element.Water)]
    public float StaminaRecoverBonus;
    [ShowWhen("element", Element.Water)]
    public float addStaminaRecoverBonus;

    [Header("Attack Related")]
    [ShowWhen("element", Element.Fire)]
    public float ATKBonus;
    [ShowWhen("element", Element.Fire)]
    public float addATKBonus;
    [ShowWhen("element", Element.Fire)]
    public float CritChanceBonus;
    [ShowWhen("element", Element.Fire)]
    public float addCritChanceBonus;
    [ShowWhen("element", Element.Fire)]
    public float CriticalBonus;
    [ShowWhen("element", Element.Fire)]
    public float addCriticalBonus;

    [Header("Defense")]
    [ShowWhen("element", Element.Earth)]
    public float DefenseBonus;
    [ShowWhen("element", Element.Earth)]
    public float addDefenseBonus;

    [Header("Speed Related")]
    [ShowWhen("element", Element.Air)]
    public float AttackSpeedMultiplier;
    [ShowWhen("element", Element.Air)]
    public float addAttackSpeedMultiplier;
    [ShowWhen("element", Element.Air)]
    public float MoveSpeedMultiplier;
    [ShowWhen("element", Element.Air)]
    public float addMoveSpeedMultiplier;



    [Header("# Link Skill Data")]

    [Header("Health Related")]
    [ShowWhen("skillType", SkillType.Link, "linkElement", Element.Earth)]
    public int LinkMaxHPBonus;
    [ShowWhen("skillType", SkillType.Link, "linkElement", Element.Earth)]
    public int addLinkMaxHPBonus;
    [ShowWhen("skillType", SkillType.Link, "linkElement", Element.Water)]
    public float LinkHealthRecoverBonus;
    [ShowWhen("skillType", SkillType.Link, "linkElement", Element.Water)]
    public float addLinkHealthRecoverBonus;

    [Header("Stamina Related")]
    [ShowWhen("skillType", SkillType.Link, "linkElement", Element.Water)]
    public float LinkStaminaRecoverBonus;
    [ShowWhen("skillType", SkillType.Link, "linkElement", Element.Water)]
    public float addLinkStaminaRecoverBonus;

    [Header("Attack Related")]
    [ShowWhen("skillType", SkillType.Link, "linkElement", Element.Fire)]
    public float LinkATKBonus;
    [ShowWhen("skillType", SkillType.Link, "linkElement", Element.Fire)]
    public float addLinkATKBonus;
    [ShowWhen("skillType", SkillType.Link, "linkElement", Element.Fire)]
    public float LinkCritChanceBonus;
    [ShowWhen("skillType", SkillType.Link, "linkElement", Element.Fire)]
    public float addLinkCritChanceBonus;
    [ShowWhen("skillType", SkillType.Link, "linkElement", Element.Fire)]
    public float LinkCriticalBonus;
    [ShowWhen("skillType", SkillType.Link, "linkElement", Element.Fire)]
    public float addLinkCriticalBonus;

    [Header("Defense")]
    [ShowWhen("skillType", SkillType.Link, "linkElement", Element.Earth)]
    public float LinkDefenseBonus;
    [ShowWhen("skillType", SkillType.Link, "linkElement", Element.Earth)]
    public float addLinkDefenseBonus;

    [Header("Speed Related")]
    [ShowWhen("skillType", SkillType.Link, "linkElement", Element.Air)]
    public float LinkAttackSpeedMultiplier;
    [ShowWhen("skillType", SkillType.Link, "linkElement", Element.Air)]
    public float addLinkAttackSpeedMultiplier;
    [ShowWhen("skillType", SkillType.Link, "linkElement", Element.Air)]
    public float LinkMoveSpeedMultiplier;
    [ShowWhen("skillType", SkillType.Link, "linkElement", Element.Air)]
    public float addLinkMoveSpeedMultiplier;

}

/*
[Header("# Dimerit Passive Skill Data")]

[Header("Health Related")]
[ShowWhen("element", Element.Air)]
public int DimeritMaxHP;
[ShowWhen("element", Element.Fire)]
public float DimeritHealthRecover;

[Header("Stamina Related")]
[ShowWhen("element", Element.Fire)]
public float DimeritStaminaRecover;

[Header("Attack Related")]
[ShowWhen("element", Element.Water)]
public int DimeritATK;
[ShowWhen("element", Element.Water)]
public float DimeritCritChance;
[ShowWhen("element", Element.Water)]
public float DimeritCritical;

[Header("Defense")]
[ShowWhen("element", Element.Air)]
public float DimeritDefense;

[Header("Speed Related")]
[ShowWhen("element", Element.Earth)]
public float DimeritAttackSpeedMultiplier;
[ShowWhen("element", Element.Earth)]
public float DimeritMoveSpeedMultiplier;
*/
