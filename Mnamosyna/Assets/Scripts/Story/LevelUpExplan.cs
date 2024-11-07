using System.Collections.Generic;
using UnityEngine;

public class LevelUpExplan : MonoBehaviour
{
    public StoryManager storyManager;
    public List<Spawner> spawners;

    private bool tutorialShown = false; // 중복 실행 방지를 위한 플래그

    void Update()
    {
        if (tutorialShown)
            return;

        foreach (Spawner spawner in spawners)
        {
            if (spawner.IsCombatEnded)
            {
                List<string> LevelUpTutorialMessages = new()
                {
                    "저길 보세요!",
                    "저건 노란 기억의 조각이에요.\r\n당신이 되찾은 기술들을 더 강하게 해줄거예요.",
                    "E키를 눌러서 조각을 회수하세요."
                };
                storyManager.StartTutorial(StoryManager.TutorialType.SkillLevelUp, LevelUpTutorialMessages);

                // 튜토리얼이 한번 실행되었음을 표시
                tutorialShown = true;

                // 모든 스포너의 isCombatEnded를 다시 false로 설정하여 중복 실행 방지
                foreach (Spawner sp in spawners)
                {
                    sp.IsCombatEnded = false;
                }

                // 오브젝트를 비활성화합니다.
                gameObject.SetActive(false);

                // 하나의 스포너에서만 실행되도록 루프 종료
                break;
            }
        }
    }
}

