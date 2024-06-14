using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockSkill : MonoBehaviour
{
    public GameObject UnlockUI;
    public StoryManager storyManager;
    private SkillManager skillManager; // 충돌한 오브젝트에서 스킬 매니저 컴포넌트 가져오기
    public GameObject Trigger;

    public bool UnlockSkillSelectEnd = false;
    private static bool storyShown = false;

    private void Start()
    {
        skillManager = FindObjectOfType<SkillManager>(); // scene에서 SkillManager 오브젝트를 찾아 할당
        if (skillManager == null)
        {
            Debug.LogError("SkillManager를 찾을 수 없습니다.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>(); // 충돌한 오브젝트에서 플레이어 컴포넌트 가져오기
            if (player != null)
            {
                // 스킬 언락 UI 열기 및 스킬 언락 메서드 호출
                OpenUnlockUpUI(skillManager);
            }
        }
    }

    public void OpenUnlockUpUI(SkillManager skillManager)
    {
        // 스킬 매니저의 Unlock 메서드를 호출합니다.
        skillManager.Unlock();

        Invoke("selectEnd", 1.0f);

        Destroy(gameObject);
    }

    void selectEnd()
    {
        if(skillManager.EndUnlockSkillChoice == true)
        {
            UnlockSkillSelectEnd = true;
        }
    }
}
