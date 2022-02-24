using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpSquare : Trap
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Awake() {
        atkDelay = 1f;
        StartCoroutine("FSM");
        hitBoxCollider.SetActive(false);

    }

    protected override IEnumerator Attack() {
        Anim.SetBool("Player",true);
        yield return new WaitForSeconds(atkDelay);

        if (player == true)
        {
            Anim.SetTrigger("Attack");
            hitBoxCollider.SetActive(true);
            
            /*if (hitBoxCollider. )
            {
                Player_rb.AddForce(new Vector2(X_power, 1) * 10, ForceMode2D.Impulse);
            }*/
            yield return new WaitForSeconds(atkDelay);
        }

        AttackOver();
        yield return new WaitForSeconds(2f);
        yield return null;
    }

    // Update is called once per frame
    void Update()
    {
        if (Anim.GetBool("Player") && player == false ) {
            Anim.SetBool("Player", false);
            StopCoroutine("Attack");
        }
    }

}
