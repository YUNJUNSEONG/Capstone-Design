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
        // �ʿ��� ��ȭ ��ũ��Ʈ���� ã���ϴ�.
        if (firstDialogueScript == null && secondDialogueScript == null && thirdDialogueScript == null)
        {
            Debug.LogError("��ȭ ��ũ��Ʈ�� ã�� �� �����ϴ�.");
        }
    }

    public string GetRandomDialogue()
    {
        string dialogue = "";

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

        return dialogue;
    }
}

