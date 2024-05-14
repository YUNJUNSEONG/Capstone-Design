using UnityEngine;
using UnityEngine.UI;

public class AttackTutorial : MonoBehaviour
{
    public Text tutorialText;
    public GameObject tutorialUI;
    public Transform triggerLocation; // 튜토리얼이 시작되는 위치
    private bool tutorialStarted = false; // 튜토리얼 시작 여부를 나타내는 변수
    private bool tutorialCompleted = false; // 튜토리얼 완료 여부를 나타내는 변수
    private int currentStep = 0;

    private string[] tutorialSteps = {
        "마우스 왼클릭으로 기본 공격을 할 수 있어요.",
        "마우스 우클릭으로 특수 공격을 할 수 있어요."
    };

    void Update()
    {
        if (tutorialCompleted)
            return;

        // 튜토리얼 시작 조건: 튜토리얼이 시작되지 않았고, 플레이어가 트리거 위치에 도달한 경우
        if (!tutorialStarted && Vector3.Distance(transform.position, triggerLocation.position) < 2f)
        {
            StartTutorial();
        }

        // 튜토리얼 진행 중에 스페이스키를 누르면 다음 단계로 진행
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
        {
            NextStep();
        }
    }

    void StartTutorial()
    {
        tutorialStarted = true;
        tutorialUI.SetActive(true);
        Time.timeScale = 0;
        UpdateTutorialText();
    }

    void NextStep()
    {
        currentStep++;

        if (currentStep >= tutorialSteps.Length)
        {
            FinishTutorial();
        }
        else
        {
            UpdateTutorialText();
        }
    }

    void FinishTutorial()
    {
        tutorialCompleted = true;
        tutorialUI.SetActive(false);
        Time.timeScale = 1;
        Debug.Log("Tutorial completed!");
        Destroy(gameObject);
    }

    void UpdateTutorialText()
    {
        tutorialText.text = tutorialSteps[currentStep];
    }
}
