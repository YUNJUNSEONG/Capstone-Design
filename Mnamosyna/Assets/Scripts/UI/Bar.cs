using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bar : MonoBehaviour
{
    public PlayerStat stat;
    public Slider hpSlider; // Inspector â���� �����̴��� �����մϴ�.
    public Slider staminaSlider;

    private float currentHP;
    private float currentStamina;

    void Start()
    {
        // �ʱ� HP ����
        currentHP = stat.max_hp;
        currentStamina = stat.max_stamina;
        UpdateHPBar();
        UpdateStaminaBar();
    }

    void UpdateHPBar()
    {
        // �����̴��� ���� ���� HP ������ �°� ����
        hpSlider.value = currentHP / stat.max_hp;
    }

    void UpdateStaminaBar()
    {
        staminaSlider.value = currentStamina / stat.max_stamina;
    }

    // HP ���� ����
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
