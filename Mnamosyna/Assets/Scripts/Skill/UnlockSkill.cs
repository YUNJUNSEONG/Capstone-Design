using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockSkill : MonoBehaviour
{
    public GameObject UnlockUI;
    public StoryManager storyManager;
    private SkillManager skillManager;
    public GameObject Trigger;

    public bool UnlockSkillSelectEnd = false;
    private static bool storyShown = false;

    // SkillSelectionUI를 연결
    public SkillSelectionUI skillSelectionUI;

    private void Start()
    {
        skillManager = FindObjectOfType<SkillManager>();
        if (skillManager == null)
        {
            Debug.LogError("SkillManager를 찾을 수 없습니다.");
        }

        // 스킬 선택 UI가 할당되지 않았다면 찾음
        if (skillSelectionUI == null)
        {
            skillSelectionUI = FindObjectOfType<SkillSelectionUI>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                // 스킬 언락 UI 열기 및 스킬 언락 메서드 호출
                OpenUnlockUpUI(skillManager);
            }
        }
    }

    public void OpenUnlockUpUI(SkillManager skillManager)
    {
        // 스킬 선택 UI가 열리면 공격을 막음
        //skillSelectionUI.OpenSkillSelection();

        // 스킬 매니저의 Unlock 메서드를 호출합니다.
        skillManager.Unlock();

        Invoke("SelectEnd", 1.0f); // 스킬 선택이 끝나는 로직
        Destroy(gameObject);
    }

    void SelectEnd()
    {
        if (skillManager.EndUnlockSkillChoice == true)
        {
            UnlockSkillSelectEnd = true;

            // 스킬 선택이 끝나면 공격 허용
            //skillSelectionUI.CloseSkillSelection();
        }
    }
}
