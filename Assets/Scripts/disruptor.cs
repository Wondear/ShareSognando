using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class disruptor : MonoBehaviour
{
    static public bool Broken = true;

    public float currentHP = 1;
    public float fullHP = 1;

    public bool isHit = false;
    public bool isGround = true;
    public bool canAtk = true;
    public bool MonsterDirRight;
    public bool Player;
    public Rigidbody2D rigid;

    protected Rigidbody2D rb;
    protected BoxCollider2D boxCollider;
    public GameObject hitBoxCollider;
    public Animator Anim;
    public LayerMask layerMask;
    public SpriteRenderer spriteRenderer;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Awake() { 
        //this.SetActive(Broken);
    }

    public void TakeDamage(int dam)
    {
        Debug.Log("Damaged");
        currentHP -= (float)dam;
        Debug.Log(currentHP);
        isHit = true;
        
        rigid.AddForce(Vector2.up * 5, ForceMode2D.Impulse);
        if (currentHP <= 0)
        { 
            hitBoxCollider.SetActive(false);
            Anim.SetTrigger("Die");
            spriteRenderer.flipY = true;
            Invoke("Die", 2);
        }
    }
    public void Die()
    {
        gameObject.SetActive(false);
    }
}
