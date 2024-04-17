using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UnlockSkill : MonoBehaviour
{
    public GameObject LevelupUI;
    SkillManager skillManager; // 충돌한 오브젝트에서 스킬 매니저 컴포넌트 가져오기

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>(); // 충돌한 오브젝트에서 플레이어 컴포넌트 가져오기
            if (player != null)
            {
                OpenUnlockUpUI(skillManager);
            }
        }
    }

    public void OpenUnlockUpUI(SkillManager skillManager)
    {
        // 스킬 매니저의 LevelUp 메서드를 호출합니다.
        skillManager.Unlock();
    }
}