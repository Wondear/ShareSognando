using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour // �ߵ��� ����(����)
{
    public GameObject hitBoxCollider;
    public Animator Anim;
    public LayerMask layerMask;
    public SpriteRenderer spriteRenderer;
    public Rigidbody2D rigid;

    public bool player = false;
    public bool isHit = false;
    public State currentState = State.Idle;

    protected float atkDelay = 1.0f; //�÷��̾ �νĵǰ� ������ �ߵ��� �� ������ �ð� 
    protected float atking = 1.0f; // ���� ��Ʈ�ڽ� ���� �ð�

    public enum State
    {
        Idle,
        Attack,
    };

    // Start is called before the first frame update
    void Start()
    {
        Anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        hitBoxCollider.SetActive(false);
    }

    // Update is called once per frame
    void Update() {
    }

    protected void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            player = true;
            currentState = State.Attack;
            //Invoke("Attack", atkDelay);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player = false;
        }
    }




    protected IEnumerator FSM()
    {
        while (true)
        {
            yield return StartCoroutine(currentState.ToString());
        }
    }

    protected IEnumerator Idle(){
        
        yield return null;
    }



    protected virtual IEnumerator Attack(){
        Anim.SetTrigger("Player");
        hitBoxCollider.SetActive(true);

        if (player == true) {
            yield return new WaitForSeconds(1f);
        }

        AttackOver();
        currentState = State.Idle;
        yield return null;
        
    }

    protected void AttackOver() {
        Anim.SetTrigger("AttackOver");
        currentState = State.Idle;
        hitBoxCollider.SetActive(false);
    }
}
