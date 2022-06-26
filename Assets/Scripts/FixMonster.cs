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
    protected bool canAtk = true;
    protected bool canSkill = true;
    protected bool MonsterDirRight , PrevDir;
    protected bool Player=false;
    protected bool haveSkill = false;
    protected bool Corouting=false;
    protected bool isDie =false;


    public bool sival=false;

    //메소드 선언
    public Rigidbody2D rb;
    protected CircleCollider2D circleCollider;
    public BoxCollider2D boxCollider;
    public GameObject healthBar;
    public Animator Anim;
    public LayerMask layerMask;
    public SpriteRenderer spriteRenderer;
    public Transform Moveoffset;
    public Transform[] wallCheck; //index 0은 flip, 1은 스킬 안되는 조건
    // 이외에는 자유

    public Vector2 Stop = new Vector2(0, 0);
    protected Vector2 ToPlayer;
    public Bullet bullet;

    public State currentState;
    public WaitForSeconds Delay100 = new WaitForSeconds(0.1f);
    //Coroutine Idle;

    public enum State{
        Idle,
        Move,
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
        //StartCoroutine(ResetCollider());
        currentState = State.Idle;
        StartCoroutine("FSM");
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


        if (Physics2D.OverlapCircle(wallCheck[0].position, 0.01f, layerMask))
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
    ToPlayer = new Vector2((PlayerData.Instance.Player.transform.position.x - transform.position.x)*100, 
        (PlayerData.Instance.Player.transform.position.y - transform.position.y)*100).normalized * moveSpeed;
    }
    

    protected IEnumerator FSM() {
        for (; ; ) {
            yield return null;
            if (!Corouting) {
        State CS = currentState;
        yield return new WaitForSeconds(0.1f);
                if (Player)
                {
                    if (Physics2D.OverlapCircle (wallCheck[1].position, 0.01f, layerMask)) {
                        currentState = State.Idle;
                    }
                    else if (canSkill && Vector2.Distance(transform.position, PlayerData.Instance.Player.transform.position) < AtkRan )
                        currentState = State.Skill;
                    else
                    {
                        currentState = State.Attack;
                    }
                }
                else { 
                    currentState = State.Idle; }
                    
                Corouting = ToggleBool(Corouting);
                StartCoroutine(currentState.ToString());
                Anim.SetTrigger(currentState.ToString());
                if (currentState != State.Idle) { 
                    IsPlayerDir();
                }
                //행동 딜레이
            }
            //if (Player == true & transform.position.x- PlayerData.Instance.Player.transform.position.x < 0) {

            //}

        }
    }


    protected IEnumerator Dead()
    {
        isDie = ToggleBool(isDie);
        Anim.SetTrigger("Dead");
        boxCollider.enabled=false;
        rb.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
        for (int i = 10; i > 0;  i--) {
            spriteRenderer.color = new Color(1, 1, 1, (float)i/10);
            yield return new WaitForSeconds(0.1f);
        }
        rb.velocity = Stop;
        yield return null;
        Invoke("Die", 1);
    }


    protected IEnumerator damageColor(){
                spriteRenderer.color = new Color(1, 1, 1, 0.4f);
                yield return new WaitForSeconds(0.2f);
                isHit = ToggleBool(isHit);
                spriteRenderer.color = new Color(1, 1, 1, 1);
        
            yield return null;
   
    }


    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("playerBullet"))
        { 
            bullet = collision.GetComponent< Bullet> ();
            TakeDamage(bullet.BulletDam);
            
        }
    }

    //데미지가 들어가는 과정을 bullet에 넣는 것이 나을 것 같음 충돌 > 몬스터면 콜라이더 정보 가져옴 > 몬스터의 체력 감소 >   
    public void TakeDamage(int dam){   
        isHit = true;
        currentHP = currentHP - dam;

        if (currentHP <= 0 & !isDie)
        {
            rb.AddForce(Vector2.up * 5, ForceMode2D.Impulse);
            StopAllCoroutines();
            StartCoroutine(Dead());
        }
        else if (currentHP>0){
            StartCoroutine("damageColor");
            rb.AddForce(Vector2.up * 5, ForceMode2D.Impulse);
        }
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

    //------------------------------------------ 상태별 행동----------------------------------------
    protected virtual IEnumerator Idle()
    {
        rb.velocity = Stop;
        //True pl
        yield return null;
    }

    protected virtual IEnumerator Move()
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

//랜덤값 획득
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

    protected bool GetLan(float TruePer)
    {
        bool randBool = (Random.value > TruePer);
        return randBool;
    }

    protected bool ToggleBool(bool target) {
        return !target;
    }

}
