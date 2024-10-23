using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdStageStart : MonoBehaviour
{
    public StoryManager storyManager;
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
            List<string> combatTutorialMessages = new List<string>
            {
                "",
            };
            storyManager.StartTutorial(StoryManager.TutorialType.Combat, combatTutorialMessages);

            // 오브젝트를 비활성화합니다.
            gameObject.SetActive(false);
        }
    }
}
