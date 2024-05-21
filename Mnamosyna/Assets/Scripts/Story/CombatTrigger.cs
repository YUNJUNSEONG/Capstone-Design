using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatTrigger : MonoBehaviour
{
    public StoryManager storyManager;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            List<string> combatTutorialMessages = new List<string>
            {
                "곧 몬스터들이 당신을 죽이려 몰려올거예요.",
                "각 구역마다 일정 수의 몬스터가 출몰해요",
                "마우스 좌클릭(L)과 우클릭(R)을 이용해 적을 공격할 수 있어요.\r\n살아 남아서 이 곳을 탈출하죠.",
            };
            storyManager.StartTutorial(StoryManager.TutorialType.Combat, combatTutorialMessages);

            // 오브젝트를 비활성화합니다.
            gameObject.SetActive(false);
        }
    }
}
