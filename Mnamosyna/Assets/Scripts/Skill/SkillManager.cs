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

        curSkills.Clear();

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
        int numSkillsToShow = Mathf.Min(3, playerCon.StanbySkills.Count); // 실제 UI에 출력할 스킬의 개수를 계산합니다.

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
        // UnlockSkill 리스트의 길이를 가져옵니다.
        int numSkillsToShow = UnlockSkill.Count;

        Debug.Log("UnlockSkill Count: " + UnlockSkill.Count);
        for (int i = 0; i < UnlockSkill.Count; i++)
        {
            Debug.Log("UnlockSkill[" + i + "]: " + UnlockSkill[i]);
        }

        // UnlockSkill 리스트의 길이와 unlockchoices 배열의 길이를 비교하여 작을 때만 해당하는 부분을 실행합니다.
        if (numSkillsToShow < unlockchoices.Length)
        {
            for (int i = 0; i < numSkillsToShow; i++)
            {
                // 유효성 검사를 추가하여 유효하지 않은 인덱스에 대한 처리를 수행합니다.
                if (UnlockSkill[i] < playerCon.StanbySkills.Count)
                {
                    int skillIndex = UnlockSkill[i];
                    SkillData skillData = playerCon.StanbySkills[skillIndex];

                    // UI 요소 설정 코드를 여기에 추가합니다.
                }
                else
                {
                    Debug.LogError("Invalid index in UnlockSkill list: " + UnlockSkill[i]);
                }
            }
        }
        else
        {
            for (int i = 0; i < unlockchoices.Length; i++)
            {
                int skillIndex = UnlockSkill[i]; // 현재 선택된 스킬의 인덱스

                Debug.Log("Selected Skill Index: " + skillIndex); // 선택된 스킬의 인덱스를 로그로 출력

                // 선택된 스킬 데이터 가져오기
                SkillData skillData = playerCon.StanbySkills[skillIndex];
                Debug.Log("Selected Skill Data: " + skillData); // 선택된 스킬 데이터를 로그로 출력
                Debug.Log("========================================================");


                // UI 요소 설정
                // 1. 이미지 설정
                unlockchoices[i].transform.GetChild(0).GetComponent<Image>().sprite = skillData.Image;

                // 2. 스킬 이름과 커맨드 설정 (커맨드가 없는 경우 출력하지 않음)
                string skillNameAndCommand = skillData.Name.ToString();
                if (!string.IsNullOrEmpty(skillData.Command))
                {
                    skillNameAndCommand += " (" + skillData.Command.ToString() + ")";
                }
                unlockchoices[i].transform.GetChild(1).GetComponent<Text>().text = skillNameAndCommand;

                // 3. 스킬의 종류 속성과 출력 (combo / Link / Passive)
                unlockchoices[i].transform.GetChild(2).GetComponent<Text>().text =  skillData.skillType.ToString();

                // 4. 스킬의 설명 출력
                // 설명이 없는 경우 출력하지 않음
                if (!string.IsNullOrEmpty(skillData.Description))
                {
                    unlockchoices[i].transform.GetChild(3).GetComponent<Text>().text = skillData.Description.ToString();
                }
                else
                {
                    unlockchoices[i].transform.GetChild(3).GetComponent<Text>().text = "";
                }

                //unlockchoices[i].transform.GetChild(4).GetComponent<Text>().text = skillData.element.ToString();
            }
        }

        UnlockBase.SetActive(true);
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
        int choice = 0;
        LevelUpSkill = new List<int>();

        // 플레이어가 보유한 스킬의 개수를 가져옵니다.
        int numSkills = playerCon.UnlockSkills.Count;

        // UI에 출력할 스킬의 개수를 설정합니다.
        int numSkillsToShow = Mathf.Min(numSkills, levelUpchoices.Length);

        // 만약 UI에 출력할 스킬의 개수가 0 이상이라면
        if (numSkillsToShow > 0)
        {
            while (choice < numSkillsToShow)
            {
                int rand = UnityEngine.Random.Range(0, numSkills);
                if (!LevelUpSkill.Contains(rand))
                {
                    LevelUpSkill.Add(rand);
                    choice++;
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
        // LevelUpSkill 리스트의 길이를 가져옵니다.
        int numSkillsToShow = LevelUpSkill.Count;

        // LevelUpSkill 리스트의 길이가 levelUpchoices 배열의 길이보다 작다면
        // 실제 UI에 출력할 스킬의 개수를 numSkillsToShow에 맞춰야 합니다.
        if (numSkillsToShow < levelUpchoices.Length)
        {
            // LevelUpSkill 리스트의 길이가 levelUpchoices 배열의 길이보다 작을 때는
            // LevelUpSkill 리스트의 길이만큼만 반복문을 실행합니다.
            for (int i = 0; i < numSkillsToShow; i++)
            {
                // LevelUpSkill 리스트에서 인덱스를 가져와 사용하기 전에 유효성을 확인합니다.
                if (LevelUpSkill[i] < playerCon.UnlockSkills.Count)
                {
                    // 유효한 인덱스라면 UI 요소를 설정합니다.
                    levelUpchoices[i].transform.GetChild(0).GetComponent<Image>().sprite = playerCon.UnlockSkills[LevelUpSkill[i]].Image;
                    levelUpchoices[i].transform.GetChild(1).GetComponent<Text>().text =
                        playerCon.UnlockSkills[LevelUpSkill[i]].Name.ToString() + '(' + playerCon.UnlockSkills[LevelUpSkill[i]].Command.ToString() + ')' + " Lv." + playerCon.UnlockSkills[LevelUpSkill[i]].Level;

                    // 기타 UI 요소 설정 코드도 이어서 작성합니다.
                }
                else
                {
                    // 유효하지 않은 인덱스일 경우에 대한 처리를 여기에 추가할 수 있습니다.
                    Debug.LogError("Invalid index in LevelUpSkill list: " + LevelUpSkill[i]);
                }
            }
        }
        else
        {
            /*
            for (int i = 0; i < levelUpchoices.Length; i++)
            {
                levelUpchoices[i].transform.GetChild(0).GetComponent<Image>().sprite = playerCon.UnlockSkills[LevelUpSkill[i]].Image;

                levelUpchoices[i].transform.GetChild(1).GetComponent<Text>().text =
                    playerCon.UnlockSkills[LevelUpSkill[i]].Name.ToString() + '(' + playerCon.UnlockSkills[LevelUpSkill[i]].Command.ToString() + ')' + " Lv." + playerCon.UnlockSkills[LevelUpSkill[i]].Level;

                if (playerCon.UnlockSkills[LevelUpSkill[i]].Level == 0)
                    levelUpchoices[i].transform.GetChild(2).GetComponent<Text>().text = playerCon.UnlockSkills[LevelUpSkill[i]].Description.ToString();
                else if (playerCon.UnlockSkills[LevelUpSkill[i]].Level < playerCon.UnlockSkills[LevelUpSkill[i]].maxLevel)
                    levelUpchoices[i].transform.GetChild(2).GetComponent<Text>().text = "데미지가 " + (playerCon.UnlockSkills[LevelUpSkill[i]].damagePercent +"%" + playerCon.UnlockSkills[LevelUpSkill[i]].addDmg * playerCon.UnlockSkills[LevelUpSkill[i]].Level) + "에서 " + (playerCon.UnlockSkills[LevelUpSkill[i]].damagePercent + playerCon.UnlockSkills[LevelUpSkill[i]].addDmg * (playerCon.UnlockSkills[LevelUpSkill[i]].Level + 1)) + "로 증가합니다.";
                else
                    levelUpchoices[i].transform.GetChild(2).GetComponent<Text>().text = "이 스킬은 마스터하셨습니다.";
                if (playerCon.UnlockSkills[LevelUpSkill[i]].Level < playerCon.UnlockSkills[LevelUpSkill[i]].maxLevel)
                {
                    levelUpchoices[i].transform.GetChild(3).GetComponentInChildren<Text>().text = "Level Up";
                }
                else
                {
                    levelUpchoices[i].transform.GetChild(3).GetComponentInChildren<Text>().text = "Close";
                }
            }*/
        }

        LevelUpBase.SetActive(true);
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