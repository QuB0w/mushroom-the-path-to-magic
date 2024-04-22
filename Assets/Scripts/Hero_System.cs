using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Hero_System : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;

    public GameObject bullet;
    private GameObject pauseMenu;
    private GameObject deathMenu;
    public GameObject jumpAnim;

    public Image[] Hearts;

    public Sprite Heart_Full;
    public Sprite Heart_Null;

    public Sprite Star_Empty;
    public Sprite Star_Full;

    private bool isGround;
    private bool isLeft = true;

    private bool isPause = false;
    private bool isHurt = true;

    public Sprite soundOn;
    public Sprite soundOff;

    public float rayDistance = 0.6f;
    private bool doubleJump = false;

    public AudioClip hit;
    public AudioClip collect;
    public AudioClip death;
    public AudioClip ui_click;
    private AudioSource audioSource;

    [Header("Характеристики")]
    public int HP = 3;
    public float Speed = 1f;
    public float jumpForce = 4f;
    public int StarCount = 0;
    public int StarsLevel = 0;

    [Header("Настройки")]
    public int levelCount = 0;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GameObject.Find("Main Camera").GetComponent<AudioSource>();

        if (PlayerPrefs.HasKey("Level_1_Stars") && levelCount == 1)
        {
            StarsLevel = PlayerPrefs.GetInt("Level_1_Stars");
        }
        else if(!PlayerPrefs.HasKey("Level_1_Stars") && levelCount == 1)
        {
            StarsLevel = 0;
        }
        if (PlayerPrefs.HasKey("Level_2_Stars") && levelCount == 2)
        {
            StarsLevel = PlayerPrefs.GetInt("Level_2_Stars");
        }
        else if (!PlayerPrefs.HasKey("Level_2_Stars") && levelCount == 2)
        {
            StarsLevel = 0;
        }

        PlayerPrefs.SetInt("LastLevel", levelCount);
        Time.timeScale = 1;
        isPause = false;
        rb = gameObject.GetComponent<Rigidbody2D>();
        anim = gameObject.GetComponent<Animator>();

        pauseMenu = GameObject.Find("Pause_Menu");
        pauseMenu.SetActive(false);
        deathMenu = GameObject.Find("Death_Menu");
        deathMenu.SetActive(false);
    }

    public void OnExit()
    {
        if(StarCount > StarsLevel && levelCount == 1)
        {
            PlayerPrefs.SetInt("Level_1_Stars", StarCount);
        }
        else if(StarCount > StarsLevel && levelCount == 2)
        {
            PlayerPrefs.SetInt("Level_2_Stars", StarCount);
        }
        if(levelCount == 1) 
        {
            PlayerPrefs.SetInt("Level_1_Passed", 1);
        }
        else if(levelCount == 2)
        {
            PlayerPrefs.SetInt("Level_2_Passed", 1);
        }

        SceneManager.LoadScene(0);
    }

    public void OnClickMainMenu()
    {
        audioSource.PlayOneShot(ui_click);
        SceneManager.LoadScene(0);
    }

    public void OnContinueClick()
    {
        audioSource.PlayOneShot(ui_click);
        isPause = false;
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
    }

    public void OnClickReplay()
    {
        audioSource.PlayOneShot(ui_click);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnClickSound()
    {
        audioSource.PlayOneShot(ui_click);
        if (audioSource.volume == 1)
        {
            GameObject.Find("Sound_Button").GetComponent<Image>().sprite = soundOff;
            audioSource.volume = 0;
        }
        else if(audioSource.volume == 0)
        {
            GameObject.Find("Sound_Button").GetComponent<Image>().sprite = soundOn;
            audioSource.volume = 1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(StarCount == 1)
        {
            GameObject.Find("Star_UI_1").GetComponent<Image>().sprite = Star_Full;
            GameObject.Find("Star_UI_2").GetComponent<Image>().sprite = Star_Empty;
            GameObject.Find("Star_UI_3").GetComponent<Image>().sprite = Star_Empty;
        }
        else if(StarCount == 2)
        {
            GameObject.Find("Star_UI_1").GetComponent<Image>().sprite = Star_Full;
            GameObject.Find("Star_UI_2").GetComponent<Image>().sprite = Star_Full;
            GameObject.Find("Star_UI_3").GetComponent<Image>().sprite = Star_Empty;
        }
        else if (StarCount == 3)
        {
            GameObject.Find("Star_UI_1").GetComponent<Image>().sprite = Star_Full;
            GameObject.Find("Star_UI_2").GetComponent<Image>().sprite = Star_Full;
            GameObject.Find("Star_UI_3").GetComponent<Image>().sprite = Star_Full;
        }
        else if (StarCount == 0)
        {
            GameObject.Find("Star_UI_1").GetComponent<Image>().sprite = Star_Empty;
            GameObject.Find("Star_UI_2").GetComponent<Image>().sprite = Star_Empty;
            GameObject.Find("Star_UI_3").GetComponent<Image>().sprite = Star_Empty;
        }

        RaycastHit2D hit = Physics2D.Raycast(rb.position, Vector2.down, rayDistance, LayerMask.GetMask("Ground"));

        if(hit.collider != null)
        {
            isGround = true;
            doubleJump = false;
        }
        else
        {
            isGround = false;
        }

        if (Input.GetKeyDown(KeyCode.Escape) && HP > 0)
        {
            if (pauseMenu.active == false)
            {
                isPause = true;
                Time.timeScale = 0;
                pauseMenu.SetActive(true);
            }
            else
            {
                isPause = false;
                Time.timeScale = 1;
                pauseMenu.SetActive(false);
            }
        }

        if (HP == 3)
        {
            Hearts[0].sprite = Heart_Full;
            Hearts[1].sprite = Heart_Full;
            Hearts[2].sprite = Heart_Full;
        }
        else if(HP == 2)
        {
            Hearts[0].sprite = Heart_Full;
            Hearts[1].sprite = Heart_Full;
            Hearts[2].sprite = Heart_Null;
            Hearts[2].GetComponent<Animator>().Play("heart_anim");
        }
        else if(HP == 1)
        {
            Hearts[0].sprite = Heart_Full;
            Hearts[1].sprite = Heart_Null;
            Hearts[2].sprite = Heart_Null;
            Hearts[1].GetComponent<Animator>().Play("heart_anim");
        }
        else if(HP <= 0)
        {
            //audioSource.PlayOneShot(death);
            isHurt = false;
            Hearts[0].sprite = Heart_Null;
            Hearts[1].sprite = Heart_Null;
            Hearts[2].sprite = Heart_Null;
            Hearts[0].GetComponent<Animator>().Play("heart_anim");
            anim.Play("Mushroom_Death");
            StartCoroutine(waitDeath());
        }
        if(isPause == false && HP > 0)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                if (isGround)
                {
                    rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
                    jumpAnim.SetActive(true);
                    jumpAnim.transform.position = new Vector3(transform.position.x, transform.position.y, -2f);
                    jumpAnim.GetComponent<Animator>().SetTrigger("jump");
                }
                else if (!doubleJump && rb.velocity.y < 0)
                {
                    doubleJump = true;
                    rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
                    jumpAnim.SetActive(true);
                    jumpAnim.transform.position = new Vector3(transform.position.x, transform.position.y, -2f);
                    jumpAnim.GetComponent<Animator>().SetTrigger("jump");
                }
            }

            if (Input.GetKey(KeyCode.A))
            {
                var rot = GameObject.Find("mushroom_rot");
                rot.transform.localPosition = new Vector3(-0.1f, rot.transform.localPosition.y, rot.transform.localPosition.z);
                anim.SetBool("Run", true);
                gameObject.GetComponent<SpriteRenderer>().flipX = true;
                rb.AddForce(Vector3.left * Speed);
                isLeft = true;
            }
            if (Input.GetKey(KeyCode.D))
            {
                var rot = GameObject.Find("mushroom_rot");
                rot.transform.localPosition = new Vector3(0.1f, rot.transform.localPosition.y, rot.transform.localPosition.z);
                anim.SetBool("Run", true);
                gameObject.GetComponent<SpriteRenderer>().flipX = false;
                rb.AddForce(Vector3.right * Speed);
                isLeft = false;
            }
            if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.W))
            {
                anim.SetBool("Run", false);
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                GameObject.Find("mushroom_rot").GetComponent<Animator>().Play("Mushroom_Attack");
                if (isLeft == true)
                {
                    var bulletObj = Instantiate(bullet, new Vector3(gameObject.transform.position.x - 0.19f, gameObject.transform.position.y - 0.369f, -1f), Quaternion.identity);
                    bulletObj.GetComponent<Bullet_System>().Speed = 3f;
                }
                else
                {
                    var bulletObj = Instantiate(bullet, new Vector3(gameObject.transform.position.x + 0.19f, gameObject.transform.position.y - 0.369f, -1f), Quaternion.identity);
                    bulletObj.GetComponent<Bullet_System>().Speed = -3f;
                }
            }
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Spikes")
        {
            audioSource.PlayOneShot(hit);
            isHurt = true;
            HP -= 1;
            anim.Play("Mushroom_Hurt");
            StartCoroutine(waitSpikes());
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Spikes")
        {
            isHurt = false;
            StopCoroutine(waitSpikes());
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Star")
        {
            audioSource.PlayOneShot(collect);
            StarCount += 1;
            collision.gameObject.GetComponent<BoxCollider2D>().enabled = false;
            collision.gameObject.GetComponentInParent<Animator>().Play("StarPivot_Collected");
            Destroy(collision.gameObject, 0.5f);
        }
        else if(collision.gameObject.name == "ExitCollider")
        {
            OnExit();
        }
        else if(collision.gameObject.name == "SecretRoom")
        {
            collision.gameObject.GetComponent<BoxCollider2D>().enabled = false;
            collision.gameObject.SetActive(false);
            GameObject.Find("Star_3_Light").GetComponent<Light>().enabled = true;
        }
        else if(collision.gameObject.name == "DoorCollider")
        {
            OnExit();
        }
    }

    public IEnumerator waitSpikes()
    {
        yield return new WaitForSeconds(1f);
        if(isHurt == true)
        {
            audioSource.PlayOneShot(hit);
            HP -= 1;
            anim.Play("Mushroom_Hurt");
            StartCoroutine(waitSpikes());
        }
    }

    public IEnumerator waitDeath()
    {
        yield return new WaitForSeconds(3f);
        deathMenu.SetActive(true);
        Time.timeScale = 0f;
    }
}
