using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class SkillManager : MonoBehaviour
{
    public static SkillManager instance;
    public GameObject player;
    private Player playerCon;

    public GameObject[] levelUpchoices = new GameObject[3];
    public GameObject[] unlockchoices = new GameObject[3];

    public GameObject LevelUpBase;
    public List<int> LevelUpSkill;

    public GameObject UnlockBase;
    public List<int> UnlockSkill;


    public List<SkillData> curSkills; // 현재 보유한 스킬
                                      //public Image DashUI;

    private void Awake()
    {
        if (instance == null)
            instance = this;

        curSkills = new List<SkillData>(); // curSkills 초기화 추가

        UnlockSkill = new List<int>(); // UnlockSkill 초기화
        LevelUpSkill = new List<int>(); // LevelUpSkill 초기화
    }


    private void Start()
    {
        playerCon = player.GetComponent<Player>();
    }

    public void Unlock(Action onUnlockComplete = null)
    {
        Time.timeScale = 0;
        UnlockSkill = new List<int>();

        // 실제 UI에 출력할 스킬의 개수를 계산합니다.
        int numSkillsToShow = Mathf.Min(3, playerCon.StanbySkills.Count);

        // UnlockSkill 리스트에 스킬을 추가합니다.
        for (int i = 0; i < numSkillsToShow; i++)
        {
            int rand = UnityEngine.Random.Range(0, playerCon.StanbySkills.Count);
            if (!UnlockSkill.Contains(rand))
            {
                UnlockSkill.Add(rand);
            }
        }

        SetUnlockUI();

        // 콜백 함수를 호출합니다.
        onUnlockComplete?.Invoke();
    }

    void SetUnlockUI()
    {
        UnlockBase.SetActive(true); // UnlockBase 활성화

        // UnlockSkill 리스트의 길이를 가져옵니다.
        int numSkillsToShow = Mathf.Min(UnlockSkill.Count, unlockchoices.Length);

        // UnlockSkill 리스트의 길이만큼 반복하여 UI 설정
        for (int i = 0; i < numSkillsToShow; i++)
        {
            int skillIndex = UnlockSkill[i];
            SkillData skillData = playerCon.StanbySkills[skillIndex];

            // UI 요소 설정
            unlockchoices[i].SetActive(true); // 해당 스킬 UI 활성화
            unlockchoices[i].transform.GetChild(0).GetComponent<Image>().sprite = skillData.Image; // 이미지 설정
            string skillNameAndCommand = skillData.Name.ToString();
            if (!string.IsNullOrEmpty(skillData.Command))
            {
                skillNameAndCommand += " (" + skillData.Command.ToString() + ")";
            }
            unlockchoices[i].transform.GetChild(1).GetComponent<Text>().text = skillNameAndCommand;

            unlockchoices[i].transform.GetChild(2).GetComponent<Text>().text = skillData.skillType.ToString(); // 스킬 종류 속성 설정
            unlockchoices[i].transform.GetChild(3).GetComponent<Text>().text = skillData.Description; // 스킬 설명 설정
        }

        // 남은 UI 비활성화
        for (int i = numSkillsToShow; i < unlockchoices.Length; i++)
        {
            unlockchoices[i].SetActive(false);
        }
    }



    public void choiceUnlockSkill(int num)
    {
        // num이 유효한지 확인
        if (num < 0 || num >= UnlockSkill.Count)
        {
            Debug.LogError("Invalid index for choiceUnlockSkill: " + num);
            return;
        }

        // 선택된 스킬의 인덱스
        int selectedSkillIndex = UnlockSkill[num];
        SkillData skillData = playerCon.StanbySkills[selectedSkillIndex];
        //출력된 후 스킬들의 인덱스 재확인
        Debug.Log("========================================================");
        Debug.Log("Selected Skill Data: " + skillData);
        Debug.Log("Selected skill index: " + selectedSkillIndex);

        // 선택된 스킬의 레벨을 증가시킴
        if (playerCon.StanbySkills[selectedSkillIndex].Level == 0)
            playerCon.StanbySkills[selectedSkillIndex].Level++;

        // 자식 스킬 추가
        for (int i = 0; i < playerCon.StanbySkills[selectedSkillIndex].childSkill.Length; i++)
        {
            if (!playerCon.StanbySkills.Contains(playerCon.StanbySkills[selectedSkillIndex].childSkill[i]))
            {
                playerCon.StanbySkills.Add(playerCon.StanbySkills[selectedSkillIndex].childSkill[i]);
            }
        }

        // 선택된 스킬을 curSkills에 추가
        if (!curSkills.Contains(playerCon.StanbySkills[selectedSkillIndex]))
            curSkills.Add(playerCon.StanbySkills[selectedSkillIndex]);

        // UI 갱신
        UnlockBase.SetActive(false);
        curSkills.Sort(compareUISkills);
        playerCon.StanbySkills.Sort(comparePlayerSkills);
        playerCon.UnlockSkills = curSkills;
        ClearSameCommand();

        // 시간 재개
        Time.timeScale = 1;
    }

    public void LevelUp()
    {
        Time.timeScale = 0;
        LevelUpSkill = new List<int>();

        // 플레이어가 보유한 스킬의 개수를 가져옵니다.
        int numSkills = playerCon.UnlockSkills.Count;

        // UI에 출력할 스킬의 개수를 설정합니다.
        int numSkillsToShow = Mathf.Min(numSkills, 3);

        // 만약 UI에 출력할 스킬의 개수가 0 이상이라면
        if (numSkillsToShow > 0)
        {
            while (LevelUpSkill.Count < numSkillsToShow)
            {
                int rand = UnityEngine.Random.Range(0, numSkills);
                if (!LevelUpSkill.Contains(rand))
                {
                    LevelUpSkill.Add(rand);
                }
            }

            SetLeveUpUI();
        }
        else
        {
            // UI에 출력할 스킬이 없을 경우에 대한 처리를 여기에 추가할 수 있습니다.
            Debug.LogWarning("There are no skills to level up.");
        }
    }


    void SetLeveUpUI()
    {
        LevelUpBase.SetActive(true); // LevelUpBase 활성화

        // LevelUpSkill 리스트의 길이를 가져옵니다.
        int numSkillsToShow = Mathf.Min(LevelUpSkill.Count, levelUpchoices.Length);

        // LevelUpSkill 리스트의 길이만큼 반복하여 UI 설정
        for (int i = 0; i < numSkillsToShow; i++)
        {
            int skillIndex = LevelUpSkill[i];
            SkillData skillData = playerCon.UnlockSkills[skillIndex];

            // UI 요소 설정
            levelUpchoices[i].SetActive(true); // 해당 스킬 UI 활성화
            levelUpchoices[i].transform.GetChild(0).GetComponent<Image>().sprite = skillData.Image; // 이미지 설정
            string skillNameAndCommand = skillData.Name.ToString();
            if (!string.IsNullOrEmpty(skillData.Command))
            {
                skillNameAndCommand += " (" + skillData.Command.ToString() + ")";
            }
            levelUpchoices[i].transform.GetChild(1).GetComponent<Text>().text = skillNameAndCommand;
            levelUpchoices[i].transform.GetChild(3).GetComponent<Text>().text = skillData.Description; // 스킬 설명 설정
        }

        // 남은 UI 비활성화
        for (int i = numSkillsToShow; i < levelUpchoices.Length; i++)
        {
            levelUpchoices[i].SetActive(false);
        }
    }



    public void choiceLevelUpSkill(int num)
    {
        if (playerCon.UnlockSkills[LevelUpSkill[num]].Level < playerCon.UnlockSkills[LevelUpSkill[num]].maxLevel)
            playerCon.UnlockSkills[LevelUpSkill[num]].Level++;


        if (!curSkills.Contains(playerCon.UnlockSkills[LevelUpSkill[num]]))
            curSkills.Add(playerCon.UnlockSkills[LevelUpSkill[num]]);

        LevelUpBase.SetActive(false);
        curSkills.Sort(compareUISkills);
        playerCon.UnlockSkills.Sort(comparePlayerSkills);

        Time.timeScale = 1;
    }


    int compareUISkills(SkillData a, SkillData b)
    {
        return a.Id < b.Id ? -1 : 1;
    }

    int comparePlayerSkills(SkillData a, SkillData b)
    {
        if ((a.isUnlock && b.isUnlock) || (!a.isUnlock && !b.isUnlock))
            return a.Id < b.Id ? -1 : 1;
        else if (a.isUnlock)
            return -1;
        else
            return 1;
    }

    void ClearSameCommand()
    {
        // StanbySkills와 UnlockSkills의 Command 및 이름을 추출하여 저장할 리스트 생성
        List<string> stanbySkillsCommands = new List<string>();
        List<string> stanbySkillsNames = new List<string>();
        List<string> unlockSkillsCommands = new List<string>();
        List<string> unlockSkillsNames = new List<string>();

        // StanbySkills와 UnlockSkills에서 Command 및 이름을 추출하여 리스트에 추가
        foreach (var skill in playerCon.StanbySkills)
        {
            if (!string.IsNullOrEmpty(skill.Command))
                stanbySkillsCommands.Add(skill.Command);
            else
                stanbySkillsNames.Add(skill.name);
        }

        foreach (var skill in playerCon.UnlockSkills)
        {
            if (!string.IsNullOrEmpty(skill.Command))
                unlockSkillsCommands.Add(skill.Command);
            else
                unlockSkillsNames.Add(skill.name);
        }

        // 삭제할 스킬 오브젝트들을 담을 리스트 생성
        List<SkillData> skillsToRemove = new List<SkillData>();

        // StanbySkills에서 확인한 Command와 같은 Command를 가지는 스킬 오브젝트들을 삭제할 리스트에 추가
        foreach (var skill in playerCon.StanbySkills)
        {
            if (unlockSkillsCommands.Contains(skill.Command))
                skillsToRemove.Add(skill);
        }

        // UnlockSkills에 있는 스킬 오브젝트의 이름과 같은 이름을 가지는 StanbySkills의 스킬 오브젝트들을 삭제할 리스트에 추가
        foreach (var skill in playerCon.StanbySkills)
        {
            if (unlockSkillsNames.Contains(skill.name))
                skillsToRemove.Add(skill);
        }

        // 스킬 오브젝트들을 한 번에 삭제
        foreach (var skill in skillsToRemove)
        {
            playerCon.StanbySkills.Remove(skill);
        }
    }


}
 
