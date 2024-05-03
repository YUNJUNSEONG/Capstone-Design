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
        "Welcome to the tutorial! Press SPACE to jump.",
        "Great! Now use the arrow keys to move around.",
        "You can interact with objects by pressing E.",
        "That's it! You've completed the tutorial!",
        "That's it! You've completed the tutorial!",
        "당신이 이 곳을 빠져 나갈때까지 도와드릴게요.",
        "WASD키를 이용하여 움직일 수 있어요.",
        "마우스 왼클릭으로는 기본 공격을, 마우스 우클릭으로 특수 공격을 할 수 있어요",
    };

    void Start()
    {
        // 게임 시작 시 튜토리얼 완료 상태를 불러옵니다.
       tutorialCompleted = PlayerPrefs.GetInt("TutorialCompleted", 0) == 1;

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


