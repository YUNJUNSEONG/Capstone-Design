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
    private Player playableCharacterController;
    public GameObject[] choices = new GameObject[3];
    public List<int> LevelUpSkill;
    public GameObject LevelBase;
    public Text CommandText;
    public Image[] UnlockSkill = new Image[9];
    public List<SkillData> skillUI;
    //public Image DashUI;

    private void Awake()
    {
        if (instance == null)
            instance = this;

        skillUI.Clear();
    }

    private void Start()
    {
        playableCharacterController = player.GetComponent<Player>();
    }

    private void Update()
    {
        // killText.text = GameManager.instance.kill + " / " + GameManager.instance.needKill;
        CommandText.text = "Command: ";
        CommandText.text += playableCharacterController.SkillCammand.Length > 5 ? playableCharacterController.SkillCammand.Substring(playableCharacterController.SkillCammand.Length - 5, 5) : playableCharacterController.SkillCammand;
    }

    public void LevelUp()
    {
        Time.timeScale = 0;
        Player.isAttack = true;
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
            choices[i].transform.GetChild(0).GetComponent<Image>().sprite = playableCharacterController.UnlockSkills[LevelUpSkill[i]].Image;

            choices[i].transform.GetChild(1).GetComponent<Text>().text =
                playableCharacterController.UnlockSkills[LevelUpSkill[i]].Name.ToString() + '(' + playableCharacterController.UnlockSkills[LevelUpSkill[i]].Command.ToString() + ')' + " Lv." + playableCharacterController.UnlockSkills[LevelUpSkill[i]].Level;

            if (playableCharacterController.UnlockSkills[LevelUpSkill[i]].Level == 0)
                choices[i].transform.GetChild(2).GetComponent<Text>().text = playableCharacterController.UnlockSkills[LevelUpSkill[i]].Description.ToString();
            else if (playableCharacterController.UnlockSkills[LevelUpSkill[i]].Level < playableCharacterController.UnlockSkills[LevelUpSkill[i]].maxLevel)
                choices[i].transform.GetChild(2).GetComponent<Text>().text = "데미지가 " + (playableCharacterController.UnlockSkills[LevelUpSkill[i]].damagePercent + (playableCharacterController.UnlockSkills[LevelUpSkill[i]].addDmg * playableCharacterController.UnlockSkills[LevelUpSkill[i]].Level)) + "에서 " + (playableCharacterController.UnlockSkills[LevelUpSkill[i]].damagePercent + (playableCharacterController.UnlockSkills[LevelUpSkill[i]].addDmg * (playableCharacterController.UnlockSkills[LevelUpSkill[i]].Level + 1))) + "로 증가합니다.";
            else
                choices[i].transform.GetChild(2).GetComponent<Text>().text = "이 스킬은 마스터하셨습니다.";

            if (playableCharacterController.UnlockSkills[LevelUpSkill[i]].Level < playableCharacterController.UnlockSkills[LevelUpSkill[i]].maxLevel)
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
        if(playableCharacterController.UnlockSkills[LevelUpSkill[num]].Level < playableCharacterController.UnlockSkills[LevelUpSkill[num]].maxLevel)
            playableCharacterController.UnlockSkills[LevelUpSkill[num]].Level++;

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
        Player.isAttack = false;
        skillUI.Sort(compareUISkills);
        playableCharacterController.UnlockSkills.Sort(comparePlayerSkills);

        for (int i = 0; i < skillUI.Count; i++)
        {
            UnlockSkill[i].GetComponent<Image>().enabled = true;
            UnlockSkill[i].sprite = skillUI[i].Image;
        }

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

    /*public void Dash()
    {
        DashUI.GetComponent<Image>().enabled = true;
        DashUI.fillAmount = 0;
    }*/

    /*IEnumerator DashCoolTime()
    {
        float coolTime = playableCharacterController.DashTime;
        while(coolTime > 0.0f)
        {
            coolTime -= Time.deltaTime;
            DashUI.fillAmount = 1.0f - (coolTime / playableCharacterController.DashTime);
            yield return new WaitForFixedUpdate();
        }
        DashUI.GetComponent<Image>().enabled = false;
    }*/
}

