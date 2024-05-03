using UnityEngine;
using System.Collections.Generic;

public class firstStageDialogue : MonoBehaviour
{
    private List<string> dialogues = new List<string>();

    void Start()
    { 
        InitializeDialogues();
    }

    void InitializeDialogues()
    {
        // �� ���������� �´� ��ȭ�� �߰��մϴ�.
        dialogues.Add("ù ��° �������� ��� 1");
        dialogues.Add("ù ��° �������� ��� 2");
        dialogues.Add("ù ��° �������� ��� 3");
        dialogues.Add("ù ��° �������� ��� 4");
        dialogues.Add("ù ��° �������� ��� 5");
        dialogues.Add("ù ��° �������� ��� 6");
        dialogues.Add("ù ��° �������� ��� 7");
        // ���� ���������� ���� �߰�...
    }

    public string GetRandomDialogue()
    {
        if (dialogues.Count == 0)
        {
            Debug.LogWarning("��ȭ�� �����ϴ�.");
            return "";
        }

        int randomIndex = Random.Range(0, dialogues.Count);
        string dialogue = dialogues[randomIndex];
        dialogues.RemoveAt(randomIndex); // ����� ��ȭ�� ����Ʈ���� �����մϴ�.
        return dialogue;
    }
}
