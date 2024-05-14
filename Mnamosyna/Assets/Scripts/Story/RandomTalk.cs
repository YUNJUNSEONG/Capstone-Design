using UnityEngine;
using UnityEngine.UI;

public class RandomTalk : MonoBehaviour
{
    public Text dialogueText;
    public GameObject canvas;

    public firstStageDialogue firstDialogueScript;
    public secondStageDialogue secondDialogueScript;
    public thirdStageDialogue thirdDialogueScript;

    void Start()
    {
        // 필요한 대화 스크립트만을 찾습니다.
        if (firstDialogueScript == null && secondDialogueScript == null && thirdDialogueScript == null)
        {
            Debug.LogError("대화 스크립트를 찾을 수 없습니다.");
        }
    }

    public void DisplayRandomDialogue()
    {
        if (dialogueText == null)
        {
            Debug.LogError("대화 텍스트를 찾을 수 없습니다.");
            return;
        }

        string dialogue = "";
        ShowUI();

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

        dialogueText.text = dialogue;
        StartHideTimer();
    }

    public void ShowUI()
    {
        if (canvas != null)
        {
            canvas.SetActive(true); // Canvas를 활성화하여 UI를 보이게 합니다.
        }
    }

    public void HideUI()
    {
        if (canvas != null)
        {
            canvas.SetActive(false); // Canvas를 비활성화하여 UI를 숨깁니다.
        }
    }

    public void StartHideTimer()
    {
        // 일정 시간이 지난 후에 HideUI 메서드를 호출하여 UI를 숨깁니다.
        Invoke("HideUI", 5.0f);
    }
}


