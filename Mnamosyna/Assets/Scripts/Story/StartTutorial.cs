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
        "���� �Ͼ�̱���. " +
            "���� �ǳ���ó�. ����� �����ֱ� ���� �Ծ��.",
        "����� ���İ��?"+
            "����� ���ó�� ����� ���� �ڵ��� ���°��̿���.",
        "����� ������ ���ָ� �޾� ����� �Ҿ�����ſ���."+
        "�׸��� �װͿ��� ����� ����� �� �������� �������ſ���.",
        "����... ����� �������� �� ���� �����̿����..."+
            "�� �Ǵ��� ���濡�� ���ѱ�� ������ ������.",
        "�� ���� ���͵��� ������ ������ ����������.",
        "�׷��� ���͵��� óġ�ϸ鼭 ����� ������ ����� �� �����ſ���.",
            "������ ��� ����� ã������ ������ ����Ʈ�����ؿ�"+
                "���� ū ���� ���� ���ϰ����� ����� ����Կ�...",
        "WASDŰ�� �̿��ϸ� ������ �� �����ſ���.",
        "�׸��� ���콺 ��Ŭ���� ��Ŭ������ ���� ���� �� �� �����ſ���"
    };

    void Start()
    {
        // ���� ���� �� Ʃ�丮�� �Ϸ� ���¸� �ҷ��ɴϴ�.
        //tutorialCompleted = PlayerPrefs.GetInt("TutorialCompleted", 0) == 1;

        // Ʃ�丮���� �Ϸ�Ǿ����� Ʃ�丮�� UI�� ��Ȱ��ȭ�մϴ�.
        if (tutorialCompleted)
        {
            tutorialUI.SetActive(false);
        }
        else
        {
            // Ʃ�丮���� �Ϸ���� �ʾ����� Ʃ�丮�� UI�� Ȱ��ȭ�մϴ�.
            tutorialUI.SetActive(true);
            Time.timeScale = 0;
            UpdateTutorialText();

            // ĳ���� �̹����� Ȱ��ȭ�մϴ�.
            characterImage.gameObject.SetActive(true);
        }
    }

    void Update()
    {
        // ���� ���, ���� �ܰ�� �����Ϸ��� ���� Ű�� ������ ���� ������ �߰��� �� �ֽ��ϴ�.
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
        {
            // ���� �ܰ�� �����մϴ�.
            StartcurrentStep++;
            // ��� �ܰ踦 �Ϸ��� ��� Ʃ�丮���� �����մϴ�.
            if (StartcurrentStep >= tutorialSteps.Length)
            {
                Debug.Log("Tutorial completed!");
                tutorialUI.SetActive(false);
                Destroy(gameObject);
                Time.timeScale = 1;
                return;
            }
            // ���� �ܰ��� Ʃ�丮�� �޽����� ǥ���մϴ�.
            UpdateTutorialText();
        }
    }

    void UpdateTutorialText()
    {
        // ���� �ܰ��� Ʃ�丮�� �޽����� Text UI ��ҿ� ǥ���մϴ�.
        tutorialText.text = tutorialSteps[StartcurrentStep];
    }

}


