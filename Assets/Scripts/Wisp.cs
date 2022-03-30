using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wisp : Monster // 1회성 몬스터, 한 번 충돌 후 터짐------------------------------
{
    public enum State // 상태 대기, 공격 , 사망
    {
        Idle,
        Attack,
        Dead
    };

    //public GameObject hitBox;
    public State currentState;
    public Transform[] wallCheck; // 벽 통과를 못 한다면 충돌체와 함께 필요 
    public Vector2 toPlayer;
    WaitForSeconds Delay250 = new WaitForSeconds(0.25f);
    WaitForSeconds Delay1500 = new WaitForSeconds(1.5f);

    Vector2 boxColliderOffset;

    void Awake()
    {

        base.Awake();
        moveSpeed = 0.5f;
        jumpPower = 0.2f;
        currentHP = 1;
        fullHP = 1;
        atkPower = 1;
        atkCoolTime = 1f;
        currentState = State.Idle;


    }



    IEnumerator Idle()
    {
        Anim.SetTrigger("Idle");
        rigid.velocity = new Vector2(0, transform.localScale.y * (moveSpeed));
        yield return Delay1500;
        rb.velocity = new Vector2(0, -transform.localScale.y * (moveSpeed));
        yield return Delay1500;
        rigid.velocity = Stop;
        yield return Delay250;//break;
        yield break;

    }

    IEnumerator Attack() { // 플레이어 쪽으로 이동------------------------
        Anim.SetTrigger("Attack");
        
        rb.velocity = new Vector2(moveSpeed * (PlayerData.Instance.Player.transform.position.x - transform.position.x), moveSpeed * (PlayerData.Instance.Player.transform.position.y - transform.position.y)).normalized;
        yield return null;
    }
   

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update(){
        StartCoroutine("FSM");
        
    }

    void FixedUpdate() { 
        if (Vector2.Distance(transform.position, PlayerData.Instance.Player.transform.position) <= 10f)
            {
                currentState = State.Attack;
                StopCoroutine("Idle");
            }

        else {
            currentState = State.Idle;
            StopCoroutine("Attack");         
        }

        if (currentHP > 0) { 
        toPlayer = new Vector2(moveSpeed*( PlayerData.Instance.Player.transform.position.x- transform.position.x) , moveSpeed * (PlayerData.Instance.Player.transform.position.y - transform.position.y));
        }
    }

    private void OnTriggerStay2D(Collider2D collision) {
        if (collision.gameObject.tag == "Player")
        {            
            StopCoroutine("FSM");
            StopAllCoroutines();  
            rigid.velocity = Stop;
            hitBoxCollider.SetActive(false);
            Anim.SetTrigger("Die");
            boxCollider.enabled = false;
            
            //hitBox.GetComponent<CircleCollider2D>().enabled = false;
            spriteRenderer.flipY = true;
            Invoke("die", 4);
        }
    }
    IEnumerator FSM()
    {
        for (; ; )
        {
            yield return StartCoroutine(currentState.ToString());
        }

    }

}
