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
                "�� �������� ���� ���� ���Ͱ� ����ؿ�.\r\n��� ���͸� óġ�ؾ� ���� �������� �Ѿ �� �־��.",
                "�� ���͵��� ����� ���̷� �����ðſ���.",
                "���콺 ��Ŭ��(L)�� ��Ŭ��(R)�� �̿��� ���� �����ؿ�.\r\n��� ���Ƽ� �� ���� Ż������.",
            };
            storyManager.StartTutorial(StoryManager.TutorialType.Combat, combatTutorialMessages);

            // ������Ʈ�� ��Ȱ��ȭ�մϴ�.
            gameObject.SetActive(false);
        }
    }
}
