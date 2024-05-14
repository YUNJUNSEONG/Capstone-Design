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
        // �ʿ��� ��ȭ ��ũ��Ʈ���� ã���ϴ�.
        if (firstDialogueScript == null && secondDialogueScript == null && thirdDialogueScript == null)
        {
            Debug.LogError("��ȭ ��ũ��Ʈ�� ã�� �� �����ϴ�.");
        }
    }

    public void DisplayRandomDialogue()
    {
        if (dialogueText == null)
        {
            Debug.LogError("��ȭ �ؽ�Ʈ�� ã�� �� �����ϴ�.");
            return;
        }

        string dialogue = "";
        ShowUI();

        // �ش��ϴ� ���������� ���� ��ȭ ��ũ��Ʈ�� ����մϴ�.
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
            Debug.LogWarning("��ȭ ��ũ��Ʈ�� ã�� �� �����ϴ�.");
        }

        dialogueText.text = dialogue;
        StartHideTimer();
    }

    public void ShowUI()
    {
        if (canvas != null)
        {
            canvas.SetActive(true); // Canvas�� Ȱ��ȭ�Ͽ� UI�� ���̰� �մϴ�.
        }
    }

    public void HideUI()
    {
        if (canvas != null)
        {
            canvas.SetActive(false); // Canvas�� ��Ȱ��ȭ�Ͽ� UI�� ����ϴ�.
        }
    }

    public void StartHideTimer()
    {
        // ���� �ð��� ���� �Ŀ� HideUI �޼��带 ȣ���Ͽ� UI�� ����ϴ�.
        Invoke("HideUI", 5.0f);
    }
}


