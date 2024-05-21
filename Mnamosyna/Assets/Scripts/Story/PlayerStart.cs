using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStart : MonoBehaviour
{
    public StoryManager storyManager;
    public Animator playerAnimator; // 이미 존재하는 Animator 컴포넌트를 참조
    public Rigidbody rb;
    public GameObject supportObject; // 플레이어 밑에 배치된 지지 오브젝트

    void Start()
    {
        // 플레이어가 누워있는 애니메이션을 시작
        playerAnimator.Play("LyingDown");

        // 일정 시간 후에 일어나는 애니메이션을 트리거
        StartCoroutine(TriggerStandUpAnimation());
    }

    IEnumerator TriggerStandUpAnimation()
    {
        // 누워있는 애니메이션 재생 시간 (2초) 동안 대기
        yield return new WaitForSeconds(2f);

        // 일어나는 애니메이션 트리거
        playerAnimator.SetTrigger("StandUpTrigger");


        // 일어나는 애니메이션이 끝날 때까지 대기
        while (playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("StandUp") &&
               playerAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
        {
            yield return null; // 한 프레임 대기
        }

        // 일어나고 나서 Idle 상태로 전환
        playerAnimator.SetTrigger("idle");

        // Idle 상태가 될 때까지 대기
        while (!playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("idle"))
        {
            yield return new WaitForSeconds(2f);
            //yield return null; // 한 프레임 대기
        }
        // 지지 오브젝트 제거
        if (supportObject != null)
        {
            Destroy(supportObject);
        }
        // 애니메이션이 끝난 후 튜토리얼 메시지 시작
        List<string> startTutorialMessages = new List<string>
        {
            "드디어 일어나셨군요.\r\n나는 므나모시나, 당신을 도와주기 위해 왔어요.",
            "여기는 어디냐고요?",
            "여기는 당신처럼 기억을 뺏긴 자들이 오는곳이예요.",
            "당신은 마룡의 저주를 받아 기억을 잃어버린거예요.",
            "그리고 그것에게 기억을 뺏기고 이 던전으로 던져진거예요.",
            "저는... 기억의 여신이자 이 곳의 주인이였어요...",
            "제 권능을 마룡에게 빼앗기기 전까진 말이죠.",
            "이 곳의 몬스터들은 마룡의 힘으로 만들어졌어요.",
            "그러니 몬스터들을 처치하면서 기억의 조각은 얻을 수 있을거예요.",
            "하지만 모든 기억을 찾으려면 마룡을 쓰러트려야해요.",
            "저는 큰 힘이 되진 못하겠지만 당신을 도울게요...",
            "WASD키를 이용하면 움직일 수 있을거예요."
        };
        storyManager.StartTutorial(StoryManager.TutorialType.Start, startTutorialMessages);
    }

    public void StopMovement()
    {
        // 플레이어 움직임을 멈추기 위해 isKinematic을 설정
        rb.isKinematic = true;
    }

    public void ResumeMovement()
    {
        // 플레이어 움직임을 다시 시작하기 위해 isKinematic 해제
        rb.isKinematic = false;
    }
}
