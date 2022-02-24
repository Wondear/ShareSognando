using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump_Ac : MonoBehaviour
{
    public Rigidbody2D Player_rb;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player_rb.AddForce(new Vector2(0, 30), ForceMode2D.Impulse);
        }
    }
}
