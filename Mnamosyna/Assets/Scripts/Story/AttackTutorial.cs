using UnityEngine;
using UnityEngine.UI;

public class AttackTutorial : MonoBehaviour
{
    public Text tutorialText;
    public GameObject tutorialUI;
    public Collider triggerCollider; // 플레이어와 상호작용하는 collider

    private bool attackTutorialCompleted = false;

    private int currentStep = 0;

    private string[] attackTutorialSteps = {
        "마우스 왼클릭으로 기본 공격을 할 수 있어요.",
        "마우스 우클릭으로 특수 공격을 할 수 있어요."
    };

    void Update()
    {
        //튜토리얼이 완료되었을 때 UI를 비활성화합니다.
        if (attackTutorialCompleted)
        {
            tutorialUI.SetActive(false);
            return;
        }

        // 플레이어가 트리거에 들어왔을 때 튜토리얼을 시작합니다.
        if (!attackTutorialCompleted && GetComponent<Collider>().enabled && currentStep == 0)
        {
            tutorialUI.SetActive(true);
            Time.timeScale = 0;
            UpdateTutorialText();
        }

        // 플레이어가 튜토리얼을 진행하는 동안 스페이스키를 누르면 다음 단계로 진행합니다.
        if (!attackTutorialCompleted && (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space)))
        {
            currentStep++;

            if (currentStep >= attackTutorialSteps.Length)
            {
                attackTutorialCompleted = true;
                PlayerPrefs.SetInt("AttackTutorialCompleted", 1);
                tutorialUI.SetActive(false);
                Time.timeScale = 1;
                Debug.Log("Tutorial completed!");
                Destroy(gameObject);
                return;
            }

            UpdateTutorialText();
        }
    }

    void UpdateTutorialText()
    {
        tutorialText.text = attackTutorialSteps[currentStep];
    }
}