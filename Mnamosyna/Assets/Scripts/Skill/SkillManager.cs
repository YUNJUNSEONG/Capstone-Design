using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
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

    public void Unlock()
    {
        Time.timeScale = 0;
        UnlockSkill = new List<int>();
        while (UnlockSkill.Count < 3)
        {
            int rand = UnityEngine.Random.Range(0, playerCon.StanbySkills.Count);
            if (!UnlockSkill.Contains(rand))
            {
                UnlockSkill.Add(rand);
            }
        }

        SetUnlockUI();
    }


    void SetUnlockUI()
    {
        for (int i = 0; i < unlockchoices.Length; i++)
        {
            unlockchoices[i].transform.GetChild(0).GetComponent<Image>().sprite = playerCon.StanbySkills[UnlockSkill[i]].Image;

            unlockchoices[i].transform.GetChild(1).GetComponent<Text>().text =
                playerCon.StanbySkills[UnlockSkill[i]].Name.ToString() + '(' + playerCon.StanbySkills[UnlockSkill[i]].Command.ToString() + ')'  + playerCon.StanbySkills[UnlockSkill[i]].Level;

            if (playerCon.StanbySkills[UnlockSkill[i]].Level == 0)
                unlockchoices[i].transform.GetChild(2).GetComponent<Text>().text = playerCon.StanbySkills[UnlockSkill[i]].Description.ToString();
            else if (playerCon.UnlockSkills[UnlockSkill[i]].Level < playerCon.StanbySkills[UnlockSkill[i]].maxLevel)
                unlockchoices[i].transform.GetChild(2).GetComponent<Text>().text = "데미지가 " + (playerCon.StanbySkills[UnlockSkill[i]].damagePercent +"%" + playerCon.StanbySkills[UnlockSkill[i]].addDmg * playerCon.StanbySkills[UnlockSkill[i]].Level) + "에서 " + (playerCon.StanbySkills[UnlockSkill[i]].damagePercent + playerCon.StanbySkills[UnlockSkill[i]].addDmg * (playerCon.StanbySkills[UnlockSkill[i]].Level + 1)) + "로 증가합니다.";
            else
                unlockchoices[i].transform.GetChild(2).GetComponent<Text>().text = "이 스킬은 마스터하셨습니다.";
             
            if (playerCon.StanbySkills[UnlockSkill[i]].Level < playerCon.StanbySkills[UnlockSkill[i]].maxLevel)
            {
                unlockchoices[i].transform.GetChild(3).GetComponentInChildren<Text>().text = "Unlock";
            }
        }

        UnlockBase.SetActive(true);

    }

    public void choiceUnlockSkill(int num)
    {

        if (playerCon.StanbySkills[UnlockSkill[num]].Level == 0)
            playerCon.StanbySkills[UnlockSkill[num]].Level++;


        for (int i = 0; i < playerCon.StanbySkills[UnlockSkill[num]].childSkill.Length; i++)
        {
            if (!playerCon.StanbySkills.Contains(playerCon.StanbySkills[UnlockSkill[num]].childSkill[i]))
            {
                playerCon.StanbySkills.Add(playerCon.StanbySkills[UnlockSkill[num]].childSkill[i]);
            }

        }

        if (!curSkills.Contains(playerCon.StanbySkills[UnlockSkill[num]]))
            curSkills.Add(playerCon.StanbySkills[UnlockSkill[num]]);

        UnlockBase.SetActive(false);
        curSkills.Sort(compareUISkills);
        playerCon.StanbySkills.Sort(comparePlayerSkills);
        playerCon.UnlockSkills = curSkills;
        //ClearSameCommand();

        Time.timeScale = 1;
    }

    public void LevelUp()
    {
        Time.timeScale = 0;
        int choice = 0;
        LevelUpSkill = new List<int>();
        while (choice < Mathf.Min(3, playerCon.UnlockSkills.Count))
        {
            int rand = UnityEngine.Random.Range(0, playerCon.UnlockSkills.Count);
            if (LevelUpSkill.Contains(rand))
            {
                continue;
            }
            else
            {
                LevelUpSkill.Add(rand);
                choice++;
            }
        }

        SetLeveUpUI();
    }

    void SetLeveUpUI()
    {
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
        // 각 커맨드에 해당하는 스킬 목록을 저장하는 리스트
        Dictionary<string, List<SkillData>> commandsMap = new Dictionary<string, List<SkillData>>();

        // 중복된 이름을 가진 패시브 스킬 제거를 위한 HashSet 및 리스트
        HashSet<string> passiveSkillNames = new HashSet<string>();
        List<SkillData> passiveSkillsToRemove = new List<SkillData>();

        foreach (SkillData skillData in playerCon.StanbySkills)
        {
            if (string.IsNullOrEmpty(skillData.Command))
            {
                // 커맨드가 없는 경우 패시브 스킬로 판단하고 중복된 이름을 가진 패시브를 삭제
                if (passiveSkillNames.Contains(skillData.Name))
                {
                    passiveSkillsToRemove.Add(skillData);
                }
                else
                {
                    passiveSkillNames.Add(skillData.Name);
                }
            }
            else
            {
                // 커맨드를 키로 하고 해당 커맨드에 해당하는 스킬을 값으로 하는 딕셔너리를 생성
                if (!commandsMap.ContainsKey(skillData.Command))
                {
                    commandsMap.Add(skillData.Command, new List<SkillData>());
                }
                commandsMap[skillData.Command].Add(skillData);
            }
        }

        // 패시브 스킬 제거
        foreach (SkillData skillToRemove in passiveSkillsToRemove)
        {
            playerCon.StanbySkills.Remove(skillToRemove);
        }

        // unlock으로 선택된 스킬과 그와 같은 커맨드를 가진 스킬들을 모두 제거
        foreach (var kvp in commandsMap)
        {
            string command = kvp.Key;
            List<SkillData> skillsWithSameCommand = kvp.Value;

            // unlock으로 선택된 스킬과 그와 같은 커맨드를 가진 스킬들을 제거
            if (skillsWithSameCommand.Exists(skill => UnlockSkill.Contains(skill.Id)))
            {
                foreach (SkillData skillData in skillsWithSameCommand)
                {
                    if (!UnlockSkill.Contains(skillData.Id))
                    {
                        playerCon.StanbySkills.Remove(skillData);
                    }
                }
            }
        }
        // 딕셔너리의 내용을 출력
        foreach (var kvp in commandsMap)
        {
            string command = kvp.Key;
            List<SkillData> skillsWithSameCommand = kvp.Value;
            Debug.Log("Command: " + command + ", Skills Count: " + skillsWithSameCommand.Count);
        }
    }

}

