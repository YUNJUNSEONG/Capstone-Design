using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q)) animator.SetTrigger("attack01");
        if (Input.GetKeyDown(KeyCode.W)) animator.SetTrigger("attack02");
        if (Input.GetKeyDown(KeyCode.E)) animator.SetTrigger("attack03");
        if (Input.GetKeyDown(KeyCode.R)) animator.SetTrigger("attack04");
        if (Input.GetKeyDown(KeyCode.T)) animator.SetTrigger("attack05");
        if (Input.GetKeyDown(KeyCode.Y)) animator.SetTrigger("attack06");
        if (Input.GetKeyDown(KeyCode.U)) animator.SetTrigger("attack07");
        if (Input.GetKeyDown(KeyCode.I)) animator.SetTrigger("attack08");
        if (Input.GetKeyDown(KeyCode.O)) animator.SetTrigger("attack09");
        if (Input.GetKeyDown(KeyCode.P)) animator.SetTrigger("attack10");
    }
}
