using Eliot.AgentComponents;
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

        public GameObject[] choices = new GameObject[3]; //�ִ� ���� ���� �� 3
        public List<int> LevelUpSkills; //���� �� ������ ��ų ����Ʈ(������ ��ų ����Ʈ) = unlockedSkill
        public List<int> LockedSkills; //����ִ�(������� ������)��ų ����Ʈ
        public List<SkillData> allSkills; //��� ��ų�� ��Ƶδ� ����Ʈ

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

            LevelUpSkills = new List<int>();

            LockedSkills = new List<int>();
            SkillData[] allSkills = Resources.LoadAll<SkillData>("Skills"); // Skills ������ �ִ� ��� ScriptableObject �ε�
            foreach (SkillData skill in allSkills)
            {
                LockedSkills.Add(skill.Id);
            }

            // ó�� ��ų �߿��� 3������ �������� ����
            List<SkillData> initialSkills = GetRandomInitialSkills(3);

            // ������ ��ų ��� �� LevelUpSkill ����Ʈ�� �߰�
            foreach (SkillData skill in initialSkills)
            {
                Debug.Log("Selected Skill: " + skill.Name);
                LevelUpSkills.Add(skill.Id);

                // �ڽ� ��ų Ȯ��
                CheckChildSkills(skill);
            }
        }

        private void Update()
        {
            CommandText.text = "Command: ";
            CommandText.text += playerCon.SkillCammand.Length > 5 ? playerCon.SkillCammand.Substring(playerCon.SkillCammand.Length - 5, 5) : playerCon.SkillCammand;
        }

        // ���� ó�� ��ų �߿��� �������� count���� ��ų�� �����ϴ� �Լ�
        List<SkillData> GetRandomInitialSkills(int count)
        {
            List<SkillData> selectedSkills = new List<SkillData>();
            List<SkillData> initialSkills = new List<SkillData>();

            // ���� ó�� ��ų ���͸�
            foreach (int skillId in LockedSkills)
            {
                SkillData skill = allSkills.Find(x => x.Id == skillId);
                if (skill != null && (skill.Id == 0 || skill.Id == 15 || skill.Id == 30 || skill.Id == 45))
                {
                    initialSkills.Add(skill);
                }
            }

            while (selectedSkills.Count < count && initialSkills.Count > 0)
            {
                int randIndex = Random.Range(0, initialSkills.Count);
                SkillData randSkill = initialSkills[randIndex];
                initialSkills.RemoveAt(randIndex);

                // �̹� ������ Ŀ�ǵ��� ��ų�� ����
                if (!LockedSkills.Contains(randSkill.Id))
                {
                    selectedSkills.Add(randSkill);
                    // ������ ��ų�� Ŀ�ǵ带 LockedSkill�� �߰�
                    LockedSkills.Add(randSkill.Id);
                }
            }

            return selectedSkills;
        }

        // ������ ��ų�� �ڽ� ��ų Ȯ�� �� ���
        void CheckChildSkills(SkillData skill)
        {
            foreach (SkillData childSkill in skill.childSkill)
            {
                if (!LockedSkills.Contains(childSkill.Id))
                {
                    Debug.Log("Child Skill: " + childSkill.Name);
                }
            }
        }


        public void choiceLevelUpSkill(int num)
        {
            if (playerCon.UnlockSkills[LevelUpSkills[num]].Level < playerCon.UnlockSkills[LevelUpSkills[num]].maxLevel)
                playerCon.UnlockSkills[LevelUpSkills[num]].Level++;

            for (int i = 0; i < playerCon.UnlockSkills[LevelUpSkills[num]].childSkill.Length; i++)
            {
                if (!playerCon.UnlockSkills.Contains(playerCon.UnlockSkills[LevelUpSkills[num]].childSkill[i]))
                {
                    playerCon.UnlockSkills.Add(playerCon.UnlockSkills[LevelUpSkills[num]].childSkill[i]);
                }

            }

            if (!skillUI.Contains(playerCon.UnlockSkills[LevelUpSkills[num]]))
                skillUI.Add(playerCon.UnlockSkills[LevelUpSkills[num]]);

            LevelBase.SetActive(false);
            Player.isAttack = false;
            skillUI.Sort(compareUISkills);
            //playerCon.UnlockSkills.Sort(comparePlayerSkills);

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

