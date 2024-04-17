//using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class PlayerHPBar : MonoBehaviour
{
    public PlayerStat stat;
    public Image hpImage; 
    public Image stImage;
    private static readonly int FillLevel = Shader.PropertyToID("_FillLevel");


    void Update()
    {
        UpdateHPValue((float)stat.Cur_HP / stat.Max_HP);
        UpdateStaminaValue(((float)stat.Cur_Stamina / stat.Max_Stamina));
    }

    void UpdateHPValue(float value)
    {
        if (hpImage == null)
        {
            return;
        }
        hpImage.material.SetFloat(FillLevel, value);
    }

    void UpdateStaminaValue(float value)
    {
        if (stImage == null)
        {
            return;
        }
        stImage.material.SetFloat(FillLevel, value);
    }
}
