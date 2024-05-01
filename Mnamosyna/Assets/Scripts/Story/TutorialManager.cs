using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public Text tutorialText;
    private int currentStep = 0;

    private string[] tutorialSteps = {
        "Welcome to the tutorial! Press SPACE to jump.",
        "Great! Now use the arrow keys to move around.",
        "You can interact with objects by pressing E.",
        "That's it! You've completed the tutorial!"
    };

    void Start()
    {
        // 시작할 때 첫 번째 단계의 튜토리얼 메시지를 표시합니다.
        UpdateTutorialText();
    }

    void Update()
    {
        // 예를 들어, 다음 단계로 진행하려면 스페이스바를 누르는 등의 조건을 추가할 수 있습니다.
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // 다음 단계로 진행합니다.
            currentStep++;
            // 모든 단계를 완료한 경우 튜토리얼을 종료합니다.
            if (currentStep >= tutorialSteps.Length)
            {
                Debug.Log("Tutorial completed!");
                return;
            }
            // 다음 단계의 튜토리얼 메시지를 표시합니다.
            UpdateTutorialText();
        }
    }

    void UpdateTutorialText()
    {
        // 현재 단계의 튜토리얼 메시지를 Text UI 요소에 표시합니다.
        tutorialText.text = tutorialSteps[currentStep];
    }
}

