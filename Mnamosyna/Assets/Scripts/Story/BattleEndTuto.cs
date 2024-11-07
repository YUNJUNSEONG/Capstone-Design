using System.Collections.Generic;
using UnityEngine;

public class BattleEndTuto : MonoBehaviour
{
    public StoryManager storyManager;
    public Spawner spawner;

    void Update()
    {
        if (spawner.IsCombatEnded)
        {
            List<string> postCombatTutorialMessages = new()
            {
                "��� ���͸� óġ�߾��.\r\n���� �̰������� ���Ͱ� ������ �����ſ���.",
                "���� ������!",
                "���� ����� �Ҿ���� ����� �����̿���.\r\n����� �ذ� �ִ� ����� ���ø� �� �����ſ���.",
                "EŰ�� ������ ������ ȸ���ϼ���."
            };
            storyManager.StartTutorial(StoryManager.TutorialType.PostCombat, postCombatTutorialMessages);

            // ���� ���� ó�� �� isCombatEnded�� �ٽ� false�� �����Ͽ� �ߺ� ���� ����
            spawner.IsCombatEnded = false;

            // ������Ʈ�� ��Ȱ��ȭ�մϴ�.
            gameObject.SetActive(false);
        }
    }
}
