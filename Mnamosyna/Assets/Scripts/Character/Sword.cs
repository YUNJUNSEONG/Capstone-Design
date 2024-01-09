using System.Collections;
using UnityEngine;

public class Sword : MonoBehaviour
{
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
        yield return new WaitForSeconds(0.5f);
        attackArea.enabled = true;
        trailEffect.enabled = true;

        yield return new WaitForSeconds(0.8f);
        attackArea.enabled = false;

        yield return new WaitForSeconds(0.8f);
        trailEffect.enabled = false;
    }
}
