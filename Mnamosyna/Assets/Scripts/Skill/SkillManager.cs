using Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace skill
{
    public class SkillManager : MonoBehaviour
    {
        public static SkillManager instance;
        public GameObject player;
        public Player playerCon;

        public GameObject[] choices = new GameObject[3]; //최대 선택 가능 수 3
        public List<int> LevelUpSkill; //레벨 업 가능한 스킬 리스트(보유한 스킬 리스트)
        public List<int> LockedSkill; //잠겨있는(잠금해제 가능한)스킬 리스트

        public GameObject LevelBase;
        public Text CommandText;
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
            playerCon = player.GetComponent<Player>();
        }

        private void Update()
        {
            CommandText.text = "Command: ";
            CommandText.text += playerCon.SkillCammand.Length > 5 ? playerCon.SkillCammand.Substring(playerCon.SkillCammand.Length - 5, 5) : playerCon.SkillCammand;
        }

        public void choiceLevelUpSkill(int num)
        {
            if (playerCon.UnlockSkills[LevelUpSkill[num]].Level < playerCon.UnlockSkills[LevelUpSkill[num]].maxLevel)
                playerCon.UnlockSkills[LevelUpSkill[num]].Level++;

            for (int i = 0; i < playerCon.UnlockSkills[LevelUpSkill[num]].childSkill.Length; i++)
            {
                if (!playerCon.UnlockSkills.Contains(playerCon.UnlockSkills[LevelUpSkill[num]].childSkill[i]))
                {
                    playerCon.UnlockSkills.Add(playerCon.UnlockSkills[LevelUpSkill[num]].childSkill[i]);
                }

            }

            if (!skillUI.Contains(playerCon.UnlockSkills[LevelUpSkill[num]]))
                skillUI.Add(playerCon.UnlockSkills[LevelUpSkill[num]]);

            LevelBase.SetActive(false);
            Player.isAttack = false;
            skillUI.Sort(compareUISkills);
            playerCon.UnlockSkills.Sort(comparePlayerSkills);

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

        public void Dash()
        {
            DashUI.GetComponent<Image>().enabled = true;
            DashUI.fillAmount = 0;
            StartCoroutine(DashCoolTime());
        }

        IEnumerator DashCoolTime()
        {
            float coolTime = 2.5f;
            while(coolTime > 0.0f)
            {
                coolTime -= Time.deltaTime;
                DashUI.fillAmount = 1.0f - (coolTime / 2.5f);
                yield return new WaitForFixedUpdate();
            }
            DashUI.GetComponent<Image>().enabled = false;
        }
    }
}

