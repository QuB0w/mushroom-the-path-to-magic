using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Enemy_System : MonoBehaviour
{
    private Rigidbody2D rb;
    private GameObject Hero;

    private bool isEnable = true;
    private bool isAnim = true;
    private bool isDead = false;

    private bool isCanAttack = true;
    private bool stop = false;

    public AudioClip hit;
    public AudioClip death;

    private AudioSource audioSource;

    [Header("Stats")]
    public int HP = 2;
    public float Speed = 1f;
    public string enemyName = "";
    public float viewDistance_x = 5f;
    public float viewDistance_y = 5f;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GameObject.Find("Main Camera").GetComponent<AudioSource>();

        rb = gameObject.GetComponent<Rigidbody2D>();
        Hero = GameObject.Find("Mushroom");
    }

    // Update is called once per frame
    void Update()
    {
        if (Hero.GetComponent<Hero_System>().HP <= 0)
        {
            isDead = true;
        }

        if (HP <= 0)
        {
            //audioSource.PlayOneShot(death);
            isDead = true;
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
            gameObject.GetComponent<Rigidbody2D>().gravityScale = 0f;
            isEnable = false;
            if(enemyName == "Slime")
            {
                gameObject.GetComponent<Animator>().Play("slime_death");
            }
            else if(enemyName == "Treant")
            {
                gameObject.GetComponent<Animator>().Play("trent_death");
            }
            Destroy(gameObject, 1f);
        }

        if(Hero.transform.position.x <= gameObject.transform.position.x + viewDistance_x && Hero.transform.position.y <= gameObject.transform.position.y + viewDistance_y && Hero.transform.position.x >= gameObject.transform.position.x - viewDistance_x && Hero.transform.position.y >= gameObject.transform.position.y - viewDistance_y)
        {
            if(Hero.transform.position.x < gameObject.transform.position.x)
            {
                if(isDead == false)
                {
                    if (enemyName == "Slime" && isEnable == true)
                    {
                        gameObject.GetComponent<SpriteRenderer>().flipX = false;
                        if(isAnim == true)
                        {
                            gameObject.GetComponent<Animator>().Play("slime_walk");
                        }
                    }
                    //else if (enemyName != "Slime" && isEnable == true)
                    //{
                    //    gameObject.GetComponent<SpriteRenderer>().flipX = true;
                    //}
                    if(enemyName == "Treant" && isEnable == true)
                    {
                        gameObject.GetComponent<SpriteRenderer>().flipX = true;
                        if (isAnim == true)
                        {
                            gameObject.GetComponent<Animator>().Play("trent_walk");
                        }
                    }
                    //else if(enemyName != "Treant" && isEnable == true)
                    //{
                    //    gameObject.GetComponent<SpriteRenderer>().flipX = true;
                    //}

                    rb.AddForce(Vector2.left * Speed);
                }
            }
            if(Hero.transform.position.x > gameObject.transform.position.x)
            {
                if(isDead == false)
                {
                    if (enemyName == "Slime" && isEnable == true)
                    {
                        gameObject.GetComponent<SpriteRenderer>().flipX = true;
                        if(isAnim == true)
                        {
                            gameObject.GetComponent<Animator>().Play("slime_walk");
                        } 
                    }
                    //else if (enemyName != "Slime" && isEnable == true)
                    //{
                    //    gameObject.GetComponent<SpriteRenderer>().flipX = false;
                    //}
                    if(enemyName == "Treant" && isEnable == true)
                    {
                        gameObject.GetComponent<SpriteRenderer>().flipX = false;
                        if (isAnim == true)
                        {
                            gameObject.GetComponent<Animator>().Play("trent_walk");
                        }
                    }
                    //else if(enemyName != "Treant" && isEnable == true)
                    //{
                    //    gameObject.GetComponent<SpriteRenderer>().flipX = false;
                    //}

                    rb.AddForce(Vector2.right * Speed);
                }
            }
        }
        //else
        //{
        //    if(isDead == false)
        //    {
        //        if (enemyName == "Slime")
        //        {
        //            gameObject.GetComponent<Animator>().Play("slime_idle");
        //        }
        //    }
        //}
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.tag == "Bullet")
        {
            if(enemyName == "Slime")
            {
                audioSource.PlayOneShot(hit);
                HP -= 1;
                isAnim = false;
                StartCoroutine(hurtWait());
                gameObject.GetComponent<Animator>().SetTrigger("hurt");
            }
            else if(enemyName == "Treant")
            {
                audioSource.PlayOneShot(hit);
                HP -= 1;
                isAnim = false;
                StartCoroutine(hurtWait());
                gameObject.GetComponent<Animator>().SetTrigger("hurt");
            }
        }
        else if(collision.transform.tag == "Player")
        {
            if(Hero.GetComponent<Hero_System>().HP > 0)
            {
                Hero.GetComponent<Animator>().Play("Mushroom_Hurt");
                if (isCanAttack == true)
                {
                    stop = false;

                    StartCoroutine(enemyAttack());
                }
            }
        }
    }

    public void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            stop = true;
            isCanAttack = true;
            StopCoroutine(enemyAttack());
        }
    }

    IEnumerator hurtWait()
    {
        yield return new WaitForSeconds(0.25f);
        isAnim = true;
    }

    IEnumerator wait()
    {
        yield return new WaitForSeconds(0.433f);
        isEnable = true;
    }

    IEnumerator enemyAttack()
    {
        if(stop == false && isDead == false)
        {
            isCanAttack = false;
            audioSource.PlayOneShot(hit);
            Hero.GetComponent<Hero_System>().HP -= 1;

            if (enemyName == "Slime")
            {
                isEnable = false;
                gameObject.GetComponent<Animator>().Play("slime_attack");

                StartCoroutine(wait());
            }
            else if(enemyName == "Treant")
            {
                isEnable = false;
                gameObject.GetComponent<Animator>().Play("trent_attack");

                StartCoroutine(wait());
            }

            yield return new WaitForSeconds(2f);
            isCanAttack = true;
            StartCoroutine(enemyAttack());
        }
        else
        {
            StopCoroutine(enemyAttack());
        }
    }
}
