using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class secondStageDialogue : MonoBehaviour
{
    private List<string> dialogues = new List<string>();

    void Start()
    {
        // ��ȭ�� �ʱ�ȭ�մϴ�.
        InitializeDialogues();
    }

    void InitializeDialogues()
    {
        // �� ���������� �´� ��ȭ�� �߰��մϴ�.
        dialogues.Add("�������� ���ܿ� ���ε�....");
        dialogues.Add("��ũ���� ���ϰ� �������� ���� ������ �ؿ�.");
        dialogues.Add("������ �η����� ���� �����ϰ� �ٷ� ���������.");
        dialogues.Add("������ ������ ���...");
        dialogues.Add("�̰��� �ʹ� ���ܿ� ������ ����.");
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
