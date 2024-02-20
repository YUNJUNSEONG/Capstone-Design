using skill;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace skill
{
    public class LevelUpSkill : MonoBehaviour
    {
        SkillManager manager = new SkillManager();
        SkillSysUI ui = new SkillSysUI();
        public void Unlock()
        {
            Time.timeScale = 0;
            manager.playerCon.SkillCammand = "";
            int choice = 0;
            manager.LevelUpSkill = new List<int>();
            while (choice < 3)
            {
                int rand = UnityEngine.Random.Range(0, manager.playerCon.UnlockSkills.Count);
                if (manager.LevelUpSkill.Contains(rand))
                {
                    continue;
                }
                else
                {
                    manager.LevelUpSkill.Add(rand);
                    choice++;
                }
            }

            ui.SetLevelUpUI();
        }
    }
}
