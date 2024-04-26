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


    public List<SkillData> curSkills; // ���� ������ ��ų
    //public Image DashUI;


    private void Awake()
    {
        if (instance == null)
            instance = this;

        curSkills.Clear();

        UnlockSkill = new List<int>(); // UnlockSkill �ʱ�ȭ
        LevelUpSkill = new List<int>(); // LevelUpSkill �ʱ�ȭ

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
                unlockchoices[i].transform.GetChild(2).GetComponent<Text>().text = "�������� " + (playerCon.StanbySkills[UnlockSkill[i]].damagePercent +"%" + playerCon.StanbySkills[UnlockSkill[i]].addDmg * playerCon.StanbySkills[UnlockSkill[i]].Level) + "���� " + (playerCon.StanbySkills[UnlockSkill[i]].damagePercent + playerCon.StanbySkills[UnlockSkill[i]].addDmg * (playerCon.StanbySkills[UnlockSkill[i]].Level + 1)) + "�� �����մϴ�.";
            else
                unlockchoices[i].transform.GetChild(2).GetComponent<Text>().text = "�� ��ų�� �������ϼ̽��ϴ�.";
             
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
                levelUpchoices[i].transform.GetChild(2).GetComponent<Text>().text = "�������� " + (playerCon.UnlockSkills[LevelUpSkill[i]].damagePercent +"%" + playerCon.UnlockSkills[LevelUpSkill[i]].addDmg * playerCon.UnlockSkills[LevelUpSkill[i]].Level) + "���� " + (playerCon.UnlockSkills[LevelUpSkill[i]].damagePercent + playerCon.UnlockSkills[LevelUpSkill[i]].addDmg * (playerCon.UnlockSkills[LevelUpSkill[i]].Level + 1)) + "�� �����մϴ�.";
            else
                levelUpchoices[i].transform.GetChild(2).GetComponent<Text>().text = "�� ��ų�� �������ϼ̽��ϴ�.";

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
        // �� Ŀ�ǵ忡 �ش��ϴ� ��ų ����� �����ϴ� ����Ʈ
        Dictionary<string, List<SkillData>> commandsMap = new Dictionary<string, List<SkillData>>();

        // �ߺ��� �̸��� ���� �нú� ��ų ���Ÿ� ���� HashSet �� ����Ʈ
        HashSet<string> passiveSkillNames = new HashSet<string>();
        List<SkillData> passiveSkillsToRemove = new List<SkillData>();

        foreach (SkillData skillData in playerCon.StanbySkills)
        {
            if (string.IsNullOrEmpty(skillData.Command))
            {
                // Ŀ�ǵ尡 ���� ��� �нú� ��ų�� �Ǵ��ϰ� �ߺ��� �̸��� ���� �нú긦 ����
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
                // Ŀ�ǵ带 Ű�� �ϰ� �ش� Ŀ�ǵ忡 �ش��ϴ� ��ų�� ������ �ϴ� ��ųʸ��� ����
                if (!commandsMap.ContainsKey(skillData.Command))
                {
                    commandsMap.Add(skillData.Command, new List<SkillData>());
                }
                commandsMap[skillData.Command].Add(skillData);
            }
        }

        // �нú� ��ų ����
        foreach (SkillData skillToRemove in passiveSkillsToRemove)
        {
            playerCon.StanbySkills.Remove(skillToRemove);
        }

        // unlock���� ���õ� ��ų�� �׿� ���� Ŀ�ǵ带 ���� ��ų���� ��� ����
        foreach (var kvp in commandsMap)
        {
            string command = kvp.Key;
            List<SkillData> skillsWithSameCommand = kvp.Value;

            // unlock���� ���õ� ��ų�� �׿� ���� Ŀ�ǵ带 ���� ��ų���� ����
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
        // ��ųʸ��� ������ ���
        foreach (var kvp in commandsMap)
        {
            string command = kvp.Key;
            List<SkillData> skillsWithSameCommand = kvp.Value;
            Debug.Log("Command: " + command + ", Skills Count: " + skillsWithSameCommand.Count);
        }
    }

}

