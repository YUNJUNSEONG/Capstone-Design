using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHp : MonoBehaviour
{
    public GameObject boss;
    public BossStat Boss;
    private Image hpImage;

    void Start()
    {
        Boss = GetComponentInParent<BossStat>();
        GameObject hpBarObject = GameObject.Find("HPbar");
        hpImage = GetComponent<Image>();

    }

    void Update()
    {
        hpImage.fillAmount = (float)Boss.Cur_HP / Boss.Max_HP;
    }
}
