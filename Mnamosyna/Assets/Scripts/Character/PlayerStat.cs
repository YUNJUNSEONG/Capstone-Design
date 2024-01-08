using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : MonoBehaviour
{
    // 체력 관련 속성
    public int Max_HP = 150;
    public int Cur_HP;
    public float HP_RecoverRate = 0; // 1초 당 n 회복

    // 스테미나 관련 속성
    public int Max_Stamina = 200;
    public int Cur_Stamina;
    public int StaminaCostPerSkill = 20;
    public float Stamina_RecoverRate = 3;

    // 공격 관련 속성
    public int MIN_ATK = 18;
    public int MAX_ATK = 22;
    public float DEF = 0;
    public float Crit_Chance = 0;
    public float Critical = 1.5f;
    public float ATK_Speed = 1.0f;
    public float Move_Speed = 1.0f;
    // Start is called before the first frame update
    void Start()
    {
        Cur_HP = Max_HP;
        Cur_Stamina = Max_Stamina;
    }

    // Update is called once per frame
    void Update()
    {
        // 체력 자동 회복
        if (Cur_HP > 0 && Cur_HP < Max_HP)
        {
            Cur_HP += Mathf.RoundToInt(HP_RecoverRate * Time.deltaTime);
            Cur_HP = Mathf.Clamp(Cur_HP, 0, Max_HP);

            if (Cur_HP == 0)
            {
                // 사망 모션 출력
                Die();
            }
        }

        // 스테미나 자동 회복
        if (Cur_Stamina < Max_Stamina)
        {
            Cur_Stamina += Mathf.RoundToInt(Stamina_RecoverRate * Time.deltaTime);
            Cur_Stamina = Mathf.Clamp(Cur_Stamina, 0, Max_Stamina);
        }
    }

    void Die()
    {
        // 사망 처리
        // 예: 게임 종료 또는 리스폰 등
    }

    public void TakeDamage(int damage)
    {
        // 피해 처리
        int finalDamage = Mathf.RoundToInt(damage * (1 - DEF)); // 피해 감소 적용
        Cur_HP = Mathf.Max(0, Cur_HP - finalDamage);

        if (Cur_HP == 0)
        {
            Die();
        }
    }

    public void UseSkill()
    {
        // 스킬 사용 처리
        if (Cur_Stamina >= StaminaCostPerSkill)
        {
            Cur_Stamina -= StaminaCostPerSkill;

            // 스킬 실행 코드 추가
        }
        else
        {
            // 스테미나 부족 처리
            Debug.Log("Not enough stamina to use the skill!");
        }
    }
}

