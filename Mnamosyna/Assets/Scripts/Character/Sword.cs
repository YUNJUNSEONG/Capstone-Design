using System.Collections;
using UnityEngine;

public class Sword : MonoBehaviour
{
    public PlayerStat stat;
    public BoxCollider attackArea;
    public TrailRenderer trailEffect;

    Coroutine attackCoroutine;

    public void Use()
    {
        if (attackCoroutine != null)
        {
            StopCoroutine(attackCoroutine);
        }

        attackCoroutine = StartCoroutine(Attack());
    }

    IEnumerator Attack()
    {
        yield return new WaitForSeconds(0.1f);
        attackArea.enabled = true;
        trailEffect.enabled = true;

        yield return new WaitForSeconds(1f);
        attackArea.enabled = false;
        trailEffect.enabled = false;
    }
}
