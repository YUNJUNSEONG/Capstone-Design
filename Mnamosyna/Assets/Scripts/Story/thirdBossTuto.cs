using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class thirdBossTuto : MonoBehaviour
{
    public StoryManager storyManager;
    public Spawner spawner;

    private void Start()
    {
        if (storyManager == null)
        {
            storyManager = FindObjectOfType<StoryManager>();
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            List<string> FirstBossTutorialMessages = new List<string>
            {
                "",
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
