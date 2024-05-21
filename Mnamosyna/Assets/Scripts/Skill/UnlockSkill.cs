using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockSkill : MonoBehaviour
{
    public GameObject LevelupUI;
    public StoryManager storyManager;
    private SkillManager skillManager; // 충돌한 오브젝트에서 스킬 매니저 컴포넌트 가져오기

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
            Player player = other.GetComponent<Player>(); // 충돌한 오브젝트에서 플레이어 컴포넌트 가져오기
            if (player != null)
            {
                // 튜토리얼 메시지 리스트 생성 및 스토리 매니저 호출
                List<string> skillUnlockTutorialMessages = new List<string>
                {
                    "스킬을 선택하세요.",
                    "스킬 선택 후 계속 진행하세요."
                };
                storyManager.StartTutorial(StoryManager.TutorialType.SkillUnlock, skillUnlockTutorialMessages);

                // 스킬 언락 UI 열기 및 스킬 언락 메서드 호출
                OpenUnlockUpUI(skillManager);
            }
        }
    }

    public void OpenUnlockUpUI(SkillManager skillManager)
    {
        // 스킬 매니저의 Unlock 메서드를 호출합니다.
        skillManager.Unlock();

        // 오브젝트 파괴
        Destroy(gameObject);
    }
}
