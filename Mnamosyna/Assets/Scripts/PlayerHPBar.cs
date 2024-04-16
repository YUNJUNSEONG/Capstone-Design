//using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHPBar : MonoBehaviour
{
    public PlayerStat stat;
    public Image hpImage; 
    public Image stImage;
    
    void Update()
    {
        hpImage.fillAmount = (float)stat.Cur_HP / stat.Max_HP;
        stImage.fillAmount = ((float)stat.Cur_Stamina / stat.Max_Stamina);
    }
}
