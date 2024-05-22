using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockTuto : MonoBehaviour
{
    public StoryManager storyManager;
    public GameObject unlock;
    public UnlockSkill unlockSkill;

    private bool tutorialCompleted = false;

    void Start()
    {
        // ���� ���� �� Ʃ�丮�� �Ϸ� ���¸� �ҷ��ɴϴ�.
        //tutorialCompleted = PlayerPrefs.GetInt("TutorialCompleted", 0) == 1;

        if (tutorialCompleted)
        {
            gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (unlockSkill.UnlockSkillSelectEnd)
        {
            List<string> skillUnlockTutorialMessages = new List<string>
            {
                "��� ���͸� óġ�߾��.\r\n���� �̰������� ���Ͱ� ������ �����ſ���.",
                "���� ������!",
                "���� ����� �Ҿ���� ����� �����̿���.\r\n����� �ذ� �ִ� ����� ���ø� �� �����ſ���.",
                "EŰ�� ������ ������ ȸ���ϼ���."
            };
            storyManager.StartTutorial(StoryManager.TutorialType.SkillUnlock, skillUnlockTutorialMessages);

            // ���� ���� ó�� �� UnlockSkillSelectEnd�� �ٽ� false�� �����Ͽ� �ߺ� ���� ����
            unlockSkill.UnlockSkillSelectEnd = false;

            // ������Ʈ�� ��Ȱ��ȭ�մϴ�.
            gameObject.SetActive(false);
        }
    }
}
