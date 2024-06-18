using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTuto : MonoBehaviour
{
    public StoryManager storyManager;
    public SkillManager skillManager;

    void Update()
    {
        if (skillManager.IsSkillUnlocked())
        {
            List<string> UnlockExplanTutorialMessages = new List<string>
            {
                "����� �����ϴ� ����� ��￡ ���� \r\n���Ⱑ �ٲ�� ���� �� �ִ� ������� �޶����ſ���.",
                "����� ù��° ����� �����ΰ���? \r\n� ���⸦ �������?",
                "������ ����� ��ã���鼭 ���� ������� \r\n���ݰ� ȸ�Ǹ� �����Ͽ� ����� �� �ִ� �޺� ���,",
                "�����θ� ��ȭ �����ִ� �нú긦 ���� �� ���� �ſ���.",
                "�ڽſ��� �ʿ��Ѱ� �������� �����ϸ鼭 \r\n��, ������ ���ư���.",
            };
            storyManager.StartTutorial(StoryManager.TutorialType.SkillUnlock, UnlockExplanTutorialMessages);


            // ������Ʈ�� ��Ȱ��ȭ�մϴ�.
            gameObject.SetActive(false);
        }
    }
}
