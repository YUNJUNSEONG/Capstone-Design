//using Eliot.AgentComponents;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

/*
namespace skill
{
    public class SkillManager : MonoBehaviour
    {
        public static SkillManager instance;
        public GameObject player;
        public Player playerCon;

        public List<SkillData> LevelUpSkills; //���� �� ������ ��ų ����Ʈ(������ ��ų ����Ʈ) = unlockedSkill
        public List<SkillData> LockedSkills; //����ִ�(������� ������)��ų ����Ʈ
        public List<SkillData> allSkills; //��� ��ų�� ��Ƶδ� ����Ʈ

        public Text CommandText;
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

            LevelUpSkills = new List<SkillData>();

            LockedSkills = new List<SkillData>();
            SkillData[] allSkills = Resources.LoadAll<SkillData>("Skills"); // Skills ������ �ִ� ��� ScriptableObject �ε�
            foreach (SkillData skill in allSkills)
            {
                LockedSkills.Add(skill);
            }

            // ó�� ��ų �߿��� 3������ �������� ����
            List<SkillData> initialSkills = GetRandomInitialSkills(3);
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

            // �뽬 ��ų 4���� ó�� ��ų�� ����
            List<SkillData> initialSkills = new List<SkillData>();
            foreach (SkillData skill in allSkills)
            {
                if (skill.Id == 0 || skill.Id == 15 || skill.Id == 30 || skill.Id == 45)
                {
                    initialSkills.Add(skill);
                }
            }
            HashSet<int> selectedSkillIds = new HashSet<int>(); // �̹� ���õ� ��ų ID�� ����ϴ� HashSet

            // count ���� ��ų ����
            while (selectedSkills.Count < count && initialSkills.Count > 0)
            {
                int randIndex = Random.Range(0, initialSkills.Count);
                SkillData randSkill = initialSkills[randIndex];

                // �̹� ���õ� ��ų���� Ȯ��
                if (!selectedSkillIds.Contains(randSkill.Id))
                {
                    selectedSkills.Add(randSkill);
                    selectedSkillIds.Add(randSkill.Id); // ������ ��ų�� ID�� ���
                }
                initialSkills.RemoveAt(randIndex); // ������ ��ų�� �ʱ� ��ų ��Ͽ��� ����
            }

            return selectedSkills;
        }

        List<SkillData> GetRandomChildSkills(int count)
        {
            List<SkillData> selectedSkills = new List<SkillData>();

            //������ ��ų�� �ڽ� ��ųȮ��

            // count ���� ��ų ����
            while (selectedSkills.Count < count && initialSkills.Count > 0)
            {
                int randIndex = Random.Range(0, initialSkills.Count);
                SkillData randSkill = initialSkills[randIndex];
                  
                // �̹� ���õ� ��ų���� Ȯ��
                if (!selectedSkillIds.Contains(randSkill.Id))
                {
                    selectedSkills.Add(randSkill);
                    selectedSkillIds.Add(randSkill.Id); // ������ ��ų�� ID�� ���
                }
                initialSkills.RemoveAt(randIndex); // ������ ��ų�� �ʱ� ��ų ��Ͽ��� ����
            }

            return selectedSkills;
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
*/
