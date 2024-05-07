using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HealSpace : MonoBehaviour
{
    public int healPlayer = 100;
    SkillManager skillManager;

    public Magic0[] magicComponents;

    private void Start()
    {
        skillManager = FindObjectOfType<SkillManager>(); // scene에서 SkillManager 오브젝트를 찾아 할당
        if (skillManager == null)
        {
            Debug.LogError("SkillManager를 찾을 수 없습니다.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            HealPlayer(other.gameObject);
        }
    }

    public void HealPlayer(GameObject playerObject)
    {
        PlayerStat player = playerObject.GetComponent<PlayerStat>();

        if (player != null)
        {
            Debug.Log("Player Heal!");
            player.cur_hp += healPlayer;
            player.cur_stamina += healPlayer;   

            OpenUnlockUpUI(skillManager, () =>
            {
                // Unlock이 완료되면 LevelUp 호출
                OpenLevelUpUI(skillManager);
            });

            Destroy(gameObject);

            foreach (Magic0 magic in magicComponents) { magic.EnableComponents(); }
        }
    }

    public void OpenUnlockUpUI(SkillManager skillManager, Action onUnlockComplete)
    {
        // 스킬 매니저의 Unlock 메서드를 호출하고, 완료되면 콜백 호출
        skillManager.Unlock(() =>
        {
            if (onUnlockComplete != null)
                onUnlockComplete.Invoke(); // 콜백 호출
        });
    }


    public void OpenLevelUpUI(SkillManager skillManager)
    {
        // 스킬 매니저의 LevelUp 메서드를 호출합니다.
        skillManager.LevelUp();
    }
}
