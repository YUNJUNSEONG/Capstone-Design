using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class secondBossTuto : MonoBehaviour
{
    public StoryManager storyManager;
    public BossSpawner spawner;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            List<string> FirstBossTutorialMessages = new List<string>
            {
                "저건 제 애완동물이였던 작은 거북이였어요.",
                "사룡의 힘때문에 저렇게 흉측하고 기괴하게 변하다니...",
                "더 이상 보기 힘드네요. 편하게 해주세요..."
            };

            // 튜토리얼이 끝났을 때 BossSpawner를 실행하도록 이벤트 구독
            storyManager.OnTutorialFinished += OnTutorialFinished;

            // 튜토리얼 시작
            storyManager.StartTutorial(StoryManager.TutorialType.SecondBoss, FirstBossTutorialMessages);

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
