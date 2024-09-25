using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class firstBossTuto : MonoBehaviour
{
    public StoryManager storyManager;
    public BossSpawner spawner;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            List<string> FirstBossTutorialMessages = new List<string>
            {
                "여기는 이 구역을 지키는 사룡의 수하가 있을 거예요.",
                "저길 보세요.\r\n저건 이 구역의 주인인 드래곤이예요.",
                "사룡보다는 약하더라도 성체인 드래곤은 매우 강해요.",
                "지금까지 상대해왔던 해츨링과는 차원이 다른거예요.",
                "그러니 조심해서 상대하죠."
            };

            // 튜토리얼이 끝났을 때 BossSpawner를 실행하도록 이벤트 구독
            storyManager.OnTutorialFinished += OnTutorialFinished;

            // 튜토리얼 시작
            storyManager.StartTutorial(StoryManager.TutorialType.FirstBoss, FirstBossTutorialMessages);

            // 오브젝트를 비활성화합니다.
            gameObject.SetActive(false);
        }
    }

    private void OnTutorialFinished()
    {
        // 튜토리얼이 끝나면 스포너를 활성화합니다.
        spawner.SpawnWaves();

        // 이벤트 구독 해제
        storyManager.OnTutorialFinished -= OnTutorialFinished;
    }
}
