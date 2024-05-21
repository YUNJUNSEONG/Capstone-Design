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
                "�� ���͵��� ����� ���̷� �����ðſ���.",
                "�� �������� ���� ���� ���Ͱ� ����ؿ�",
                "���콺 ��Ŭ��(L)�� ��Ŭ��(R)�� �̿��� ���� ������ �� �־��.\r\n��� ���Ƽ� �� ���� Ż������.",
            };
            storyManager.StartTutorial(StoryManager.TutorialType.Combat, combatTutorialMessages);

            // ������Ʈ�� ��Ȱ��ȭ�մϴ�.
            gameObject.SetActive(false);
        }
    }
}
