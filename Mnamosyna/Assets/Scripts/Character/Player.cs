using NUnit.Framework.Interfaces;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    float hAxis;
    float vAxis;
    bool leftDown;
    bool rightDown;
    bool isBorder;

    public Camera followCamera;
    bool isAttackReady;

    Vector3 moveVec;
    Animator anim;
    Rigidbody rigid;
    PlayerStat stat;
    Sword sword;

    float attackDelay;
    void Start()
    {
        stat.Cur_HP = stat.Max_HP;
        stat.Cur_Stamina = stat.Max_Stamina;
    }

    void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rigid = GetComponent<Rigidbody>();
        sword = GetComponentInChildren<Sword>();
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        Move();
        LeftAttack();
        //RightAttack();
    }

    void GetInput()
    {
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
        leftDown = Input.GetButtonDown("Fire1");
        //rightDown = Input.GetButtonDown("Fire3");
    }

    void Move()
    {
        moveVec = new Vector3(hAxis, 0, vAxis).normalized;

        if (leftDown || rightDown)
        {
            moveVec = Vector3.zero;
        }

        if (!isBorder)
        {
            transform.position += moveVec * stat.Move_Speed *  Time.deltaTime;
        }

        anim.SetBool("isRun", moveVec != Vector3.zero);


        //키보드 회전
        transform.LookAt(transform.position + moveVec);
        // 마우스 회전
        /* Ray ray = followCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit rayHit;
        if(Physics.Raycast(ray, out rayHit, 100))
        {
            Vector3 nextVec = rayHit.point - transform.position;
            nextVec.y = 0;
            transform.LookAt(transform.position + nextVec);
        } */
    }

    void FixedUpdate()
    {
        FreezeRotation();
        stopToWall();
    }
    void FreezeRotation()
    {
        rigid.angularVelocity = Vector3.zero;
    }
    void stopToWall()
    {
        Debug.DrawRay(transform.position, transform.forward * 5, Color.white);
        isBorder = Physics.Raycast(transform.position, transform.forward, 5, LayerMask.GetMask("Wall"));
    }
    void LeftAttack()
    {

        attackDelay += Time.deltaTime;
        isAttackReady = stat.ATK_Speed < attackDelay;

        if (leftDown && isAttackReady)
        {
            sword.Use();
            anim.SetTrigger("LeftAttack");
            attackDelay = 0;
        }
    }

    /*void RightAttack()
    {
        attackDelay += Time.deltaTime;
        isAttackReady = ATK_Speed < attackDelay;

        if(rightDown && isAttackReady)
        {
            sword.Use();
            anim.SetTrigger("RightAttack");
            attackDelay = 0;
        }
    }*/
}
