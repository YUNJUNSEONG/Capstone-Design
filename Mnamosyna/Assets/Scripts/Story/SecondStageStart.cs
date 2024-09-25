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
                "�� ���� �̷��� Ȳ�������ٴ�...",
                "����� ���ſ� �̷��� ���㰡 �ƴϿ����.",
                "�ʸ��� ��â�� �︲�̿���.",
                "���� ������ ���̿� �̷��� ���ϴٴ�...",
            };
            storyManager.StartTutorial(StoryManager.TutorialType.Combat, combatTutorialMessages);

            // ������Ʈ�� ��Ȱ��ȭ�մϴ�.
            gameObject.SetActive(false);
        }
    }
}
