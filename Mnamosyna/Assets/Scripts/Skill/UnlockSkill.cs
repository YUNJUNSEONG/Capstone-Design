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
                    continue; // �̹� ������ ��ų�̸� �ǳʶݴϴ�.
                }
                else
                {
                    manager.LevelUpSkills.Add(selectedSkillId); // ������ ��ų ����Ʈ�� �߰��մϴ�.
                    choice++;
                }
            }

            // UI ������Ʈ�� �����մϴ�.
            ui.SetUnlockUI();


            // �ð� �������� ���� ������ �ٽ� �����մϴ�.
            Time.timeScale = 1;
        }

    }
}
*/
