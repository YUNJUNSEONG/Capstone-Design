using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillLearnTestDebug : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("skill1: " + SkillManager.skill1);
    }

}
