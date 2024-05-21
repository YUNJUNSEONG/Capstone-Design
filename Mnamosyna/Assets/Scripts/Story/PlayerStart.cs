using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStart : MonoBehaviour
{
    public StoryManager storyManager;
    public Animator playerAnimator; // �̹� �����ϴ� Animator ������Ʈ�� ����
    public Rigidbody rb;
    public GameObject supportObject; // �÷��̾� �ؿ� ��ġ�� ���� ������Ʈ

    void Start()
    {
        // �÷��̾ �����ִ� �ִϸ��̼��� ����
        playerAnimator.Play("LyingDown");

        // ���� �ð� �Ŀ� �Ͼ�� �ִϸ��̼��� Ʈ����
        StartCoroutine(TriggerStandUpAnimation());
    }

    IEnumerator TriggerStandUpAnimation()
    {
        // �����ִ� �ִϸ��̼� ��� �ð� (2��) ���� ���
        yield return new WaitForSeconds(2f);

        // �Ͼ�� �ִϸ��̼� Ʈ����
        playerAnimator.SetTrigger("StandUpTrigger");


        // �Ͼ�� �ִϸ��̼��� ���� ������ ���
        while (playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("StandUp") &&
               playerAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
        {
            yield return null; // �� ������ ���
        }

        // �Ͼ�� ���� Idle ���·� ��ȯ
        playerAnimator.SetTrigger("idle");

        // Idle ���°� �� ������ ���
        while (!playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("idle"))
        {
            yield return new WaitForSeconds(2f);
            //yield return null; // �� ������ ���
        }
        // ���� ������Ʈ ����
        if (supportObject != null)
        {
            Destroy(supportObject);
        }
        // �ִϸ��̼��� ���� �� Ʃ�丮�� �޽��� ����
        List<string> startTutorialMessages = new List<string>
        {
            "���� �Ͼ�̱���.\r\n���� �ǳ���ó�, ����� �����ֱ� ���� �Ծ��.",
            "����� ���İ��?",
            "����� ���ó�� ����� ���� �ڵ��� ���°��̿���.",
            "����� ������ ���ָ� �޾� ����� �Ҿ�����ſ���.",
            "�׸��� �װͿ��� ����� ����� �� �������� �������ſ���.",
            "����... ����� �������� �� ���� �����̿����...",
            "�� �Ǵ��� ���濡�� ���ѱ�� ������ ������.",
            "�� ���� ���͵��� ������ ������ ����������.",
            "�׷��� ���͵��� óġ�ϸ鼭 ����� ������ ���� �� �����ſ���.",
            "������ ��� ����� ã������ ������ ����Ʈ�����ؿ�.",
            "���� ū ���� ���� ���ϰ����� ����� ����Կ�...",
            "WASDŰ�� �̿��ϸ� ������ �� �����ſ���."
        };
        storyManager.StartTutorial(StoryManager.TutorialType.Start, startTutorialMessages);
    }

    public void StopMovement()
    {
        // �÷��̾� �������� ���߱� ���� isKinematic�� ����
        rb.isKinematic = true;
    }

    public void ResumeMovement()
    {
        // �÷��̾� �������� �ٽ� �����ϱ� ���� isKinematic ����
        rb.isKinematic = false;
    }
}
