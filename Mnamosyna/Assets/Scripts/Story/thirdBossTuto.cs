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

            // Ʃ�丮���� ������ �� BossSpawner�� �����ϵ��� �̺�Ʈ ����
            storyManager.OnTutorialFinished += OnTutorialFinished;

            // Ʃ�丮�� ����
            storyManager.StartTutorial(StoryManager.TutorialType.SecondBoss, FirstBossTutorialMessages);

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
