using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class firstBossTuto : MonoBehaviour
{
    public StoryManager storyManager;
    public BossSpawner spawner;

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

            // Ʃ�丮���� ������ �� BossSpawner�� �����ϵ��� �̺�Ʈ ����
            storyManager.OnTutorialFinished += OnTutorialFinished;

            // Ʃ�丮�� ����
            storyManager.StartTutorial(StoryManager.TutorialType.FirstBoss, FirstBossTutorialMessages);

            // ������Ʈ�� ��Ȱ��ȭ�մϴ�.
            gameObject.SetActive(false);
        }
    }

    private void OnTutorialFinished()
    {
        // Ʃ�丮���� ������ �����ʸ� Ȱ��ȭ�մϴ�.
        spawner.SpawnWaves();

        // �̺�Ʈ ���� ����
        storyManager.OnTutorialFinished -= OnTutorialFinished;
    }
}
