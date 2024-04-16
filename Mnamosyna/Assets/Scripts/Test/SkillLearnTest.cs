using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillLearnTest : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        SkillManager1.SetLearnedSkill(ref SkillManager1.skill1, true);
    }
}
