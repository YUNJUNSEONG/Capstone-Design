using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterHPBar : MonoBehaviour
{
    public GameObject monster;
    public BaseMonster Monster;
    private Image hpImage;  

    void Start()
    {
        Monster = GetComponentInParent<BaseMonster>();
        GameObject hpBarObject = GameObject.Find("HPbar"); 
        hpImage = GetComponent<Image>();
        
    }
    
    void Update()
    {
        hpImage.fillAmount = (float)Monster.Cur_HP / Monster.Max_HP;
    }
}
