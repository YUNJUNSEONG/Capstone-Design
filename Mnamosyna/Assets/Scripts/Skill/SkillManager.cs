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

        curSkills = new List<SkillData>(); // curSkills �ʱ�ȭ �߰�

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

        // ���� UI�� ����� ��ų�� ������ ����մϴ�.
        int numSkillsToShow = Mathf.Min(3, playerCon.StanbySkills.Count);

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
        UnlockBase.SetActive(true); // UnlockBase Ȱ��ȭ

        // UnlockSkill ����Ʈ�� ���̸� �����ɴϴ�.
        int numSkillsToShow = Mathf.Min(UnlockSkill.Count, unlockchoices.Length);

        // UnlockSkill ����Ʈ�� ���̸�ŭ �ݺ��Ͽ� UI ����
        for (int i = 0; i < numSkillsToShow; i++)
        {
            int skillIndex = UnlockSkill[i];
            SkillData skillData = playerCon.StanbySkills[skillIndex];

            // UI ��� ����
            unlockchoices[i].SetActive(true); // �ش� ��ų UI Ȱ��ȭ
            unlockchoices[i].transform.GetChild(0).GetComponent<Image>().sprite = skillData.Image; // �̹��� ����
            string skillNameAndCommand = skillData.Name.ToString();
            if (!string.IsNullOrEmpty(skillData.Command))
            {
                skillNameAndCommand += " (" + skillData.Command.ToString() + ")";
            }
            unlockchoices[i].transform.GetChild(1).GetComponent<Text>().text = skillNameAndCommand;

            unlockchoices[i].transform.GetChild(2).GetComponent<Text>().text = skillData.skillType.ToString(); // ��ų ���� �Ӽ� ����
            unlockchoices[i].transform.GetChild(3).GetComponent<Text>().text = skillData.Description; // ��ų ���� ����
        }

        // ���� UI ��Ȱ��ȭ
        for (int i = numSkillsToShow; i < unlockchoices.Length; i++)
        {
            unlockchoices[i].SetActive(false);
        }
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
        LevelUpSkill = new List<int>();

        // �÷��̾ ������ ��ų�� ������ �����ɴϴ�.
        int numSkills = playerCon.UnlockSkills.Count;

        // UI�� ����� ��ų�� ������ �����մϴ�.
        int numSkillsToShow = Mathf.Min(numSkills, 3);

        // ���� UI�� ����� ��ų�� ������ 0 �̻��̶��
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
            // UI�� ����� ��ų�� ���� ��쿡 ���� ó���� ���⿡ �߰��� �� �ֽ��ϴ�.
            Debug.LogWarning("There are no skills to level up.");
        }
    }


    void SetLeveUpUI()
    {
        LevelUpBase.SetActive(true); // LevelUpBase Ȱ��ȭ

        // LevelUpSkill ����Ʈ�� ���̸� �����ɴϴ�.
        int numSkillsToShow = Mathf.Min(LevelUpSkill.Count, levelUpchoices.Length);

        // LevelUpSkill ����Ʈ�� ���̸�ŭ �ݺ��Ͽ� UI ����
        for (int i = 0; i < numSkillsToShow; i++)
        {
            int skillIndex = LevelUpSkill[i];
            SkillData skillData = playerCon.UnlockSkills[skillIndex];

            // UI ��� ����
            levelUpchoices[i].SetActive(true); // �ش� ��ų UI Ȱ��ȭ
            levelUpchoices[i].transform.GetChild(0).GetComponent<Image>().sprite = skillData.Image; // �̹��� ����
            string skillNameAndCommand = skillData.Name.ToString();
            if (!string.IsNullOrEmpty(skillData.Command))
            {
                skillNameAndCommand += " (" + skillData.Command.ToString() + ")";
            }
            levelUpchoices[i].transform.GetChild(1).GetComponent<Text>().text = skillNameAndCommand;
            levelUpchoices[i].transform.GetChild(3).GetComponent<Text>().text = skillData.Description; // ��ų ���� ����
        }

        // ���� UI ��Ȱ��ȭ
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
 
