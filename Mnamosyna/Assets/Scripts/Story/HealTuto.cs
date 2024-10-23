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
                "이 곳은 안전한 곳이예요.\r\n여기서 잠시 쉬어가죠.",
                "저길 보세요.\r\n저건 제가 이 던전의 주인이었을 적 있던 성수예요.",
                "사악한 힘에 의해 색이 탁해진것 같지만, 그래도 아직 제 힘이 남아있어요.",
                "마시면 떨어진 체력을 회복 할 수 있을거예요.",
                "그리고 제 힘이 좀 더 남아있다면 당신의 기억도 찾을지도 몰라요."
            };
            storyManager.StartTutorial(StoryManager.TutorialType.Healing, HealTutorialMessages);

            // 오브젝트를 비활성화합니다.
            gameObject.SetActive(false);
        }
    }
}
