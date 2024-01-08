//using Base.Mobs.Entity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    public BoxCollider attackArea;

    [SerializeField]
    private float _atk;
    public float Atk
    {
        get { return _atk; }
    }

    public void Use(float attackTime, float Atk)
    {
        _atk = Atk;
        //StartCoroutine(Attack(attackTime, Atk));
    }

    IEnumerator Attack(float attackTime, float Atk)
    {
        _atk = Atk;
        yield return new WaitForSeconds(0.5f);
        attackArea.enabled = true;
        yield return new WaitForSeconds(attackTime - 0.5f);
        attackArea.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 8)
        {
            Debug.Log("Attack");
           // other.gameObject.GetComponent<IDamagable>().DamagedEntity(_atk);
        }
    }
}