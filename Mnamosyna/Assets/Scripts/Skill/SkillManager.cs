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

    public GameObject[] choices = new GameObject[3];

    public List<int> LevelUpSkill;
    public GameObject LevelUpBase;

    public List<int> UnlockSkill;
    public GameObject UnlockBase;


    public List<SkillData> skillUI;
    public Image DashUI;


    private void Awake()
    {
        if (instance == null)
            instance = this;

        skillUI.Clear();
    }

    private void Start()
    {
        playerCon = player.GetComponent<Player>();
    }

    public void LevelUp()
    {
        Time.timeScale = 0;
        playerCon.isAttack = true;
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
        for (int i = 0; i < choices.Length; i++)
        {
            choices[i].transform.GetChild(0).GetComponent<Image>().sprite = playerCon.UnlockSkills[LevelUpSkill[i]].Image;

            choices[i].transform.GetChild(1).GetComponent<Text>().text =
                playerCon.UnlockSkills[LevelUpSkill[i]].Name.ToString() + '(' + playerCon.UnlockSkills[LevelUpSkill[i]].Command.ToString() + ')' + " Lv." + playerCon.UnlockSkills[LevelUpSkill[i]].Level;

            if (playerCon.UnlockSkills[LevelUpSkill[i]].Level == 0)
                choices[i].transform.GetChild(2).GetComponent<Text>().text = playerCon.UnlockSkills[LevelUpSkill[i]].Description.ToString();
            else if (playerCon.UnlockSkills[LevelUpSkill[i]].Level < playerCon.UnlockSkills[LevelUpSkill[i]].maxLevel)
                choices[i].transform.GetChild(2).GetComponent<Text>().text = "데미지가 " + (playerCon.UnlockSkills[LevelUpSkill[i]].damagePercent +'%' + playerCon.UnlockSkills[LevelUpSkill[i]].addDmg * playerCon.UnlockSkills[LevelUpSkill[i]].Level) + "에서 " + (playerCon.UnlockSkills[LevelUpSkill[i]].damagePercent + playerCon.UnlockSkills[LevelUpSkill[i]].addDmg * (playerCon.UnlockSkills[LevelUpSkill[i]].Level + 1)) + "로 증가합니다.";
            else
                choices[i].transform.GetChild(2).GetComponent<Text>().text = "이 스킬은 마스터하셨습니다.";

            if (playerCon.UnlockSkills[LevelUpSkill[i]].Level < playerCon.UnlockSkills[LevelUpSkill[i]].maxLevel)
            {
                choices[i].transform.GetChild(3).GetComponentInChildren<Text>().text = "Level Up";
            }
            else
            {
                choices[i].transform.GetChild(3).GetComponentInChildren<Text>().text = "Close";
            }
        }

        LevelUpBase.SetActive(true);
    }

    public void Unlock()
    {
        Time.timeScale = 0;
        playerCon.isAttack = true;
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
        for (int i = 0; i < choices.Length; i++)
        {
            choices[i].transform.GetChild(0).GetComponent<Image>().sprite = playerCon.UnlockSkills[LevelUpSkill[i]].Image;

            choices[i].transform.GetChild(1).GetComponent<Text>().text =
                playerCon.UnlockSkills[LevelUpSkill[i]].Name.ToString() + '(' + playerCon.UnlockSkills[LevelUpSkill[i]].Command.ToString() + ')' + " Lv." + playerCon.UnlockSkills[LevelUpSkill[i]].Level;

            if (playerCon.UnlockSkills[LevelUpSkill[i]].Level == 0)
                choices[i].transform.GetChild(2).GetComponent<Text>().text = playerCon.UnlockSkills[LevelUpSkill[i]].Description.ToString();
            else if (playerCon.UnlockSkills[LevelUpSkill[i]].Level < playerCon.UnlockSkills[LevelUpSkill[i]].maxLevel)
                choices[i].transform.GetChild(2).GetComponent<Text>().text = "데미지가 " + (playerCon.UnlockSkills[LevelUpSkill[i]].damagePercent +'%' + playerCon.UnlockSkills[LevelUpSkill[i]].addDmg * playerCon.UnlockSkills[LevelUpSkill[i]].Level) + "에서 " + (playerCon.UnlockSkills[LevelUpSkill[i]].damagePercent + playerCon.UnlockSkills[LevelUpSkill[i]].addDmg * (playerCon.UnlockSkills[LevelUpSkill[i]].Level + 1)) + "로 증가합니다.";
            else
                choices[i].transform.GetChild(2).GetComponent<Text>().text = "이 스킬은 마스터하셨습니다.";

            if (playerCon.UnlockSkills[LevelUpSkill[i]].Level < playerCon.UnlockSkills[LevelUpSkill[i]].maxLevel)
            {
                choices[i].transform.GetChild(3).GetComponentInChildren<Text>().text = "Level Up";
            }
            else
            {
                choices[i].transform.GetChild(3).GetComponentInChildren<Text>().text = "Close";
            }
        }

        LevelUpBase.SetActive(true);
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


}

