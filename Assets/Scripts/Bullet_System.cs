using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_System : MonoBehaviour
{
    private Rigidbody2D rb;
    public float Speed = 0;
    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        Destroy(gameObject, 5f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.AddForce(Vector2.left * Speed, ForceMode2D.Impulse);
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.tag != "Player")
        {
            Destroy(gameObject);
        }
    }
}
