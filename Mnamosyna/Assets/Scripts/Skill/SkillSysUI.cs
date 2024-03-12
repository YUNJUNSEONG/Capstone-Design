using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace skill
{
    public class SkillSysUI : MonoBehaviour
    {
        SkillManager manager = new SkillManager();

        public void SetUnlockUI()
        {
            for (int i = 0; i < manager.choices.Length; i++)
            {
                manager.choices[i].transform.GetChild(0).GetComponent<Image>().sprite = manager.playerCon.allSkills[manager.LevelUpSkills[i]].Image;

                manager.choices[i].transform.GetChild(1).GetComponent<Text>().text =
                    manager.playerCon.UnlockSkills[manager.LevelUpSkills[i]].Name.ToString() + '(' + manager.playerCon.UnlockSkills[manager.LevelUpSkills[i]].Command.ToString() + ')' + " Lv." + manager.playerCon.UnlockSkills[manager.LevelUpSkills[i]].Level;

                if (manager.playerCon.UnlockSkills[manager.LevelUpSkills[i]].Level == 0)
                    manager.choices[i].transform.GetChild(2).GetComponent<Text>().text = manager.playerCon.UnlockSkills[manager.LevelUpSkills[i]].Description.ToString();
                else if (manager.playerCon.UnlockSkills[manager.LevelUpSkills[i]].Level < manager.playerCon.UnlockSkills[manager.LevelUpSkills[i]].maxLevel)
                    manager.choices[i].transform.GetChild(2).GetComponent<Text>().text = "데미지가 " + (manager.playerCon.UnlockSkills[manager.LevelUpSkills[i]].damagePercent + (manager.playerCon.UnlockSkills[manager.LevelUpSkills[i]].addDmg * manager.playerCon.UnlockSkills[manager.LevelUpSkills[i]].Level)) + "에서 " + (manager.playerCon.UnlockSkills[manager.LevelUpSkills[i]].damagePercent + (manager.playerCon.UnlockSkills[manager.LevelUpSkills[i]].addDmg * (manager.playerCon.UnlockSkills[manager.LevelUpSkills[i]].Level + 1))) + "로 증가합니다.";
                else
                    manager.choices[i].transform.GetChild(2).GetComponent<Text>().text = "이 스킬은 마스터하셨습니다.";

                if (manager.playerCon.UnlockSkills[manager.LevelUpSkills[i]].Level < manager.playerCon.UnlockSkills[manager.LevelUpSkills[i]].maxLevel)
                {
                    manager.choices[i].transform.GetChild(3).GetComponentInChildren<Text>().text = "Level Up";
                }
                else
                {
                    manager.choices[i].transform.GetChild(3).GetComponentInChildren<Text>().text = "Close";
                }
            }

            manager.LevelBase.SetActive(true);
        }

        public void SetLevelUpUI()
        {
            for (int i = 0; i < manager.choices.Length; i++)
            {
                manager.choices[i].transform.GetChild(0).GetComponent<Image>().sprite = manager.playerCon.UnlockSkills[manager.LevelUpSkills[i]].Image;

                manager.choices[i].transform.GetChild(1).GetComponent<Text>().text =
                    manager.playerCon.UnlockSkills[manager.LevelUpSkills[i]].Name.ToString() + '(' + manager.playerCon.UnlockSkills[manager.LevelUpSkills[i]].Command.ToString() + ')' + " Lv." + manager.playerCon.UnlockSkills[manager.LevelUpSkills[i]].Level;

                if (manager.playerCon.UnlockSkills[manager.LevelUpSkills[i]].Level == 0)
                    manager.choices[i].transform.GetChild(2).GetComponent<Text>().text = manager.playerCon.UnlockSkills[manager.LevelUpSkills[i]].Description.ToString();
                else if (manager.playerCon.UnlockSkills[manager.LevelUpSkills[i]].Level < manager.playerCon.UnlockSkills[manager.LevelUpSkills[i]].maxLevel)
                    manager.choices[i].transform.GetChild(2).GetComponent<Text>().text = "데미지가 " + (manager.playerCon.UnlockSkills[manager.LevelUpSkills[i]].damagePercent + (manager.playerCon.UnlockSkills[manager.LevelUpSkills[i]].addDmg * manager.playerCon.UnlockSkills[manager.LevelUpSkills[i]].Level)) + "에서 " + (manager.playerCon.UnlockSkills[manager.LevelUpSkills[i]].damagePercent + (manager.playerCon.UnlockSkills[manager.LevelUpSkills[i]].addDmg * (manager.playerCon.UnlockSkills[manager.LevelUpSkills[i]].Level + 1))) + "로 증가합니다.";
                else
                    manager.choices[i].transform.GetChild(2).GetComponent<Text>().text = "이 스킬은 마스터하셨습니다.";

                if (manager.playerCon.UnlockSkills[manager.LevelUpSkills[i]].Level < manager.playerCon.UnlockSkills[manager.LevelUpSkills[i]].maxLevel)
                {   
                    manager.choices[i].transform.GetChild(3).GetComponentInChildren<Text>().text = "Level Up";
                }
                else
                {
                    manager.choices[i].transform.GetChild(3).GetComponentInChildren<Text>().text = "Close";
                }
            }

            manager.LevelBase.SetActive(true);
        }
    }
}

