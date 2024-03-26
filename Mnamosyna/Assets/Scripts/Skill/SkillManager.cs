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

        public List<SkillData> LevelUpSkills; //레벨 업 가능한 스킬 리스트(보유한 스킬 리스트) = unlockedSkill
        public List<SkillData> LockedSkills; //잠겨있는(잠금해제 가능한)스킬 리스트
        public List<SkillData> allSkills; //모든 스킬을 담아두는 리스트

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
            SkillData[] allSkills = Resources.LoadAll<SkillData>("Skills"); // Skills 폴더에 있는 모든 ScriptableObject 로드
            foreach (SkillData skill in allSkills)
            {
                LockedSkills.Add(skill);
            }

            // 처음 스킬 중에서 3가지를 무작위로 선택
            List<SkillData> initialSkills = GetRandomInitialSkills(3);
        }

        private void Update()
        {
            CommandText.text = "Command: ";
            CommandText.text += playerCon.SkillCammand.Length > 5 ? playerCon.SkillCammand.Substring(playerCon.SkillCammand.Length - 5, 5) : playerCon.SkillCammand;
        }

        // 가장 처음 스킬 중에서 무작위로 count개의 스킬을 선택하는 함수
        List<SkillData> GetRandomInitialSkills(int count)
        {
            List<SkillData> selectedSkills = new List<SkillData>();

            // 대쉬 스킬 4개를 처음 스킬로 설정
            List<SkillData> initialSkills = new List<SkillData>();
            foreach (SkillData skill in allSkills)
            {
                if (skill.Id == 0 || skill.Id == 15 || skill.Id == 30 || skill.Id == 45)
                {
                    initialSkills.Add(skill);
                }
            }
            HashSet<int> selectedSkillIds = new HashSet<int>(); // 이미 선택된 스킬 ID를 기록하는 HashSet

            // count 개의 스킬 선택
            while (selectedSkills.Count < count && initialSkills.Count > 0)
            {
                int randIndex = Random.Range(0, initialSkills.Count);
                SkillData randSkill = initialSkills[randIndex];

                // 이미 선택된 스킬인지 확인
                if (!selectedSkillIds.Contains(randSkill.Id))
                {
                    selectedSkills.Add(randSkill);
                    selectedSkillIds.Add(randSkill.Id); // 선택한 스킬의 ID를 기록
                }
                initialSkills.RemoveAt(randIndex); // 선택한 스킬은 초기 스킬 목록에서 제거
            }

            return selectedSkills;
        }

        List<SkillData> GetRandomChildSkills(int count)
        {
            List<SkillData> selectedSkills = new List<SkillData>();

            //보유한 스킬의 자식 스킬확인

            // count 개의 스킬 선택
            while (selectedSkills.Count < count && initialSkills.Count > 0)
            {
                int randIndex = Random.Range(0, initialSkills.Count);
                SkillData randSkill = initialSkills[randIndex];
                  
                // 이미 선택된 스킬인지 확인
                if (!selectedSkillIds.Contains(randSkill.Id))
                {
                    selectedSkills.Add(randSkill);
                    selectedSkillIds.Add(randSkill.Id); // 선택한 스킬의 ID를 기록
                }
                initialSkills.RemoveAt(randIndex); // 선택한 스킬은 초기 스킬 목록에서 제거
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
