using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu_System : MonoBehaviour
{
    public int LastLevelCount = 0;

    public Sprite starEmpty;
    public Sprite starFull;

    public Sprite levelOpen;
    public Sprite levelClose;

    public GameObject[] stars;
    public GameObject[] levelCount;

    public Text starsCount;
    public int starsAll = 0;

    public GameObject MainMenu;
    public GameObject Levels;

    public AudioClip ui_click;
    private AudioSource audioSource;

    public bool isLevel_1_Open = true;
    public bool isLevel_2_Open = false;
    
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GameObject.Find("Main Camera").GetComponent<AudioSource>();

        if (PlayerPrefs.HasKey("Level_1_Stars"))
        {
            starsAll += PlayerPrefs.GetInt("Level_1_Stars");

            if (PlayerPrefs.GetInt("Level_1_Stars") == 1)
            {
                GameObject.Find("Level_1_Star_1").GetComponent<Image>().sprite = starFull;
                GameObject.Find("Level_1_Star_2").GetComponent<Image>().sprite = starEmpty;
                GameObject.Find("Level_1_Star_3").GetComponent<Image>().sprite = starEmpty;
            }
            else if(PlayerPrefs.GetInt("Level_1_Stars") == 2)
            {
                GameObject.Find("Level_1_Star_1").GetComponent<Image>().sprite = starFull;
                GameObject.Find("Level_1_Star_2").GetComponent<Image>().sprite = starFull;
                GameObject.Find("Level_1_Star_3").GetComponent<Image>().sprite = starEmpty;
            }
            else if (PlayerPrefs.GetInt("Level_1_Stars") == 3)
            {
                GameObject.Find("Level_1_Star_1").GetComponent<Image>().sprite = starFull;
                GameObject.Find("Level_1_Star_2").GetComponent<Image>().sprite = starFull;
                GameObject.Find("Level_1_Star_3").GetComponent<Image>().sprite = starFull;
            }
        }
        if (PlayerPrefs.HasKey("Level_2_Stars"))
        {
            starsAll += PlayerPrefs.GetInt("Level_2_Stars");

            if (PlayerPrefs.GetInt("Level_2_Stars") == 1)
            {
                GameObject.Find("Level_2_Star_1").GetComponent<Image>().sprite = starFull;
                GameObject.Find("Level_2_Star_2").GetComponent<Image>().sprite = starEmpty;
                GameObject.Find("Level_2_Star_3").GetComponent<Image>().sprite = starEmpty;
            }
            else if (PlayerPrefs.GetInt("Level_2_Stars") == 2)
            {
                GameObject.Find("Level_2_Star_1").GetComponent<Image>().sprite = starFull;
                GameObject.Find("Level_2_Star_2").GetComponent<Image>().sprite = starFull;
                GameObject.Find("Level_2_Star_3").GetComponent<Image>().sprite = starEmpty;
            }
            else if (PlayerPrefs.GetInt("Level_2_Stars") == 3)
            {
                GameObject.Find("Level_2_Star_1").GetComponent<Image>().sprite = starFull;
                GameObject.Find("Level_2_Star_2").GetComponent<Image>().sprite = starFull;
                GameObject.Find("Level_2_Star_3").GetComponent<Image>().sprite = starFull;
            }
        }

        if (PlayerPrefs.HasKey("Level_1_Passed"))
        {
            isLevel_2_Open = true;
        }
        else
        {
            isLevel_2_Open = false;
        }

        if (!PlayerPrefs.HasKey("LastLevel"))
        {
            LastLevelCount = 1;
        }
        else
        {
            LastLevelCount = PlayerPrefs.GetInt("LastLevel");
        }

        MainMenu.SetActive(true); 
        Levels.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        starsCount.text = starsAll.ToString();

        if(Levels.active == true)
        {
            if (isLevel_1_Open == true)
            {
                GameObject.Find("Level_1_Level").GetComponent<Image>().sprite = levelOpen;
                stars[0].SetActive(true);
                levelCount[0].SetActive(true);
                stars[0].GetComponentInParent<Button>().enabled = true;
            }
            else
            {
                GameObject.Find("Level_1_Level").GetComponent<Image>().sprite = levelClose;
                stars[0].SetActive(false);
                levelCount[0].SetActive(false);
                stars[0].GetComponentInParent<Button>().enabled = false;
            }

            if (isLevel_2_Open == true)
            {
                GameObject.Find("Level_2_Level").GetComponent<Image>().sprite = levelOpen;
                stars[1].SetActive(true);
                levelCount[1].SetActive(true);
                stars[1].GetComponentInParent<Button>().enabled = true;
            }
            else
            {
                GameObject.Find("Level_2_Level").GetComponent<Image>().sprite = levelClose;
                stars[1].SetActive(false);
                levelCount[1].SetActive(false);
                stars[1].GetComponentInParent<Button>().enabled = false;
            }
        }
    }

    public void OnButtonPlayClicked()
    {
        audioSource.PlayOneShot(ui_click);
        SceneManager.LoadScene(LastLevelCount);
    }

    public void OnClickLevels()
    {
        audioSource.PlayOneShot(ui_click);
        MainMenu.SetActive(false);
        Levels.SetActive(true);
    }

    public void OnClickMenu()
    {
        audioSource.PlayOneShot(ui_click);
        MainMenu.SetActive(true);
        Levels.SetActive(false);
    }

    public void OnLevel_1_Clicked()
    {
        audioSource.PlayOneShot(ui_click);
        SceneManager.LoadScene(1);
    }

    public void OnLevel_2_Clicked()
    {
        audioSource.PlayOneShot(ui_click);
        SceneManager.LoadScene(2);
    }
}
