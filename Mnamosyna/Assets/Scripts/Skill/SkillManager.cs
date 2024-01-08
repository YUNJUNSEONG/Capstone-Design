using Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SkillManager : MonoBehaviour
{
    public static SkillManager instance;
    public GameObject player;
    private PlayableCharacterController playableCharacterController;
    public GameObject[] choices = new GameObject[3];
    public List<int> LevelUpSkill;
    public GameObject LevelBase;
    public Text CommandText;
    public Text killText;
    public Slider HpBar;
    public Image[] UnlockSkill = new Image[9];
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
        playableCharacterController = player.GetComponent<PlayableCharacterController>();
    }

    private void Update()
    {

        // killText.text = GameManager.instance.kill + " / " + GameManager.instance.needKill;
        CommandText.text = "Command: ";
        CommandText.text += playableCharacterController.SkillCammand.Length > 5 ? playableCharacterController.SkillCammand.Substring(playableCharacterController.SkillCammand.Length - 5, 5) : playableCharacterController.SkillCammand;
        HpBar.value = playableCharacterController.Life / 200;
    }

    public void LevelUp()
    {
        Time.timeScale = 0;
        PlayableCharacterController.isAttack = true;
        playableCharacterController.SkillCammand = "";
        int choice = 0;
        LevelUpSkill = new List<int>();
        while (choice < 3)
        { 
            int rand = UnityEngine.Random.Range(0, playableCharacterController.UnlockSkills.Count);
            if(LevelUpSkill.Contains(rand))
            {
                continue;
            }
            else
            {
                LevelUpSkill.Add(rand);
                choice++;
            }
        }

        SetUI();
    }

    void SetUI()
    {
        for(int i = 0; i < choices.Length; i++)
        {
            choices[i].transform.GetChild(0).GetComponent<Image>().sprite = playableCharacterController.UnlockSkills[LevelUpSkill[i]].skillImage;

            choices[i].transform.GetChild(1).GetComponent<Text>().text =
                playableCharacterController.UnlockSkills[LevelUpSkill[i]].skillName.ToString() + '(' + playableCharacterController.UnlockSkills[LevelUpSkill[i]].skillCommand.ToString() + ')' + " Lv." + playableCharacterController.UnlockSkills[LevelUpSkill[i]].skillLevel;

            if (playableCharacterController.UnlockSkills[LevelUpSkill[i]].skillLevel == 0)
                choices[i].transform.GetChild(2).GetComponent<Text>().text = playableCharacterController.UnlockSkills[LevelUpSkill[i]].skillDescription.ToString();
            else if (playableCharacterController.UnlockSkills[LevelUpSkill[i]].skillLevel < playableCharacterController.UnlockSkills[LevelUpSkill[i]].maxLevel)
                choices[i].transform.GetChild(2).GetComponent<Text>().text = "데미지가 " + (playableCharacterController.UnlockSkills[LevelUpSkill[i]].baseDamage + playableCharacterController.UnlockSkills[LevelUpSkill[i]].ad * playableCharacterController.UnlockSkills[LevelUpSkill[i]].skillLevel) + "에서 " + (playableCharacterController.UnlockSkills[LevelUpSkill[i]].baseDamage + playableCharacterController.UnlockSkills[LevelUpSkill[i]].ad * (playableCharacterController.UnlockSkills[LevelUpSkill[i]].skillLevel + 1)) + "로 증가합니다.";
            else
                choices[i].transform.GetChild(2).GetComponent<Text>().text = "이 스킬은 마스터하셨습니다.";

            if (playableCharacterController.UnlockSkills[LevelUpSkill[i]].skillLevel < playableCharacterController.UnlockSkills[LevelUpSkill[i]].maxLevel)
            {
                choices[i].transform.GetChild(3).GetComponentInChildren<Text>().text = "Level Up";
            }
            else
            {
                choices[i].transform.GetChild(3).GetComponentInChildren<Text>().text = "Close";
            }
        }

        LevelBase.SetActive(true);
    }

    public void choiceLevelUpSkill(int num)
    {
        if(playableCharacterController.UnlockSkills[LevelUpSkill[num]].skillLevel < playableCharacterController.UnlockSkills[LevelUpSkill[num]].maxLevel)
            playableCharacterController.UnlockSkills[LevelUpSkill[num]].skillLevel++;

        for(int i = 0; i < playableCharacterController.UnlockSkills[LevelUpSkill[num]].childSkill.Length; i++)
        {
            if (!playableCharacterController.UnlockSkills.Contains(playableCharacterController.UnlockSkills[LevelUpSkill[num]].childSkill[i]))
            {
                playableCharacterController.UnlockSkills.Add(playableCharacterController.UnlockSkills[LevelUpSkill[num]].childSkill[i]);
            }

        }

        if (!skillUI.Contains(playableCharacterController.UnlockSkills[LevelUpSkill[num]]))
            skillUI.Add(playableCharacterController.UnlockSkills[LevelUpSkill[num]]);

        LevelBase.SetActive(false);
        PlayableCharacterController.isAttack = false;
        skillUI.Sort(compareUISkills);
        playableCharacterController.UnlockSkills.Sort(comparePlayerSkills);

        for (int i = 0; i < skillUI.Count; i++)
        {
            UnlockSkill[i].GetComponent<Image>().enabled = true;
            UnlockSkill[i].sprite = skillUI[i].skillImage;
        }

        Time.timeScale = 1;
    }

    int compareUISkills(SkillData a, SkillData b)
    {
        return a.skillId < b.skillId ? -1 : 1;
    }

    int comparePlayerSkills(SkillData a, SkillData b)
    {
        if ((a.isUnlock && b.isUnlock) || (!a.isUnlock && !b.isUnlock))
            return a.skillId < b.skillId ? -1 : 1;
        else if (a.isUnlock)
            return -1;
        else
            return 1;
    }

    public void Dash()
    {
        DashUI.GetComponent<Image>().enabled = true;
        DashUI.fillAmount = 0;
        StartCoroutine(DashCoolTime());
    }

    IEnumerator DashCoolTime()
    {
        float coolTime = playableCharacterController.DashTime;
        while(coolTime > 0.0f)
        {
            coolTime -= Time.deltaTime;
            DashUI.fillAmount = 1.0f - (coolTime / playableCharacterController.DashTime);
            yield return new WaitForFixedUpdate();
        }
        DashUI.GetComponent<Image>().enabled = false;
    }
}
