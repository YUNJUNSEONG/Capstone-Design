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
        dialogues.Add("냄새나고 역겨운 어인들....");
        dialogues.Add("오크들은 추하고 둔하지만 강한 공격을 해요.");
        dialogues.Add("웜들은 두려움이 많아 공격하고 바로 숨어버려요.");
        dialogues.Add("더러운 반짐승 놈들...");
        dialogues.Add("이곳은 너무 역겨운 냄새가 나요.");
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
