using UnityEngine;
using UnityEngine.UI;

public class RandomTalk : MonoBehaviour
{
    public firstStageDialogue firstDialogueScript;
    public secondStageDialogue secondDialogueScript;
    public thirdStageDialogue thirdDialogueScript;

    public Text Dialogue;

    void Start()
    {
        // 필요한 대화 스크립트만을 찾습니다.
        if (firstDialogueScript == null && secondDialogueScript == null && thirdDialogueScript == null)
        {
            Debug.LogError("대화 스크립트를 찾을 수 없습니다.");
        }
    }

    public string GetRandomDialogue()
    {
        string dialogue = "";

        // 해당하는 스테이지에 따라 대화 스크립트를 사용합니다.
        if (firstDialogueScript != null)
        {
            dialogue = firstDialogueScript.GetRandomDialogue();
        }
        else if (secondDialogueScript != null)
        {
            dialogue = secondDialogueScript.GetRandomDialogue();
        }
        else if (thirdDialogueScript != null)
        {
            dialogue = thirdDialogueScript.GetRandomDialogue();
        }
        else
        {
            Debug.LogWarning("대화 스크립트를 찾을 수 없습니다.");
        }

        return dialogue;
    }
}

