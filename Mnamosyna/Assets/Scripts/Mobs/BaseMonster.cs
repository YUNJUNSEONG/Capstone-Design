using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class BaseMonster : MobStat
{
    public enum State
    {
        Chase,
        Attack,
        Die
    }
    protected State currentState;

    public float invincibleTime = 1f; // ���ݹ��� �� ���� �ð�
    private float lastDamagedTime;
    public Spawner spawner;
    public delegate void DeathHandler();
    public event DeathHandler OnDeath;
    public GameObject exclamationMark;

    public Text damageText;
    private Vector3 originalPosition;
    private Color originalColor;

    // ù ��° ���� �ִϸ��̼��� ���� (�� ����)
    public float firstAttackAnimationLength;

    // �� ��° ���� �ִϸ��̼��� ���� (�� ����)
    public float secondAttackAnimationLength;

    protected GameObject player;
    protected float switchTime = 2.0f;

    protected Rigidbody rigid; // ������ �ٵ�
    protected NavMeshAgent nav;
    protected System.Random random;
    protected Animator anim;

    protected const float WAIT_TIME = 0.2f;
    public bool isAttack = false;
    public bool isSkill = false;
    protected bool isChase;
    protected bool isDamage = false;
    protected bool isDead = false;
    protected float Skill1CanUse = 0;
    protected float Skill2CanUse = 0;

    // �ִϸ��̼ǿ�
    protected static readonly int BattleIdleHash = Animator.StringToHash("BattleIdle");
    protected static readonly int Attack01Hash = Animator.StringToHash("Attack01");
    protected static readonly int Attack02Hash = Animator.StringToHash("Attack02");
    protected static readonly int Attack03Hash = Animator.StringToHash("Attack03");
    protected static readonly int RunHash = Animator.StringToHash("Run");
    protected static readonly int GetHitHash = Animator.StringToHash("GetHit");
    protected static readonly int DieHash = Animator.StringToHash("Die");

    // ���ݹ��� �� �����̴� �뵵
    private float flashDuration = 0.1f;
    private int flashCount = 2;
    private List<Renderer> renderers;

    protected virtual void Awake()
    {
        isAttack = false;
        player = GameObject.FindWithTag("Player");
        rigid = GetComponent<Rigidbody>();
        nav = GetComponent<NavMeshAgent>();
        nav.stoppingDistance = distance;
        random = new System.Random();
        exclamationMark.SetActive(false);
        anim = GetComponent<Animator>();
        renderers = new List<Renderer>(GetComponentsInChildren<Renderer>());
        damageText.gameObject.SetActive(false);
        originalPosition = damageText.transform.position;
        originalColor = damageText.color;
    }

}

