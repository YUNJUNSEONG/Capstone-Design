using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class StartTutorial : MonoBehaviour
{
    public Text tutorialText;
    public GameObject tutorialUI;
    private bool tutorialCompleted = false;

    private int StartcurrentStep = 0;

    private string[] tutorialSteps = {
        "드디어 일어났군요. " +
            "여기가 어디고 나는 누구냐고요?",
        "성질도 급하셔라"+
            "여기는 당신처럼 기억을 뺏긴 자들이 오는곳이예요.",
        "제가 당신의 기억을 가져갔나고요?",
        "반은 맞고 반은 틀렸어요."+
            "나는 이곳의 주인이였어요..."+
                "마룡에게 빼앗기기 전까진 말이죠.",
        "당신은 그것에게 기억을 뺏기고 이 던전으로 던져진거예요.",
        "이곳의 몬스터들을 처치하면 기억의 조각은 얻겠지만"+
            "모든 기억을 찾으려면 마룡을 쓰러트려야해요"+
                "저는 힘이 되지 못하겠지만 당신을 지켜보면서 말동무라도 되드릴게요...",
        "WASD키를 이용하면 움직일 수 있을거예요.",
        "그리고 마우스 왼클릭과 우클릭으로 적을 공격 할 수 있을거예요",
    };

    void Start()
    {
        // 게임 시작 시 튜토리얼 완료 상태를 불러옵니다.
       //tutorialCompleted = PlayerPrefs.GetInt("TutorialCompleted", 0) == 1;

        // 튜토리얼이 완료되었으면 튜토리얼 UI를 비활성화합니다.
        if (tutorialCompleted)
        {
            tutorialUI.SetActive(false);
        }
        else
        {
            // 튜토리얼이 완료되지 않았으면 튜토리얼 UI를 활성화합니다.
            tutorialUI.SetActive(true);
            Time.timeScale = 0;
            UpdateTutorialText();
        }
    }

    void Update()
    {
        // 예를 들어, 다음 단계로 진행하려면 엔터 키를 누르는 등의 조건을 추가할 수 있습니다.
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
        {
            // 다음 단계로 진행합니다.
            StartcurrentStep++;
            // 모든 단계를 완료한 경우 튜토리얼을 종료합니다.
            if (StartcurrentStep >= tutorialSteps.Length)
            {
                Debug.Log("Tutorial completed!");
                tutorialUI.SetActive(false);
                Destroy(gameObject);
                Time.timeScale = 1;
                return;
            }
            // 다음 단계의 튜토리얼 메시지를 표시합니다.
            UpdateTutorialText();
        }
    }

    void UpdateTutorialText()
    {
        // 현재 단계의 튜토리얼 메시지를 Text UI 요소에 표시합니다.
        tutorialText.text = tutorialSteps[StartcurrentStep];
    }

}


