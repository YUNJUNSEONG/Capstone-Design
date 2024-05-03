using UnityEngine;
using UnityEngine.UI;

public class AttackTutorial : MonoBehaviour
{
    public Text tutorialText;
    public GameObject tutorialUI;
    public Collider triggerCollider; // �÷��̾�� ��ȣ�ۿ��ϴ� collider

    private bool attackTutorialCompleted = false;

    private int currentStep = 0;

    private string[] attackTutorialSteps = {
        "���콺 ��Ŭ������ �⺻ ������ �� �� �־��.",
        "���콺 ��Ŭ������ Ư�� ������ �� �� �־��."
    };

    void Update()
    {
        //Ʃ�丮���� �Ϸ�Ǿ��� �� UI�� ��Ȱ��ȭ�մϴ�.
        if (attackTutorialCompleted)
        {
            tutorialUI.SetActive(false);
            return;
        }

        // �÷��̾ Ʈ���ſ� ������ �� Ʃ�丮���� �����մϴ�.
        if (!attackTutorialCompleted && GetComponent<Collider>().enabled && currentStep == 0)
        {
            tutorialUI.SetActive(true);
            Time.timeScale = 0;
            UpdateTutorialText();
        }

        // �÷��̾ Ʃ�丮���� �����ϴ� ���� �����̽�Ű�� ������ ���� �ܰ�� �����մϴ�.
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