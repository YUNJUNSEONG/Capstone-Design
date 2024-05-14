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
        // 각 스테이지에 맞는 대화를 추가합니다.
        dialogues.Add("대부분의 몬스터들은 당신을 죽이려 달려들거예요.");
        dialogues.Add("안그런 몬스터가 있냐고요? ... 아마 없을걸요.");
        dialogues.Add("작고 귀여운 버섯처럼 보이지만... 당신한테는 다를 거예요.");
        dialogues.Add("슬라임들은 쓰러트리면 분열하니 조심하세요.");
        dialogues.Add("저 작은 해츨링들이 뱉어내는 불은 무시할 정도가 아니예요. 잘 보고 피하세요.");
        dialogues.Add("샐러맨더들이 내뿜는 불을 조심해요");
        dialogues.Add("거미들을 조심하세요. 순식간에 들이박을거예요");
        dialogues.Add("항상 자신의 체력에 유의하세요.");
        dialogues.Add("목숨을 소중히 여기세요. 죽지않도록 조심하세요");
        dialogues.Add("");
        // 이하 스테이지에 따라 추가...
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
        dialogues.RemoveAt(randomIndex); // 사용한 대화를 리스트에서 삭제합니다.
        return dialogue;
    }
}
