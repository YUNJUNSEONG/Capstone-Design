using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerStart : MonoBehaviour
{
    public StoryManager storyManager;
    public Animator playerAnimator;
    public Rigidbody rb;
    //public GameObject supportObject;

    void Start()
    {
        InitializePlayer();
    }
    public void RestartGameInitialization()
    {
        InitializePlayer(); // ���� �ٽ� ���� �� ȣ��Ǵ� �޼���
    }

    void InitializePlayer()
    {
        //playerAnimator.Play("LyingDown");

        StartCoroutine(TriggerStandUpAnimation());
    }


    IEnumerator TriggerStandUpAnimation()
    {
        
        // �����ִ� �ִϸ��̼� ��� �ð� (2��) ���� ���
        yield return new WaitForSeconds(0.3f);
        /*
        // �Ͼ�� �ִϸ��̼� Ʈ����
        playerAnimator.SetTrigger("StandUpTrigger");

        // �Ͼ�� �ִϸ��̼��� ���� ������ ���
        while (playerAnimator.GetCurrentAnimatorStateInfo(1).IsName("StandUp") &&
               playerAnimator.GetCurrentAnimatorStateInfo(1).normalizedTime < 1.0f)
        {
            yield return null; // �� ������ ���
        }

        // �Ͼ�� ���� Idle ���·� ��ȯ
        playerAnimator.SetTrigger("idle");

        // Idle ���°� �� ������ ���
        while (!playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("idle"))
        {
            yield return null; // �� ������ ���
        }
        
        // ���� ������Ʈ ����
        if (supportObject != null)
        {
            Destroy(supportObject);
        }
        */

        // �ִϸ��̼��� ���� �� Ʃ�丮�� �޽��� ����
        List<string> startTutorialMessages = new List<string>
        {
            "�� �̰��� ���ο� ����� �Ա���....\r\n���� �ǳ���ó�, ����� �����ֱ� ���� �Ծ��.",
            "����� ���İ��?",
            "����� ���ó�� ����� ���� �ڵ��� ���°��̿���.\r\n����� ������ ���ָ� �޾� ����� ���Ѱ���.",
            "�׸��� �� �������� �������ſ���.",
            "����... ����� �������� �� ���� �����̿����...\r\n�� �Ǵ��� ���濡�� ���ѱ�� ������ ������.",
            "���� ���������� �ʹٸ� ���� ����� �����ٰԿ�.",
            "�� ���� ���͵��� ������ ������ ����������.\r\n�׷��� ���͵��� óġ�ϸ鼭 ����� ���ѱ� ����� ������ ���� �� �����ſ���.",
            "������ ��� ����� ã������ ������ ����Ʈ���߸��ؿ�.",
            "���� ū ���� ���� ���ϰ����� ����� ����Կ�...",
            "�켱 WASDŰ�� �̿��ϸ� ������ �� �����ſ���."
        };
        storyManager.StartTutorial(StoryManager.TutorialType.Start, startTutorialMessages);
    }

    public void StopMovement()
    {
        rb.isKinematic = true;
    }

    public void ResumeMovement()
    {
        rb.isKinematic = false;
    }
}
