using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstBattleEnd : MonoBehaviour
{
    public StoryManager storyManager;
    public Spawner spawner;

    void Update()
    {
        if (spawner.isCombatEnded)
        {
            List<string> potalExplanTutorialMessages = new List<string>
            {
                "여기서 부터는 갈림길이예요.",
                "푸른 포탈 너머에서는 당신이 잃어버린 기억들을 찾을 수 있을거예요.",
                "노란 포탈 너머로는 당신의 찾은 기억의 조각들을 강하게 만들 수 있을거예요.",
                "잘 선택하면서 앞으로 나아가죠."
            };
            storyManager.StartTutorial(StoryManager.TutorialType.Potal, potalExplanTutorialMessages);

            // 전투 종료 처리 후 isCombatEnded를 다시 false로 설정하여 중복 실행 방지
            spawner.isCombatEnded = false;

            // 오브젝트를 비활성화합니다.
            gameObject.SetActive(false);
        }
    }
}
