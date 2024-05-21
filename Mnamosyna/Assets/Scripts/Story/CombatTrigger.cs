using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatTrigger : MonoBehaviour
{
    public StoryManager storyManager;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            List<string> combatTutorialMessages = new List<string>
            {
                "전투가 시작됩니다.",
                "적을 공격하세요."
            };
            storyManager.StartTutorial(StoryManager.TutorialType.Combat, combatTutorialMessages);

            // 오브젝트를 비활성화합니다.
            gameObject.SetActive(false);
        }
    }
}
