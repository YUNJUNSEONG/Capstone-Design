using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealTuto : MonoBehaviour
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
            List<string> HealTutorialMessages = new List<string>
            {
                "�� ���� ������ ���̿���.\r\n���⼭ ��� �����.",
                "���� ������.\r\n���� ���� �� ������ �����̾��� �� �ִ� ��������.",
                "����� ���� ���� ���� Ź������ ������, �׷��� ���� �� ���� �����־��.",
                "���ø� ������ ü���� ȸ�� �� �� �����ſ���.",
                "�׸��� �� ���� �� �� �����ִٸ� ����� ��ﵵ ã������ �����."
            };
            storyManager.StartTutorial(StoryManager.TutorialType.Healing, HealTutorialMessages);

            // ������Ʈ�� ��Ȱ��ȭ�մϴ�.
            gameObject.SetActive(false);
        }
    }
}
