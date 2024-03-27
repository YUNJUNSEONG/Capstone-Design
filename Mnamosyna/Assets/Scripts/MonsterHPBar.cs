using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterHPBar : MonoBehaviour
{
    //private GameObject mainCameraObject;
    //public Transform cameraTransform;
    public GameObject monster;
    private MeleeMonster meleeMonster;
    private Image hpImage;  

    void Start()
    {
        //mainCameraObject = GameObject.Find("Main Camera");
        meleeMonster = GetComponentInParent<MeleeMonster>();
        GameObject hpBarObject = GameObject.Find("HPbar"); 
        hpImage = GetComponent<Image>();
        
    }
    
    void Update()
    {
        if(meleeMonster.CurrentHP > 0)
        {
            hpImage.fillAmount = meleeMonster.CurrentHP / meleeMonster.MaxHP;
            //cameraTransform = mainCameraObject.transform;
            //transform.LookAt(transform.position + cameraTransform.rotation * Vector3.forward, cameraTransform.rotation * Vector3.up);
        }
    }
}
