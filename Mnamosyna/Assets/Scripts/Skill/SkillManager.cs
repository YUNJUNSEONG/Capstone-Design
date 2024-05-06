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

    public void Unlock(Action onUnlockComplete = null)
    {
        Time.timeScale = 0;
        UnlockSkill = new List<int>();
        int numSkillsToShow = Mathf.Min(3, playerCon.StanbySkills.Count); // ���� UI�� ����� ��ų�� ������ ����մϴ�.

        // UnlockSkill ����Ʈ�� ��ų�� �߰��մϴ�.
        for (int i = 0; i < numSkillsToShow; i++)
        {
            int rand = UnityEngine.Random.Range(0, playerCon.StanbySkills.Count);
            if (!UnlockSkill.Contains(rand))
            {
                UnlockSkill.Add(rand);
            }
        }

        SetUnlockUI();

        // �ݹ� �Լ��� ȣ���մϴ�.
        onUnlockComplete?.Invoke();
    }

    void SetUnlockUI()
    {
        // UnlockSkill ����Ʈ�� ���̸� �����ɴϴ�.
        int numSkillsToShow = UnlockSkill.Count;

        Debug.Log("UnlockSkill Count: " + UnlockSkill.Count);
        for (int i = 0; i < UnlockSkill.Count; i++)
        {
            Debug.Log("UnlockSkill[" + i + "]: " + UnlockSkill[i]);
        }

        // UnlockSkill ����Ʈ�� ���̿� unlockchoices �迭�� ���̸� ���Ͽ� ���� ���� �ش��ϴ� �κ��� �����մϴ�.
        if (numSkillsToShow < unlockchoices.Length)
        {
            for (int i = 0; i < numSkillsToShow; i++)
            {
                // ��ȿ�� �˻縦 �߰��Ͽ� ��ȿ���� ���� �ε����� ���� ó���� �����մϴ�.
                if (UnlockSkill[i] < playerCon.StanbySkills.Count)
                {
                    int skillIndex = UnlockSkill[i];
                    SkillData skillData = playerCon.StanbySkills[skillIndex];

                    // UI ��� ���� �ڵ带 ���⿡ �߰��մϴ�.
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
                int skillIndex = UnlockSkill[i]; // ���� ���õ� ��ų�� �ε���

                Debug.Log("Selected Skill Index: " + skillIndex); // ���õ� ��ų�� �ε����� �α׷� ���

                // ���õ� ��ų ������ ��������
                SkillData skillData = playerCon.StanbySkills[skillIndex];
                Debug.Log("Selected Skill Data: " + skillData); // ���õ� ��ų �����͸� �α׷� ���
                Debug.Log("========================================================");


                // UI ��� ����
                // 1. �̹��� ����
                unlockchoices[i].transform.GetChild(0).GetComponent<Image>().sprite = skillData.Image;

                // 2. ��ų �̸��� Ŀ�ǵ� ���� (Ŀ�ǵ尡 ���� ��� ������� ����)
                string skillNameAndCommand = skillData.Name.ToString();
                if (!string.IsNullOrEmpty(skillData.Command))
                {
                    skillNameAndCommand += " (" + skillData.Command.ToString() + ")";
                }
                unlockchoices[i].transform.GetChild(1).GetComponent<Text>().text = skillNameAndCommand;

                // 3. ��ų�� ���� �Ӽ��� ��� (combo / Link / Passive)
                unlockchoices[i].transform.GetChild(2).GetComponent<Text>().text =  skillData.skillType.ToString();

                // 4. ��ų�� ���� ���
                // ������ ���� ��� ������� ����
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
        // num�� ��ȿ���� Ȯ��
        if (num < 0 || num >= UnlockSkill.Count)
        {
            Debug.LogError("Invalid index for choiceUnlockSkill: " + num);
            return;
        }

        // ���õ� ��ų�� �ε���
        int selectedSkillIndex = UnlockSkill[num];
        SkillData skillData = playerCon.StanbySkills[selectedSkillIndex];
        //��µ� �� ��ų���� �ε��� ��Ȯ��
        Debug.Log("========================================================");
        Debug.Log("Selected Skill Data: " + skillData);
        Debug.Log("Selected skill index: " + selectedSkillIndex);

        // ���õ� ��ų�� ������ ������Ŵ
        if (playerCon.StanbySkills[selectedSkillIndex].Level == 0)
            playerCon.StanbySkills[selectedSkillIndex].Level++;

        // �ڽ� ��ų �߰�
        for (int i = 0; i < playerCon.StanbySkills[selectedSkillIndex].childSkill.Length; i++)
        {
            if (!playerCon.StanbySkills.Contains(playerCon.StanbySkills[selectedSkillIndex].childSkill[i]))
            {
                playerCon.StanbySkills.Add(playerCon.StanbySkills[selectedSkillIndex].childSkill[i]);
            }
        }

        // ���õ� ��ų�� curSkills�� �߰�
        if (!curSkills.Contains(playerCon.StanbySkills[selectedSkillIndex]))
            curSkills.Add(playerCon.StanbySkills[selectedSkillIndex]);

        // UI ����
        UnlockBase.SetActive(false);
        curSkills.Sort(compareUISkills);
        playerCon.StanbySkills.Sort(comparePlayerSkills);
        playerCon.UnlockSkills = curSkills;
        ClearSameCommand();

        // �ð� �簳
        Time.timeScale = 1;
    }

    public void LevelUp()
    {
        Time.timeScale = 0;
        int choice = 0;
        LevelUpSkill = new List<int>();

        // �÷��̾ ������ ��ų�� ������ �����ɴϴ�.
        int numSkills = playerCon.UnlockSkills.Count;

        // UI�� ����� ��ų�� ������ �����մϴ�.
        int numSkillsToShow = Mathf.Min(numSkills, levelUpchoices.Length);

        // ���� UI�� ����� ��ų�� ������ 0 �̻��̶��
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
            // UI�� ����� ��ų�� ���� ��쿡 ���� ó���� ���⿡ �߰��� �� �ֽ��ϴ�.
            Debug.LogWarning("There are no skills to level up.");
        }
    }


    void SetLeveUpUI()
    {
        // LevelUpSkill ����Ʈ�� ���̸� �����ɴϴ�.
        int numSkillsToShow = LevelUpSkill.Count;

        // LevelUpSkill ����Ʈ�� ���̰� levelUpchoices �迭�� ���̺��� �۴ٸ�
        // ���� UI�� ����� ��ų�� ������ numSkillsToShow�� ����� �մϴ�.
        if (numSkillsToShow < levelUpchoices.Length)
        {
            // LevelUpSkill ����Ʈ�� ���̰� levelUpchoices �迭�� ���̺��� ���� ����
            // LevelUpSkill ����Ʈ�� ���̸�ŭ�� �ݺ����� �����մϴ�.
            for (int i = 0; i < numSkillsToShow; i++)
            {
                // LevelUpSkill ����Ʈ���� �ε����� ������ ����ϱ� ���� ��ȿ���� Ȯ���մϴ�.
                if (LevelUpSkill[i] < playerCon.UnlockSkills.Count)
                {
                    // ��ȿ�� �ε������ UI ��Ҹ� �����մϴ�.
                    levelUpchoices[i].transform.GetChild(0).GetComponent<Image>().sprite = playerCon.UnlockSkills[LevelUpSkill[i]].Image;
                    levelUpchoices[i].transform.GetChild(1).GetComponent<Text>().text =
                        playerCon.UnlockSkills[LevelUpSkill[i]].Name.ToString() + '(' + playerCon.UnlockSkills[LevelUpSkill[i]].Command.ToString() + ')' + " Lv." + playerCon.UnlockSkills[LevelUpSkill[i]].Level;

                    // ��Ÿ UI ��� ���� �ڵ嵵 �̾ �ۼ��մϴ�.
                }
                else
                {
                    // ��ȿ���� ���� �ε����� ��쿡 ���� ó���� ���⿡ �߰��� �� �ֽ��ϴ�.
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
        // StanbySkills�� UnlockSkills�� Command �� �̸��� �����Ͽ� ������ ����Ʈ ����
        List<string> stanbySkillsCommands = new List<string>();
        List<string> stanbySkillsNames = new List<string>();
        List<string> unlockSkillsCommands = new List<string>();
        List<string> unlockSkillsNames = new List<string>();

        // StanbySkills�� UnlockSkills���� Command �� �̸��� �����Ͽ� ����Ʈ�� �߰�
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

        // ������ ��ų ������Ʈ���� ���� ����Ʈ ����
        List<SkillData> skillsToRemove = new List<SkillData>();

        // StanbySkills���� Ȯ���� Command�� ���� Command�� ������ ��ų ������Ʈ���� ������ ����Ʈ�� �߰�
        foreach (var skill in playerCon.StanbySkills)
        {
            if (unlockSkillsCommands.Contains(skill.Command))
                skillsToRemove.Add(skill);
        }

        // UnlockSkills�� �ִ� ��ų ������Ʈ�� �̸��� ���� �̸��� ������ StanbySkills�� ��ų ������Ʈ���� ������ ����Ʈ�� �߰�
        foreach (var skill in playerCon.StanbySkills)
        {
            if (unlockSkillsNames.Contains(skill.name))
                skillsToRemove.Add(skill);
        }

        // ��ų ������Ʈ���� �� ���� ����
        foreach (var skill in skillsToRemove)
        {
            playerCon.StanbySkills.Remove(skill);
        }
    }


}