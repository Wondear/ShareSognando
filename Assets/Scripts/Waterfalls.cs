using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waterfalls : MonoBehaviour
{
    public PlayerMove PScript ;
    public Rigidbody2D Player_rb;
    

    // Start is called before the first frame update
    void Awake()
    {
       //PScript = GameObject.Find("Player").GetComponent<PlayerMove>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PScript = collision.GetComponent<PlayerMove>(); 
            //���� �ݶ��̴��� ��ũ��Ʈ�� �������� 
            PScript.maxSpeed = 5.4f;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player_rb.AddForce(new Vector2(0, -43));
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PScript.maxSpeed = 7;
            PScript = null;

        }
    }
}
