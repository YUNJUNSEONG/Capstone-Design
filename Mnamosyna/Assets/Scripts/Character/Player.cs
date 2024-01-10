using NUnit.Framework.Interfaces;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player instance;
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
    public int MIN_DMG = 15;
    public int MAX_DMG = 20;
    public float DEF = 0;
    public float Crit_Chance = 0;
    public float Critical = 1.5f;
    public float ATK_Speed = 1.0f;
    public float Move_Speed = 10.0f;

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
        Cur_HP = Max_HP;
        Cur_Stamina = Max_Stamina;
    }

    void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rigid = GetComponent<Rigidbody>();
        sword = GetComponentInChildren<Sword>();
        instance = this;
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
            transform.position += moveVec * Move_Speed *  Time.deltaTime;
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
        isAttackReady = ATK_Speed < attackDelay;

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
