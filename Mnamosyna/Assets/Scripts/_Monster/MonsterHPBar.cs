using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterHPBar : MonoBehaviour
{
    public GameObject monster;
    public Monster Monster;
    private Image hpImage;  

    void Start()
    {
        Monster = GetComponentInParent<Monster>();
        GameObject hpBarObject = GameObject.Find("HPbar"); 
        hpImage = GetComponent<Image>();
        
    }
    
    void Update()
    {
        hpImage.fillAmount = Monster.Cur_HP / Monster.Max_HP;
    }
}
