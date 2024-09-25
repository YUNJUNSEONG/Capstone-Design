using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondStageStart : MonoBehaviour
{
    public StoryManager storyManager;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            List<string> combatTutorialMessages = new List<string>
            {
                "이 곳이 이렇게 황폐해지다니...",
                "여기는 과거에 이렇게 폐허가 아니였어요.",
                "초목이 울창한 삼림이였죠.",
                "제가 없어진 사이에 이렇게 변하다니...",
            };
            storyManager.StartTutorial(StoryManager.TutorialType.Combat, combatTutorialMessages);

            // 오브젝트를 비활성화합니다.
            gameObject.SetActive(false);
        }
    }
}
