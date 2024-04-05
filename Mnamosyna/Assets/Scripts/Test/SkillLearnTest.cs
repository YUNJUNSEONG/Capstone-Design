using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillLearnTest : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        SkillManager.SetLearnedSkill(ref SkillManager.skill1, true);
    }
}
