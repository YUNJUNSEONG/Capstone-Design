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
        InitializePlayer(); // 게임 다시 시작 시 호출되는 메서드
    }

    void InitializePlayer()
    {
        //playerAnimator.Play("LyingDown");

        StartCoroutine(TriggerStandUpAnimation());
    }


    IEnumerator TriggerStandUpAnimation()
    {
        
        // 누워있는 애니메이션 재생 시간 (2초) 동안 대기
        yield return new WaitForSeconds(0.3f);
        /*
        // 일어나는 애니메이션 트리거
        playerAnimator.SetTrigger("StandUpTrigger");

        // 일어나는 애니메이션이 끝날 때까지 대기
        while (playerAnimator.GetCurrentAnimatorStateInfo(1).IsName("StandUp") &&
               playerAnimator.GetCurrentAnimatorStateInfo(1).normalizedTime < 1.0f)
        {
            yield return null; // 한 프레임 대기
        }

        // 일어나고 나서 Idle 상태로 전환
        playerAnimator.SetTrigger("idle");

        // Idle 상태가 될 때까지 대기
        while (!playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("idle"))
        {
            yield return null; // 한 프레임 대기
        }
        
        // 지지 오브젝트 제거
        if (supportObject != null)
        {
            Destroy(supportObject);
        }
        */

        // 애니메이션이 끝난 후 튜토리얼 메시지 시작
        List<string> startTutorialMessages = new List<string>
        {
            "또 이곳에 새로운 사람이 왔군요....\r\n나는 므나모시나, 당신을 도와주기 위해 왔어요.",
            "여기는 어디냐고요?",
            "여기는 당신처럼 기억을 뺏긴 자들이 오는곳이예요.\r\n당신은 마룡의 저주를 받아 기억을 빼앗겼어요.",
            "그리고 이 던전으로 던져진거예요.",
            "저는... 기억의 여신이자 이 곳의 주인이였어요...\r\n제 권능을 마룡에게 빼앗기기 전까진 말이죠.",
            "여길 빠져나가고 싶다면 제가 당신을 도와줄게요.",
            "이 곳의 몬스터들은 마룡의 힘으로 만들어졌어요.\r\n그러니 몬스터들을 처치하면서 당신이 빼앗긴 기억의 조각은 얻을 수 있을거예요.",
            "하지만 모든 기억을 찾으려면 마룡을 쓰러트려야만해요.",
            "저는 큰 힘이 되진 못하겠지만 당신을 도울게요...",
            "우선 WASD키를 이용하면 움직일 수 있을거예요."
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
