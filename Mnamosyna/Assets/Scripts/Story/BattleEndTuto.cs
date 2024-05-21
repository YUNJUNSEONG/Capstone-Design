using System.Collections.Generic;
using UnityEngine;

public class BattleEndTuto : MonoBehaviour
{
    public StoryManager storyManager;
    public Spawner spawner;

    void Update()
    {
        if (spawner.isCombatEnded)
        {
            List<string> postCombatTutorialMessages = new List<string>
            {
                "������ �������ϴ�.",
                "�������� ȹ���ϼ���."
            };
            storyManager.StartTutorial(StoryManager.TutorialType.PostCombat, postCombatTutorialMessages);

            // ���� ���� ó�� �� isCombatEnded�� �ٽ� false�� �����Ͽ� �ߺ� ���� ����
            spawner.isCombatEnded = false;

            // ������Ʈ�� ��Ȱ��ȭ�մϴ�.
            gameObject.SetActive(false);
        }
    }
}
