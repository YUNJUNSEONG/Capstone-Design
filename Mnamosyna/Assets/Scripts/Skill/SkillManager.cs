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
    }

    private void Start()
    {
        playerCon = player.GetComponent<Player>();
    }

    public void Unlock()
    {
        Time.timeScale = 0;
        playerCon.SkillCammand = "";
        int choice = 0;
        UnlockSkill = new List<int>();
        while (choice < 3)
        {
            int rand = UnityEngine.Random.Range(0, playerCon.UnlockSkills.Count);
            if (UnlockSkill.Contains(rand))
            {
                continue;
            }
            else
            {
                UnlockSkill.Add(rand);
                choice++;
            }
        }

        SetUnlockUI();
    }

    void SetUnlockUI()
    {
        for (int i = 0; i < unlockchoices.Length; i++)
        {
            unlockchoices[i].transform.GetChild(0).GetComponent<Image>().sprite = playerCon.UnlockSkills[UnlockSkill[i]].Image;

            unlockchoices[i].transform.GetChild(1).GetComponent<Text>().text =
                playerCon.UnlockSkills[UnlockSkill[i]].Name.ToString() + '(' + playerCon.UnlockSkills[UnlockSkill[i]].Command.ToString() + ')'  + playerCon.UnlockSkills[UnlockSkill[i]].Level;

            if(playerCon.UnlockSkills[UnlockSkill[i]].Level == 0)
                unlockchoices[i].transform.GetChild(2).GetComponent<Text>().text = playerCon.UnlockSkills[UnlockSkill[i]].Description.ToString();


            if (playerCon.UnlockSkills[UnlockSkill[i]].Level < playerCon.UnlockSkills[UnlockSkill[i]].maxLevel)
            {
                unlockchoices[i].transform.GetChild(3).GetComponentInChildren<Text>().text = "Level Up";
            }
            else
            {
                unlockchoices[i].transform.GetChild(3).GetComponentInChildren<Text>().text = "Close";
            }
        }

        UnlockBase.SetActive(true);

    }

    public void choiceUnlockSkill(int num)
    {

        if (playerCon.UnlockSkills[LevelUpSkill[num]].Level < 1)
            playerCon.UnlockSkills[LevelUpSkill[num]].Level++;

        for (int i = 0; i < playerCon.UnlockSkills[UnlockSkill[num]].childSkill.Length; i++)
        {
            if (!playerCon.UnlockSkills.Contains(playerCon.UnlockSkills[UnlockSkill[num]].childSkill[i]))
            {
                playerCon.UnlockSkills.Add(playerCon.UnlockSkills[UnlockSkill[num]].childSkill[i]);
            }

        }

        if (!curSkills.Contains(playerCon.UnlockSkills[UnlockSkill[num]]))
            curSkills.Add(playerCon.UnlockSkills[UnlockSkill[num]]);

        UnlockBase.SetActive(false);
        curSkills.Sort(compareUISkills);
        playerCon.UnlockSkills.Sort(comparePlayerSkills);

        Time.timeScale = 1;
    }

    public void LevelUp()
    {
        Time.timeScale = 0;
        playerCon.SkillCammand = "";
        int choice = 0;
        LevelUpSkill = new List<int>();
        while (choice < 3)
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
                levelUpchoices[i].transform.GetChild(2).GetComponent<Text>().text = "데미지가 " + (playerCon.UnlockSkills[LevelUpSkill[i]].damagePercent +'%' + playerCon.UnlockSkills[LevelUpSkill[i]].addDmg * playerCon.UnlockSkills[LevelUpSkill[i]].Level) + "에서 " + (playerCon.UnlockSkills[LevelUpSkill[i]].damagePercent + playerCon.UnlockSkills[LevelUpSkill[i]].addDmg * (playerCon.UnlockSkills[LevelUpSkill[i]].Level + 1)) + "로 증가합니다.";
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
        // 중복된 커맨드를 확인하기 위한 딕셔너리
        Dictionary<string, SkillData> command = new Dictionary<string, SkillData>();

        foreach (SkillData skillData in playerCon.UnlockSkills)
        {
            // 이미 같은 커맨드를 가진 스킬이 있는지 확인합니다.
            if (!command.ContainsKey(skillData.Command))
            {
                // 중복된 커맨드가 없으면 현재 스킬을 딕셔너리에 추가합니다.
                command.Add(skillData.Command, skillData);
            }
            else
            {
                // 중복된 커맨드가 있으면 해당 스킬을 제거합니다.
                playerCon.UnlockSkills.Remove(skillData);
            }
        }
    }

}

