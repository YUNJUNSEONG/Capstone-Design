using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterHPBar : MonoBehaviour
{
    public GameObject monster;
    private MeleeMonster meleeMonster;
    private Image hpImage;  

    void Start()
    {
        meleeMonster = GetComponentInParent<MeleeMonster>();
        GameObject hpBarObject = GameObject.Find("HPbar"); 
        hpImage = GetComponent<Image>();
        
    }
    
    void Update()
    {
        hpImage.fillAmount = meleeMonster.CurrentHP / meleeMonster.MaxHP;
    }
}
