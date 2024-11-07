using System.Collections.Generic;
using UnityEngine;

public class BattleEndTuto : MonoBehaviour
{
    public StoryManager storyManager;
    public Spawner spawner;

    void Update()
    {
        if (spawner.IsCombatEnded)
        {
            List<string> postCombatTutorialMessages = new()
            {
                "모든 몬스터를 처치했어요.\r\n이제 이곳에서는 몬스터가 나오지 않을거예요.",
                "저길 보세요!",
                "저건 당신이 잃어버린 기억의 조각이에요.\r\n당신이 잊고 있던 기술을 떠올릴 수 있을거예요.",
                "E키를 눌러서 조각을 회수하세요."
            };
            storyManager.StartTutorial(StoryManager.TutorialType.PostCombat, postCombatTutorialMessages);

            // 전투 종료 처리 후 isCombatEnded를 다시 false로 설정하여 중복 실행 방지
            spawner.IsCombatEnded = false;

            // 오브젝트를 비활성화합니다.
            gameObject.SetActive(false);
        }
    }
}
