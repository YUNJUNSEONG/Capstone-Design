using System.Collections.Generic;
using UnityEngine;

public class BattleEndTuto : MonoBehaviour
{
    public StoryManager storyManager;
    public Spawner spawner;

    void Update()
    {
        if (spawner.isCombatEnded)
        {
            List<string> postCombatTutorialMessages = new List<string>
            {
                "전투가 끝났습니다.",
                "아이템을 획득하세요."
            };
            storyManager.StartTutorial(StoryManager.TutorialType.PostCombat, postCombatTutorialMessages);

            // 전투 종료 처리 후 isCombatEnded를 다시 false로 설정하여 중복 실행 방지
            spawner.isCombatEnded = false;

            // 오브젝트를 비활성화합니다.
            gameObject.SetActive(false);
        }
    }
}
