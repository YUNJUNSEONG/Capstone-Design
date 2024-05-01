using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class secondStageDialogue : MonoBehaviour
{
    private List<string> dialogues = new List<string>();

    void Start()
    {
        // 대화를 초기화합니다.
        InitializeDialogues();
    }

    void InitializeDialogues()
    {
        // 각 스테이지에 맞는 대화를 추가합니다.
        dialogues.Add("첫 번째 스테이지 대사 1");
        dialogues.Add("첫 번째 스테이지 대사 2");
        dialogues.Add("첫 번째 스테이지 대사 3");
        // 이하 스테이지에 따라 추가...
    }

    public string GetRandomDialogue()
    {
        if (dialogues.Count == 0)
        {
            Debug.LogWarning("대화가 없습니다.");
            return "";
        }

        int randomIndex = Random.Range(0, dialogues.Count);
        string dialogue = dialogues[randomIndex];
        dialogues.RemoveAt(randomIndex); // 사용한 대화를 리스트에서 삭제합니다.
        return dialogue;
    }
}
