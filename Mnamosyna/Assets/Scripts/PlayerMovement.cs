using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public PlayerStat stat;
    float hAxis;
    float vAxis;
    bool isBorder;
    Vector3 moveVec;
    Animator anim;
    Rigidbody rigid;
    Camera followCamera;
    
    //공격받을때 깜빡이는 용도
    public float flashDuration = 0.1f;
    public int flashCount = 6;
    private List<Renderer> renderers;

    public void Awake()
    {   
        anim = GetComponentInChildren<Animator>();
        rigid = GetComponent<Rigidbody>();
        stat = GetComponent<PlayerStat>();
    }
    
    private void Start()
    {
        renderers = new List<Renderer>(GetComponentsInChildren<Renderer>());
    }

    public void Update()
    {
        GetInput();
        Move();
    }
    
    void GetInput()
    {
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
    }

    void Move()
    {
        Vector3 inputDirection = new Vector3(hAxis, 0, vAxis);
        inputDirection = Camera.main.transform.TransformDirection(inputDirection);
        inputDirection.y = 0;
        moveVec = inputDirection.normalized;

        if( !isBorder)
        {
            transform.position += moveVec * (stat.move_speed * Time.deltaTime);
        }

        anim.SetBool("isRun", moveVec != Vector3.zero);

        if(moveVec != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(moveVec);
        }
    }
    
    public void GetHit()
    {
        anim.SetTrigger("GetHit");
        Flash();
        var playerAttack = GetComponent<PlayerAttack>();
        if (playerAttack != null)
        {
            playerAttack.DisableSwordCollider();
        }
        else{Debug.Log("isattacking변경실패");}
    }
    
    public void Flash()
    {
        StartCoroutine(DoFlash());
    }
    
    private IEnumerator DoFlash()
    {
        for (int i = 0; i < flashCount; i++)
        {
            foreach (Renderer renderer in renderers)
            {
                renderer.enabled = false; 
            }

            yield return new WaitForSeconds(flashDuration);

            foreach (Renderer renderer in renderers)
            {
                renderer.enabled = true;
            }

            yield return new WaitForSeconds(flashDuration);
        }
    }
    
    
}
