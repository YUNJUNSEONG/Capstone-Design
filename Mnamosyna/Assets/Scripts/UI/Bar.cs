using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bar : MonoBehaviour
{
    public PlayerStat stat;
    public Slider hpSlider; // Inspector 창에서 슬라이더를 연결합니다.
    public Slider staminaSlider;

    private float currentHP;
    private float currentStamina;

    void Start()
    {
        // 초기 HP 설정
        currentHP = stat.max_hp;
        currentStamina = stat.max_stamina;
        UpdateHPBar();
        UpdateStaminaBar();
    }

    void UpdateHPBar()
    {
        // 슬라이더의 값을 현재 HP 비율에 맞게 설정
        hpSlider.value = currentHP / stat.max_hp;
    }

    void UpdateStaminaBar()
    {
        staminaSlider.value = currentStamina / stat.max_stamina;
    }

    // HP 감소 예시
    void DecreaseHP(float damage)
    {
        currentHP -= damage;
        UpdateHPBar();
    }

    void DecreaseStamina(float cost)
    {
        currentStamina -= cost;
        UpdateStaminaBar();
    }

}
