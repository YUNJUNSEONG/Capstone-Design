using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class StartTutorial : MonoBehaviour
{
    public Text tutorialText;
    public GameObject tutorialUI;
    public Image characterImage;
    private bool tutorialCompleted = false;

    private int StartcurrentStep = 0;

    private string[] tutorialSteps = {
        "드디어 일어나셨군요. " +
            "나는 므나모시나. 당신을 도와주기 위해 왔어요.",
        "여기는 어디냐고요?"+
            "여기는 당신처럼 기억을 뺏긴 자들이 오는곳이예요.",
        "당신은 마룡의 저주를 받아 기억을 잃어버린거예요."+
        "그리고 그것에게 기억을 뺏기고 이 던전으로 던져진거예요.",
        "저는... 기억의 여신이자 이 곳의 주인이였어요..."+
            "제 권능을 마룡에게 빼앗기기 전까진 말이죠.",
        "이 곳의 몬스터들은 마룡의 힘으로 만들어졌어요.",
        "그러니 몬스터들을 처치하면서 기억의 조각은 얻겠을 수 있을거예요.",
            "하지만 모든 기억을 찾으려면 마룡을 쓰러트려야해요"+
                "저는 큰 힘이 되진 못하겠지만 당신을 도울게요...",
        "WASD키를 이용하면 움직일 수 있을거예요.",
        "그리고 마우스 왼클릭과 우클릭으로 적을 공격 할 수 있을거예요"
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

            // 캐릭터 이미지를 활성화합니다.
            characterImage.gameObject.SetActive(true);
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


