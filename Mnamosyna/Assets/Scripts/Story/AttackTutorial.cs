using UnityEngine;
using UnityEngine.UI;

public class AttackTutorial : MonoBehaviour
{
    public Text tutorialText;
    public GameObject tutorialUI;
    public Transform triggerLocation; // Ʃ�丮���� ���۵Ǵ� ��ġ
    private bool tutorialStarted = false; // Ʃ�丮�� ���� ���θ� ��Ÿ���� ����
    private bool tutorialCompleted = false; // Ʃ�丮�� �Ϸ� ���θ� ��Ÿ���� ����
    private int currentStep = 0;

    private string[] tutorialSteps = {
        "���콺 ��Ŭ������ �⺻ ������ �� �� �־��.",
        "���콺 ��Ŭ������ Ư�� ������ �� �� �־��."
    };

    void Update()
    {
        if (tutorialCompleted)
            return;

        // Ʃ�丮�� ���� ����: Ʃ�丮���� ���۵��� �ʾҰ�, �÷��̾ Ʈ���� ��ġ�� ������ ���
        if (!tutorialStarted && Vector3.Distance(transform.position, triggerLocation.position) < 2f)
        {
            StartTutorial();
        }

        // Ʃ�丮�� ���� �߿� �����̽�Ű�� ������ ���� �ܰ�� ����
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
