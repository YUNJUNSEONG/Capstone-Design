using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using skill;
using Eliot.AgentComponents;

namespace skill
{
    public class UnlockSkill : MonoBehaviour
    {
        SkillManager manager = new SkillManager();
        SkillSysUI ui = new SkillSysUI();
        public void Unlock()
        {
            Time.timeScale = 0;
            manager.playerCon.SkillCammand = "";
            int choice = 0;

            while (choice < 3)
            {
                int rand = UnityEngine.Random.Range(0, manager.playerCon.UnlockSkills.Count);
                if (manager.LockedSkills.Contains(rand))
                {
                    continue;
                }
                else
                {
                    manager.LockedSkills.Add(rand);
                    choice++;
                }
            }

            ui.SetUnlockUI();
        }
    }
}
