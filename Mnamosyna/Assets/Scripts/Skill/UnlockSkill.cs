using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using skill;
using Eliot.AgentComponents;

/*
namespace skill
{
    public class UnlockSkill : MonoBehaviour
    {
        SkillManager manager = new SkillManager();
        SkillSysUI ui = new SkillSysUI();
        public void unlockPlayer(GameObject playerObject)
        {
            Time.timeScale = 0;

            int choice = 0;

            while (choice < 3)
            {
                int rand = UnityEngine.Random.Range(0, manager.LockedSkills.Count);
                int selectedSkillId = manager.LockedSkills[rand];

                if (manager.LevelUpSkills.Contains(selectedSkillId))
                {
                    continue; // 이미 보유한 스킬이면 건너뜁니다.
                }
                else
                {
                    manager.LevelUpSkills.Add(selectedSkillId); // 보유한 스킬 리스트에 추가합니다.
                    choice++;
                }
            }

            // UI 업데이트를 수행합니다.
            ui.SetUnlockUI();


            // 시간 스케일을 원래 값으로 다시 설정합니다.
            Time.timeScale = 1;
        }

    }
}
*/
