using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wisp : Monster // 1ȸ�� ����, �� �� �浹 �� ����------------------------------
{
    public enum State // ���� ���, ���� , ���
    {
        Idle,
        Attack,
        Dead
    };

    //public GameObject hitBox;
    bool isdie;
    public State currentState = State.Idle;
    public Transform[] wallCheck; // �� ����� �� �Ѵٸ� �浹ü�� �Բ� �ʿ� 
    public Vector2 toPlayer;
    WaitForSeconds Delay250 = new WaitForSeconds(0.25f);
    WaitForSeconds Delay1500 = new WaitForSeconds(1.5f);

    Vector2 boxColliderOffset;

    void Awake()
    {
       
        isdie = false;
        base.Awake();
        moveSpeed = 0.2f;
        jumpPower = 0.2f;
        currentHP = 1;
        fullHP = 1;
        atkPower = 1;
        atkCoolTime = 1f;
        atkCoolTimeCalc = atkCoolTime;
        StartCoroutine(FSM());

    }

    IEnumerator FSM()
    {
            while (true)
            {
                yield return StartCoroutine(currentState.ToString());
            }
 
    }

    IEnumerator Idle()
    {
        boxCollider.offset = boxColliderOffset;
        rb.velocity = new Vector2(rb.velocity.x, transform.localScale.y * (moveSpeed));
        yield return Delay1500;
        rb.velocity = new Vector2(rb.velocity.x, -transform.localScale.y * (moveSpeed));
        yield return Delay1500;
        rb.velocity = Stop;

        if (Vector2.Distance(transform.position, PlayerData.Instance.Player.transform.position) < 10f){
            Anim.SetTrigger("Attack");
            currentState = State.Attack; 
                
        }
        yield return new WaitForSeconds(0.5f);    //break;
            
        
    }

    IEnumerator Attack() { // �÷��̾� ������ �̵�------------------------

        toPlayer = new Vector2((PlayerData.Instance.Player.transform.position.x- transform.position.x) * moveSpeed,(PlayerData.Instance.Player.transform.position.y - transform.position.y)*moveSpeed);
        rb.velocity = toPlayer;
        if (Vector2.Distance(transform.position, PlayerData.Instance.Player.transform.position) > 10f){
            Anim.SetTrigger("Idle");
            currentState = State.Idle;
            rb.velocity = Stop;
        }
        yield return null;
    }
   

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update(){
        /*GroundCheck();
        if (!isHit && isGround && !IsPlayingAnim("Move"))
        {
            boxCollider.offset = boxColliderOffset;
            MyAnimSetTrigger("Idle");
        }*/


    }
    private void OnTriggerStay2D(Collider2D collision) {
        if (collision.gameObject.tag == "Player" ){
            Anim.SetTrigger("Die");
            boxCollider.enabled = false;
            hitBoxCollider.SetActive(false);
            //hitBox.GetComponent<CircleCollider2D>().enabled = false;
            spriteRenderer.flipY = true;
            Invoke("die", 1);
        }
    } 
}
