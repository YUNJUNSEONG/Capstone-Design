using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTuto : MonoBehaviour
{
    public StoryManager storyManager;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            List<string> FirstBossTutorialMessages = new List<string>
            {
                "여기는 이 구역을 지키는 사룡의 수하가 있을 거예요.",
                "저길 보세요.\r\n저건 이 구역의 주인인 드래곤이예요.",
                "사룡보다는 약하더라도 성체인 드래곤은 매우 강해요.",
                "지금까지 상대해왔던 해츨링과는 차원이 다른거예요.",
                "그러니 조심해서 상대하죠."
            };
            storyManager.StartTutorial(StoryManager.TutorialType.FirstBoss, FirstBossTutorialMessages);

            // 오브젝트를 비활성화합니다.
            gameObject.SetActive(false);
        }
    }
}
