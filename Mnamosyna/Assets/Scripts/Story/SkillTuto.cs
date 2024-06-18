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
                "당신이 선택하는 기술의 기억에 따라 \r\n무기가 바뀌고 얻을 수 있는 기술들이 달라질거예요.",
                "당신의 첫번째 기억은 무엇인가요? \r\n어떤 무기를 사용했죠?",
                "앞으로 기억을 되찾으면서 얻은 기술들은 \r\n공격과 회피를 조합하여 사용할 수 있는 콤보 기술,",
                "스스로를 강화 시켜주는 패시브를 얻을 수 있을 거예요.",
                "자신에게 필요한게 무엇일지 선택하면서 \r\n자, 앞으로 나아가죠.",
            };
            storyManager.StartTutorial(StoryManager.TutorialType.SkillUnlock, UnlockExplanTutorialMessages);


            // 오브젝트를 비활성화합니다.
            gameObject.SetActive(false);
        }
    }
}
