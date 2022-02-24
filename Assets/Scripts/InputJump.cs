using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputJump : Trap
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    void Awake()
    {
        atkDelay = 0.3f;
        StartCoroutine("FSM");
        hitBoxCollider.SetActive(false);

    }

    protected override IEnumerator Attack()
    {
        yield return new WaitForSeconds(atkDelay);

            Anim.SetTrigger("Attack");
            hitBoxCollider.SetActive(true);

            yield return new WaitForSeconds(atkDelay);

        AttackOver();
        yield return null;
    }


    // Update is called once per frame
    void Update()
    {
        if (player == true && Input.GetKeyDown("w")) {
            currentState = State.Attack;
        }
        
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player = true;
        }
    }
}
