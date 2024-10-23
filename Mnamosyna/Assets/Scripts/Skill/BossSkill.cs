using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class BossSkill : MonoBehaviour
{
    public GameObject UnlockUI;
    public GameObject LevelupUI;
    public StoryManager storyManager;
    private SkillManager skillManager; // 충돌한 오브젝트에서 스킬 매니저 컴포넌트 가져오기
    public int healPlayer = 100;
    public Magic0[] magicComponents;

    public bool UnlockSkillSelectEnd = false;
    private static bool storyShown = false;

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
            BossMemory(other.gameObject);
        }
    }

    public void BossMemory(GameObject playerObject)
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
