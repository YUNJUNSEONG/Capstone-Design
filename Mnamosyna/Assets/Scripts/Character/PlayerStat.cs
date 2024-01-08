using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : MonoBehaviour
{
    // ü�� ���� �Ӽ�
    public int Max_HP = 150;
    public int Cur_HP;
    public float HP_RecoverRate = 0; // 1�� �� n ȸ��

    // ���׹̳� ���� �Ӽ�
    public int Max_Stamina = 200;
    public int Cur_Stamina;
    public int StaminaCostPerSkill = 20;
    public float Stamina_RecoverRate = 3;

    // ���� ���� �Ӽ�
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
        // ü�� �ڵ� ȸ��
        if (Cur_HP > 0 && Cur_HP < Max_HP)
        {
            Cur_HP += Mathf.RoundToInt(HP_RecoverRate * Time.deltaTime);
            Cur_HP = Mathf.Clamp(Cur_HP, 0, Max_HP);

            if (Cur_HP == 0)
            {
                // ��� ��� ���
                Die();
            }
        }

        // ���׹̳� �ڵ� ȸ��
        if (Cur_Stamina < Max_Stamina)
        {
            Cur_Stamina += Mathf.RoundToInt(Stamina_RecoverRate * Time.deltaTime);
            Cur_Stamina = Mathf.Clamp(Cur_Stamina, 0, Max_Stamina);
        }
    }

    void Die()
    {
        // ��� ó��
        // ��: ���� ���� �Ǵ� ������ ��
    }

    public void TakeDamage(int damage)
    {
        // ���� ó��
        int finalDamage = Mathf.RoundToInt(damage * (1 - DEF)); // ���� ���� ����
        Cur_HP = Mathf.Max(0, Cur_HP - finalDamage);

        if (Cur_HP == 0)
        {
            Die();
        }
    }

    public void UseSkill()
    {
        // ��ų ��� ó��
        if (Cur_Stamina >= StaminaCostPerSkill)
        {
            Cur_Stamina -= StaminaCostPerSkill;

            // ��ų ���� �ڵ� �߰�
        }
        else
        {
            // ���׹̳� ���� ó��
            Debug.Log("Not enough stamina to use the skill!");
        }
    }
}

