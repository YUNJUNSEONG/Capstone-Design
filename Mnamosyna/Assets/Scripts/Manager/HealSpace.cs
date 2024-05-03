using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealSpace : MonoBehaviour
{
    public int healPlayer = 100;
    SkillManager skillManager;

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

            OpenLevelUpUI(skillManager);

            OpenUnlockUpUI(skillManager);


            Destroy(gameObject);
        }
    }

    public void OpenUnlockUpUI(SkillManager skillManager)
    {
        // 스킬 매니저의 Unlock메서드를 호출합니다.
        skillManager.Unlock();
    }

    public void OpenLevelUpUI(SkillManager skillManager)
    {
        // 스킬 매니저의 LevelUp 메서드를 호출합니다.
        skillManager.LevelUp();
    }
}
