using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using skill;

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

            manager.LockedSkill = new List<int>();
            while (choice < 3)
            {
                int rand = UnityEngine.Random.Range(0, manager.playerCon.UnlockSkills.Count);
                if (manager.LockedSkill.Contains(rand))
                {
                    continue;
                }
                else
                {
                    manager.LockedSkill.Add(rand);
                    choice++;
                }
            }

            ui.SetUnlockUI();
        }
    }
}
