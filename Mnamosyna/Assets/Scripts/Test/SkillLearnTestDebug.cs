using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillLearnTestDebug : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("몬스터닿");
    }

}
