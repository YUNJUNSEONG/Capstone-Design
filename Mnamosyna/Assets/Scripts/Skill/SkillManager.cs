using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SkillManager : MonoBehaviour
{
    public static SkillManager instance;
    public GameObject player;
    private Player playerCon;

    public GameObject[] levelUpChoices = new GameObject[3];
    public GameObject[] unlockChoices = new GameObject[3];

    public GameObject LevelUpBase;
    public List<int> LevelUpSkill;

    public GameObject UnlockBase;
    public List<int> UnlockSkill;

    public List<SkillData> curSkills; // 현재 보유한 스킬

    private void Awake()
    {
        if (instance == null)
            instance = this;

        curSkills = new List<SkillData>();
        UnlockSkill = new List<int>();
        LevelUpSkill = new List<int>();
    }

    private void Start()
    {
        playerCon = player.GetComponent<Player>();
    }

    public void Unlock(Action onUnlockComplete = null)
    {
        Time.timeScale = 0;
        UnlockSkill = GetRandomSkills(playerCon.StanbySkills.Count, 3);
        SetUnlockUI();
        onUnlockComplete?.Invoke();
    }

    private List<int> GetRandomSkills(int totalSkills, int numSkillsToShow)
    {
        HashSet<int> selectedIndexes = new HashSet<int>();
        List<int> skills = new List<int>();

        numSkillsToShow = Mathf.Min(numSkillsToShow, totalSkills);
        while (skills.Count < numSkillsToShow)
        {
            int rand;
            do
            {
                rand = UnityEngine.Random.Range(0, totalSkills);
            } while (selectedIndexes.Contains(rand));
            skills.Add(rand);
            selectedIndexes.Add(rand);
        }

        return skills;
    }

    private void SetUnlockUI()
    {
        int numSkillsToShow = UnlockSkill.Count;

        for (int i = 0; i < unlockChoices.Length; i++)
        {
            if (i < numSkillsToShow)
            {
                int skillIndex = UnlockSkill[i];
                SkillData skillData = playerCon.StanbySkills[skillIndex];
                SetSkillUI(unlockChoices[i], skillData);
            }
            else
            {
                unlockChoices[i].SetActive(false);
            }
        }

        UnlockBase.SetActive(true);
    }

    private void SetSkillUI(GameObject skillUI, SkillData skillData)
    {
        skillUI.transform.GetChild(0).GetComponent<Image>().sprite = skillData.Image;

        string skillNameAndCommand = skillData.Name.ToString();
        if (!string.IsNullOrEmpty(skillData.Command))
        {
            skillNameAndCommand += " (" + skillData.Command + ")";
        }
        skillUI.transform.GetChild(1).GetComponent<Text>().text = skillNameAndCommand;

        skillUI.transform.GetChild(2).GetComponent<Text>().text = skillData.skillType.ToString();

        skillUI.transform.GetChild(3).GetComponent<Text>().text = !string.IsNullOrEmpty(skillData.Description)
            ? skillData.Description
            : "";
    }

    public void ChoiceUnlockSkill(int num)
    {
        if (num < 0 || num >= UnlockSkill.Count)
        {
            Debug.LogError("Invalid index for choiceUnlockSkill: " + num);
            return;
        }

        int selectedSkillIndex = UnlockSkill[num];
        SkillData skillData = playerCon.StanbySkills[selectedSkillIndex];

        if (skillData.Level == 0)
            skillData.Level++;

        foreach (var childSkill in skillData.childSkill)
        {
            if (!playerCon.StanbySkills.Contains(childSkill))
                playerCon.StanbySkills.Add(childSkill);
        }

        if (!curSkills.Contains(skillData))
            curSkills.Add(skillData);

        curSkills.Sort(CompareSkills);
        playerCon.StanbySkills.Sort(CompareSkills);
        playerCon.UnlockSkills = curSkills;
        ClearSameCommand();

        UnlockBase.SetActive(false);
        Time.timeScale = 1;
    }

    public void LevelUp()
    {
        Time.timeScale = 0;
        LevelUpSkill = GetRandomSkills(playerCon.UnlockSkills.Count, levelUpChoices.Length);
        SetLevelUpUI();
    }

    private void SetLevelUpUI()
    {
        int numSkillsToShow = LevelUpSkill.Count;

        for (int i = 0; i < levelUpChoices.Length; i++)
        {
            if (i < numSkillsToShow)
            {
                int skillIndex = LevelUpSkill[i];
                SkillData skillData = playerCon.UnlockSkills[skillIndex];
                SetLevelUpSkillUI(levelUpChoices[i], skillData);
            }
            else
            {
                levelUpChoices[i].SetActive(false);
            }
        }

        LevelUpBase.SetActive(true);
    }

    private void SetLevelUpSkillUI(GameObject skillUI, SkillData skillData)
    {
        skillUI.transform.GetChild(0).GetComponent<Image>().sprite = skillData.Image;
        skillUI.transform.GetChild(1).GetComponent<Text>().text = $"{skillData.Name} ({skillData.Command}) Lv.{skillData.Level}";

        if (skillData.Level == 0)
        {
            skillUI.transform.GetChild(3).GetComponent<Text>().text = skillData.Description;
        }
        else if (skillData.Level < skillData.maxLevel)
        {
            float newDamage = skillData.damagePercent * 100 + skillData.addDmg * 100 * (skillData.Level + 1);
            skillUI.transform.GetChild(3).GetComponent<Text>().text = $"데미지가 {skillData.damagePercent * 100 + skillData.addDmg * 100}% 에서 {newDamage}%로 증가합니다.";
        }
        else
        {
            skillUI.transform.GetChild(2).GetComponent<Text>().text = "이 스킬은 마스터하셨습니다.";
        }

        skillUI.transform.GetChild(2).GetComponentInChildren<Text>().text = skillData.Level < skillData.maxLevel ? "Level Up" : "Close";
    }

    public void ChoiceLevelUpSkill(int num)
    {
        if (num < 0 || num >= LevelUpSkill.Count)
        {
            Debug.LogError("Invalid index for choiceLevelUpSkill: " + num);
            return;
        }

        SkillData skillData = playerCon.UnlockSkills[LevelUpSkill[num]];
        if (skillData.Level < skillData.maxLevel)
            skillData.Level++;

        if (!curSkills.Contains(skillData))
            curSkills.Add(skillData);

        curSkills.Sort(CompareSkills);
        playerCon.UnlockSkills.Sort(CompareSkills);

        LevelUpBase.SetActive(false);
        Time.timeScale = 1;
    }

    private int CompareSkills(SkillData a, SkillData b)
    {
        return a.Id.CompareTo(b.Id);
    }

    private void ClearSameCommand()
    {
        var commandsToRemove = playerCon.StanbySkills
            .Where(skill => playerCon.UnlockSkills.Any(unlockSkill => unlockSkill.Command == skill.Command || unlockSkill.name == skill.name))
            .ToList();

        foreach (var skill in commandsToRemove)
        {
            playerCon.StanbySkills.Remove(skill);
        }
    }
}
