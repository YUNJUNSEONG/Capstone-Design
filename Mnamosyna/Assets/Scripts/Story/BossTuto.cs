using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTuto : MonoBehaviour
{
    public StoryManager storyManager;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            List<string> FirstBossTutorialMessages = new List<string>
            {
                "����� �� ������ ��Ű�� ����� ���ϰ� ���� �ſ���.",
                "���� ������.\r\n���� �� ������ ������ �巡���̿���.",
                "��溸�ٴ� ���ϴ��� ��ü�� �巡���� �ſ� ���ؿ�.",
                "���ݱ��� ����ؿԴ� ���������� ������ �ٸ��ſ���.",
                "�׷��� �����ؼ� �������."
            };
            storyManager.StartTutorial(StoryManager.TutorialType.FirstBoss, FirstBossTutorialMessages);

            // ������Ʈ�� ��Ȱ��ȭ�մϴ�.
            gameObject.SetActive(false);
        }
    }
}
