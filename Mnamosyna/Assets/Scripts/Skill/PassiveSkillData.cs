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
    public float effectInterval; // ���� �ֱ�
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
        // Player ��ũ��Ʈ ��������
        Player playerStats = player.GetComponent<Player>();

        if (playerStats != null && !playerStats.isStatApplied)
        {
            // ������ �� ���� ����ǵ��� üũ
            switch (passiveSkillType)
            {
                case PassiveSkillType.AOE:
                    SpawnAOEEffect(player);
                    ApplyStatBoost(player, Level); // ù ��° ȣ�� �ÿ��� ���� ����
                    break;
                case PassiveSkillType.Blast:
                    SpawnBlastEffect(player);
                    ApplyStatBoost(player, Level); // ù ��° ȣ�� �ÿ��� ���� ����
                    break;
                case PassiveSkillType.Missile:
                    SpawnMissileEffect(player);
                    ApplyStatBoost(player, Level); // ù ��° ȣ�� �ÿ��� ���� ����
                    break;
                case PassiveSkillType.Beam:
                    SpawnBeamEffect(player);
                    ApplyStatBoost(player, Level); // ù ��° ȣ�� �ÿ��� ���� ����
                    break;
                case PassiveSkillType.None:
                    ApplyStatBoost(player, Level); // ù ��° ȣ�� �ÿ��� ���� ����
                    break;
            }

            // ������ ����� ��, isStatApplied�� true�� ����
            playerStats.isStatApplied = true;
        }
        else
        {
            // ������ �� ���� ����ǹǷ�, ���Ŀ��� ����Ʈ�� ������Ʈ
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
        // ����Ʈ ���� Ÿ�̸� ����
        currentEffectInterval -= Time.deltaTime;
        if (!isEffectAttack)
        {
            float yOffset = 0.5f; // y������ �ø��� ���� ����

            // �÷��̾� �ֺ��� ���� ���� ����Ʈ ���� ��ġ ����
            Vector3 effectPosition = player.transform.position + Vector3.up * yOffset;
            GameObject effect = Instantiate(effectPrefab, effectPosition, Quaternion.identity);

            effect.transform.localScale = new Vector3(5f, 5f, 5f); // ���ϴ� ���� ũ�� ����
            Destroy(effect, 1f); // ����Ʈ�� 1�� �Ŀ� �ڵ� ����

            // ���� �� ���鿡�� ������ ���� (y�� 1��ŭ �ø� ��ġ ���)
            Collider[] hitColliders = Physics.OverlapSphere(effectPosition, 5f); // ���� �ݰ� 5
            foreach (var hitCollider in hitColliders)
            {
                BaseMonster enemy = hitCollider.GetComponent<BaseMonster>();
                if (enemy != null)
                {
                    enemy.TakeDamage(effectDamage); // ������ ������ ����
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
        // ����Ʈ ���� Ÿ�̸� ����
        currentEffectInterval -= Time.deltaTime;

        // Ÿ�̸Ӱ� 0 ���ϰ� �Ǹ� ����Ʈ�� ����ǰ� Ÿ�̸Ӹ� ����
        if (currentEffectInterval <= 0 && !isEffectAttack)
        {
            float yOffset = 1.5f; // y������ �ø��� ���� ����

            // �÷��̾� �������� ��Ÿ���� ����Ʈ ��ġ ����
            Vector3 blastPosition = player.transform.position + player.transform.forward * 2.0f + Vector3.up * yOffset;
            GameObject effect = Instantiate(effectPrefab, blastPosition, player.transform.rotation);

            Destroy(effect, 1.5f); // ����Ʈ�� 1.5�� �Ŀ� �ڵ� ����

            // ���� �� ���鿡�� ������ ���� (�������� 3 ���� �ݰ� ����)
            Collider[] hitColliders = Physics.OverlapSphere(blastPosition, 3f); // ���� �ݰ� 3
            foreach (var hitCollider in hitColliders)
            {
                BaseMonster enemy = hitCollider.GetComponent<BaseMonster>();
                if (enemy != null)
                {
                    enemy.TakeDamage(effectDamage); // ������ ������ ����
                }
            }

            // ����Ʈ �ߵ� ���� ����
            isEffectAttack = true;
            currentEffectInterval = effectInterval; // Ÿ�̸� ����
        }

        // ����Ʈ �ߵ� ���¸� ����
        if (isEffectAttack && currentEffectInterval <= 0)
        {
            isEffectAttack = false;
        }
    }



    public void SpawnMissileEffect(GameObject player)
    {
        // ���� ���� �� ���� ���� ���� (��, ��, ������, ����)
        Vector3[] directions = {
        player.transform.forward,         // ��
        -player.transform.forward,        // ��
        player.transform.right,           // ������
        -player.transform.right           // ����
    };
        // ����Ʈ ���� Ÿ�̸� ����
        currentEffectInterval -= Time.deltaTime;

        if (!isEffectAttack)
        {
            float yOffset = 1.0f; // y������ �ø��� ���� ����
            Vector3 spawnPosition = player.transform.position + Vector3.up * yOffset; // y�� ������ ��ġ

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



    // MissileTriggerHandler Ŭ���� ����
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
                enemy.TakeDamage(skillData.effectDamage); // PassiveSkillData�� effectDamage ���
                Destroy(gameObject); // �浹 �� ����ü ����
            }
        }
    }
    public void SpawnBeamEffect(GameObject player)
    {
        // ���� ���� �� ���� ���� ���� (��, ��, ������, ����)
        Vector3[] directions = {
        player.transform.forward,         // ��
        -player.transform.forward,        // ��
        player.transform.right,           // ������
        -player.transform.right           // ����
    };
        // ����Ʈ ���� Ÿ�̸� ����
        currentEffectInterval -= Time.deltaTime;
        if (!isEffectAttack)
        {
            foreach (var direction in directions)
            {
                float yOffset = 1.0f; // y������ �ø��� ���� ����
                // Beam ȿ���� �÷��̾� ��ġ���� ����
                GameObject beamEffect = Instantiate(effectPrefab, player.transform.position, Quaternion.LookRotation(direction));

                // Beam�� 2�� �Ŀ� �ڵ� ����
                Destroy(beamEffect, 1.5f);

                // yOffset�� �����Ͽ� ���� �߾� ��ġ�� ���� �ø�
                Vector3 boxCenter = player.transform.position + direction * 2.5f + Vector3.up * yOffset;

                // Beam ȿ���� Collider�� ���� ���� �� ���鿡�� ������ ����
                Collider[] hitColliders = Physics.OverlapBox(
                    boxCenter,                               // ���� �߾� ��ġ
                    new Vector3(1f, 1f, 5f),                 // ���� ũ�� (���� ����)
                    Quaternion.LookRotation(direction)       // ���� ����
                );

                foreach (var hitCollider in hitColliders)
                {
                    BaseMonster enemy = hitCollider.GetComponent<BaseMonster>();
                    if (enemy != null)
                    {
                        enemy.TakeDamage(effectDamage); // ������ ������ ����
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
            // �⺻ ü�� ���ʽ� ����
            playerStats.max_hp += MaxHPBonus;
            playerStats.cur_hp += MaxHPBonus;
            playerStats.max_hp += LinkMaxHPBonus;
            playerStats.cur_hp += LinkMaxHPBonus;
            // ��ų ������ ���� �߰� ü�� ���ʽ� ����
            if (Level > 1) // ��: ��ų ������ 2 �̻��� �� ��ȭ ���ʽ� ����
            {
                playerStats.max_hp += addMaxHPBonus * (Level - 1);
                playerStats.cur_hp += addMaxHPBonus * (Level - 1);
                playerStats.max_hp += addLinkMaxHPBonus * (Level - 1);
                playerStats.cur_hp += addLinkMaxHPBonus * (Level - 1);
            }

            // �ٸ� ���� ���ʽ��� ���� ������� ����
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

            // ���� �� �ӵ� ���ʽ��� ���� ������� ����
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

            // ü�� �� ���¹̳� ȸ����
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
