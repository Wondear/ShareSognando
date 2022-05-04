using System.Collections;
using UnityEngine;

public class FixMonster : MonoBehaviour
{
    //체력 관련은 정수형 (int로 선언. float는 공간차지가 더함)
    // 약자들은 상속, 선언, 매개 변수때 사용할 것
    public int currentHP,fullHP, atkPower;

    //이속 점프높이, 평타쿨(고정), 스킬" ,인식범위 
    protected float moveSpeed, jumpPower, atkCoolTime, skill_Cool,DetectRan;
    protected float AtkRan;
    public float timer = 0.0f;
    public float sktimer = 0.0f;
    //몬스터 상태, 기본 몬스터는 최대 평타 + 스킬 1개를 가지게함 
    protected bool isHit = false;
    protected bool isGround = true;
    public bool canAtk = true;
    public bool canSkill = true;
    protected bool MonsterDirRight , PrevDir;
    protected bool Player=false;
    public bool haveSkill = false;

    //메소드 선언
    public Rigidbody2D rb;
    protected CircleCollider2D circleCollider;
    public BoxCollider2D boxCollider;
    public GameObject healthBar;
    public Animator Anim;
    public LayerMask layerMask;
    public SpriteRenderer spriteRenderer;
    public Transform Moveoffset;
    public Transform[] wallCheck;

    public Vector2 Stop = new Vector2(0, 0);
    protected Vector2 ToPlayer;
    public Bullet bullet;

    public State currentState;
    WaitForSeconds Delay100 = new WaitForSeconds(0.1f);
    //Coroutine Idle;

    public enum State{
        Idle,
        Attack,
        Skill,
        Dead,
    }

    protected virtual void Awake() {
        rb = GetComponent<Rigidbody2D>();
        Anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
        Moveoffset = transform.Find("offset");
        //wallCheck = GetComponentsInChildren<Wallcheck>();
        Debug.Log("catch");
        //StartCoroutine(ResetCollider());
        currentState = State.Idle;
        StartCoroutine("FSM");
        StartCoroutine("damageColor");
       // StartCoroutine(AtCalc(atkCoolTime));
 
        moveSpeed = 3f;
        jumpPower = 15f;
        currentHP = 6;
        atkCoolTime = 3f;
        fullHP = 6;
        atkPower = 1;
        skill_Cool = 10;
        DetectRan = 5;
        AtkRan = 5;
        MonsterDirRight = true;

    }

    protected void Start() { // Awake 보다 후순위, 
        currentHP = fullHP;
        
    }


    // Update is called once per frame
    protected virtual void Update() // :base() 상속될 스크립트에 넣기
    {
        GroundCheck();

        if (Vector2.Distance(transform.position, PlayerData.Instance.Player.transform.position) < DetectRan)
        {
            Player = true;
        }
        else  Player = false;
        currentState = State.Idle;


        if (Physics2D.OverlapCircle(wallCheck[1].position, 0.01f, layerMask))
        {
            MonsterFlip();
        }



        if (!canAtk) {
            timer += Time.deltaTime;
            if (timer >= atkCoolTime) canAtk=ToggleBool(canAtk);
        }

        if (!canSkill & haveSkill) {
            sktimer += Time.deltaTime;
            if (sktimer >= skill_Cool) canSkill= ToggleBool(canSkill);
        }

    }
    

    protected IEnumerator FSM() {
        StartCoroutine(Idle());
        for (; ; ) { 
        State CS = currentState;
        yield return new WaitForSeconds(0.1f);
            if (Player)
            {
                if (Vector2.Distance(transform.position, PlayerData.Instance.Player.transform.position) < AtkRan & canSkill)
                    currentState = State.Skill;
                else
                {
                    currentState = State.Attack;
                }
            }


            if ((currentState != CS && CS == State.Idle))
            {
                StopCoroutine("Idle");
                StartCoroutine(currentState.ToString());
                Anim.SetTrigger(currentState.ToString());

                IsPlayerDir();

                yield return new WaitForSeconds(0.3f);
                //행동 딜레이
            }
            else if (currentState == State.Idle & currentState != CS) {
                StopCoroutine(CS.ToString());
                StartCoroutine("Idle");
            }
            //if (Player == true & transform.position.x- PlayerData.Instance.Player.transform.position.x < 0) {
                
            //}

        }
    }



   //------------------------------------------ 상태별 행동----------------------------------------
    protected virtual IEnumerator Idle()
    {
        //True pl
        yield return null;
    }

    protected virtual IEnumerator Attack()
    {
        yield return null;
    }


    protected virtual IEnumerator Skill()
    {
        yield return null;
    }


    protected IEnumerator Dead()
    {
        Anim.SetTrigger("Dead");
        boxCollider.enabled=false;
        rb.velocity = Stop;
        yield return null;
        Invoke("Die", 4);
    }


    protected IEnumerator damageColor()
    {
        while (currentHP>0) {
            if (isHit) { 
                spriteRenderer.color = new Color(1, 1, 1, 0.4f);
                yield return new WaitForSeconds(0.2f);
                spriteRenderer.color = new Color(1, 1, 1, 1);
                isHit = false;
            }
            yield return null;
        }
    }


    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("playerBullet"))
        { 
            bullet = collision.GetComponent< Bullet> ();
            TakeDamage(bullet.BulletDam);
            
            if (currentHP <= 0)
            {
                StopAllCoroutines();
                StartCoroutine(Dead());
            }

        }
    }

    //데미지가 들어가는 과정을 bullet에 넣는 것이 나을 것 같음 충돌 > 몬스터면 콜라이더 정보 가져옴 > 몬스터의 체력 감소 >   
    public void TakeDamage(int dam)
        {
            currentHP = currentHP - dam;
            isHit = true;

            if (currentHP <= 0)
            {
                StopAllCoroutines();
                spriteRenderer.flipY = true;
                
            }
            rb.AddForce(Vector2.up *5 , ForceMode2D.Impulse);
        }
    public void Die()
    {
        gameObject.SetActive(false);
    }


    protected void GroundCheck()
    {
        if (Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.size, 0, Vector2.down, 0.05f, layerMask))
        {
            isGround = true;
        }
        else
        {
            isGround = false;
        }
    }

    protected bool IsPlayerDir()
    {
        if (transform.position.x < PlayerData.Instance.Player.transform.position.x ? MonsterDirRight : !MonsterDirRight)
        {
            return true;
        }
        else { 
            MonsterFlip();
            return false;
        }
        
    }

    protected void MonsterFlip()
    {
        MonsterDirRight = !MonsterDirRight;

        Vector3 thisScale = transform.localScale;
        if (MonsterDirRight)
        {
            thisScale.x = -Mathf.Abs(thisScale.x);
        }
        else
        {
            thisScale.x = Mathf.Abs(thisScale.x);
        }
        transform.localScale = thisScale;
        rb.velocity = Vector2.zero;
    }

    protected float GetLan(float start, float end)
    {
        float Number = Random.Range(start, end);

        return Number;
    }

    protected int GetLan(int start, int end)
    {
        int Number = Random.Range(start, end);

        return Number;
    }

    protected bool GetLan()
    {
        bool randBool = (Random.value > 0.5f);
        return randBool;
    }

    protected bool ToggleBool(bool target) {
        return !target;
    }

}
