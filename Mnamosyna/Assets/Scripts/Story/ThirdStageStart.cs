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

            // ������Ʈ�� ��Ȱ��ȭ�մϴ�.
            gameObject.SetActive(false);
        }
    }
}
