using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstBattleEnd : MonoBehaviour
{
    public StoryManager storyManager;
    public Spawner spawner;

    void Update()
    {
        if (spawner.IsCombatEnded)
        {
            List<string> potalExplanTutorialMessages = new()
            {
                "���⼭ ���ʹ� �������̿���.",
                "Ǫ�� ��Ż �ʸӿ����� ����� �Ҿ���� ������ ã�� �� �����ſ���.",
                "��� ��Ż �ʸӷδ� ����� ã�� ����� �������� ���ϰ� ���� �� �����ſ���.",
                "�� �����ϸ鼭 ������ ���ư���."
            };
            storyManager.StartTutorial(StoryManager.TutorialType.Potal, potalExplanTutorialMessages);

            // ���� ���� ó�� �� isCombatEnded�� �ٽ� false�� �����Ͽ� �ߺ� ���� ����
            spawner.IsCombatEnded = false;

            // ������Ʈ�� ��Ȱ��ȭ�մϴ�.
            gameObject.SetActive(false);
        }
    }
}
