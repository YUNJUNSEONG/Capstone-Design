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
        dialogues.Add("��κ��� ���͵��� ����� ���̷� �޷���ſ���.");
        dialogues.Add("�ȱ׷� ���Ͱ� �ֳİ��? ... �Ƹ� �����ɿ�.");
        dialogues.Add("�۰� �Ϳ��� ����ó�� ��������... ������״� �ٸ� �ſ���.");
        dialogues.Add("�����ӵ��� ����Ʈ���� �п��ϴ� �����ϼ���.");
        dialogues.Add("�� ���� ���������� ���� ���� ������ ������ �ƴϿ���. �� ���� ���ϼ���.");
        dialogues.Add("�����Ǵ����� ���մ� ���� �����ؿ�");
        dialogues.Add("�Ź̵��� �����ϼ���. ���İ��� ���̹����ſ���");
        dialogues.Add("�׻� �ڽ��� ü�¿� �����ϼ���.");
        dialogues.Add("����� ������ ���⼼��. �����ʵ��� �����ϼ���");
        dialogues.Add("");
        // ���� ���������� ���� �߰�...
    }

    public string GetRandomDialogue()
    {
        if (dialogues.Count == 0)
        {
            Debug.LogWarning(".........");
            return "";
        }

        int randomIndex = Random.Range(0, dialogues.Count);
        string dialogue = dialogues[randomIndex];
        dialogues.RemoveAt(randomIndex); // ����� ��ȭ�� ����Ʈ���� �����մϴ�.
        return dialogue;
    }
}
