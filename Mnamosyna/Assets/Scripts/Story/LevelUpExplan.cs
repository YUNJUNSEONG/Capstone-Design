using System.Collections.Generic;
using UnityEngine;

public class LevelUpExplan : MonoBehaviour
{
    public StoryManager storyManager;
    public List<Spawner> spawners;

    private bool tutorialShown = false; // �ߺ� ���� ������ ���� �÷���

    void Update()
    {
        if (tutorialShown)
            return;

        foreach (Spawner spawner in spawners)
        {
            if (spawner.IsCombatEnded)
            {
                List<string> LevelUpTutorialMessages = new()
                {
                    "���� ������!",
                    "���� ��� ����� �����̿���.\r\n����� ��ã�� ������� �� ���ϰ� ���ٰſ���.",
                    "EŰ�� ������ ������ ȸ���ϼ���."
                };
                storyManager.StartTutorial(StoryManager.TutorialType.SkillLevelUp, LevelUpTutorialMessages);

                // Ʃ�丮���� �ѹ� ����Ǿ����� ǥ��
                tutorialShown = true;

                // ��� �������� isCombatEnded�� �ٽ� false�� �����Ͽ� �ߺ� ���� ����
                foreach (Spawner sp in spawners)
                {
                    sp.IsCombatEnded = false;
                }

                // ������Ʈ�� ��Ȱ��ȭ�մϴ�.
                gameObject.SetActive(false);

                // �ϳ��� �����ʿ����� ����ǵ��� ���� ����
                break;
            }
        }
    }
}

