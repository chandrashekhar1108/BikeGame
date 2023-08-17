using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public static MenuManager instance;
    public GameObject MenuPage,ShopPage, SettingsPage,StorePage,LevelSelectionPage,PurchaseCoinsPage,NoCoinsPopup;
    public GameObject[] PlayBalls;
    public int[] BallsPrice;
    public GameObject Prev_ArrowObj,Next_ArrowObj,LockObj,BuyButton,NextButtonInStore,SoundButton;
    public Text TextPrice,TextTotalCoins;
    public GameObject[] Levels;
    public Sprite SoundOn, SoundOff;

    private void Awake()
    {
        instance = this;
        if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
        }
       
    }
    void Start()
    {
        //if (PlayerPrefs.GetInt("Begin") == 1)
        //{
        //    Invoke("ShowPromo", 1f);
        //}

        if (!PlayerPrefs.HasKey("CurrentBall"))
        {
            PlayerPrefs.SetInt("CurrentBall", 0);
        }
        if (!PlayerPrefs.HasKey("TotalCoins"))
        {
            PlayerPrefs.SetInt("TotalCoins", 10);
        }
        if (!PlayerPrefs.HasKey("PlayerLock"+0))
        {
            PlayerPrefs.SetInt("PlayerLock"+0, 1);
        }
        if (!PlayerPrefs.HasKey("UnlockedLevels"))
        {
            PlayerPrefs.SetInt("UnlockedLevels", 1);
        }
        if (!PlayerPrefs.HasKey("Sound"))
        {
            PlayerPrefs.SetString("Sound","ON");
        }
        CheckSound();
        PlayerPrefs.GetInt("UnlockedLevels");
        PlayerPrefs.SetInt("SelectedLevel",0);
        PlayerPrefs.GetInt("CurrentBall");
        PlayerPrefs.GetInt("TotalCoins");
        if(PlayerPrefs.GetInt("PlayerLock" + 0)==1)
        {
            BuyButton.SetActive(false);
            NextButtonInStore.SetActive(true);
            TextPrice.transform.parent.gameObject.SetActive(false);
            LockObj.SetActive(false);
        }
        TextTotalCoins.text=""+ PlayerPrefs.GetInt("TotalCoins");
        CheckLevelLocks();
        UnlockAllStatus();
        Debug.Log("NextTTTTTTT" + PlayerPrefs.GetInt("NextSelected"));
        if (PlayerPrefs.GetInt("NextSelected") == 1)
        {
            MenuPage.SetActive(false);
            LevelSelectionPage.SetActive(true);
            PlayerPrefs.SetInt("NextSelected", 0);
        }
    }
    void ShowPromo()
    {
        //CrossPromo.Instance.ForceShowPopup();
        PlayerPrefs.SetInt("Begin", 0);
    }


    private void OnApplicationQuit()
    {
        PlayerPrefs.SetInt("Begin", 1);
    }
    public void ShopPageAct()
    {
        ShopPage.SetActive(true);
        MenuPage.SetActive(false);
    }
    public void SettingPageAct()
    {
        SettingsPage.SetActive(true);
        MenuPage.SetActive(false);
    }
    public void BackToMain()
    {
        ShopPage.SetActive(false);
        SettingsPage.SetActive(false);
        StorePage.SetActive(false);
        MenuPage.SetActive(true);
    }
    public void BackToStore()
    {
        StorePage.SetActive(true);
        LevelSelectionPage.SetActive(false);
    }
    public void OnClickMoreGames()
    {
        Application.OpenURL("https://play.google.com/store/apps/dev?id=6997972662360750451&gl=US");
    }
    public void PlayButtonAct()
    {
        StorePage.SetActive(true);
        MenuPage.SetActive(false);
    }
    public int p = 0;
    public void PrevArrow()
    {
        p--;
        PlayBall(p); 
        if (p == 0)
        {
            Prev_ArrowObj.SetActive(false);
        }else
        {
            Next_ArrowObj.SetActive(true);
        }
    }
    public void NextArrow()
    {
        p++;
        PlayBall(p);
        if (p == PlayBalls.Length-1)
        {
            Next_ArrowObj.SetActive(false);
        }
        else
        {
            Prev_ArrowObj.SetActive(true);
        }

    }
    public void PlayBall(int Playerindex)
    {
        for (int i = 0; i < PlayBalls.Length; i++)
        {
            if (i == Playerindex)
            {
                PlayBalls[i].SetActive(true);
                TextPrice.text = "" + BallsPrice[i];
                if (PlayerPrefs.GetInt("PlayerLock" + p) == 1)
                {
                    LockObj.SetActive(false);
                    BuyButton.SetActive(false);
                    NextButtonInStore.SetActive(true);
                    TextPrice.transform.parent.gameObject.SetActive(false);
                }
                else
                {
                    LockObj.SetActive(true);
                    BuyButton.SetActive(true);
                    NextButtonInStore.SetActive(false);
                    TextPrice.transform.parent.gameObject.SetActive(true);
                }
            }
            else
            {
                PlayBalls[i].SetActive(false);
            }
        }
    }

    public void BuyPlayer()
    {
        if(PlayerPrefs.GetInt("TotalCoins") >= BallsPrice[p])
        {
            UpdateCoins(BallsPrice[p], false);
            PlayerPrefs.SetInt("PlayerLock" + p, 1);
            BuyButton.SetActive(false);
            TextPrice.transform.parent.gameObject.SetActive(false);
            LockObj.SetActive(false);
            NextButtonInStore.SetActive(true);
        }else
        {
            Debug.Log("not enough Coins");
            NoCoinsPopup.SetActive(true);
        }
        
    }
    public void UpdateCoins(int amount,bool Add)
    {
        int TempCoins = PlayerPrefs.GetInt("TotalCoins");
        if(Add==true)
        {
            TempCoins += amount;
        }
        else
        {
            TempCoins -= amount;
        }
        PlayerPrefs.SetInt("TotalCoins", TempCoins);
        TextTotalCoins.text = "" + PlayerPrefs.GetInt("TotalCoins");
    }
    public void NextAct_Store()
    {
        PlayerPrefs.SetInt("CurrentBall",p);
        Debug.Log("SelectedPlayer Index :::"+p);
        LevelSelectionPage.SetActive(true);
        StorePage.SetActive(false);
        CheckLevelLocks();
    }
    public void Level(int LevelIndex)
    {
        PlayerPrefs.SetInt("SelectedLevel", LevelIndex);
        Debug.Log("Selected Level:::::::::"+ PlayerPrefs.GetInt("SelectedLevel"));

        PlayerPrefs.SetInt("CurrentScene", 2);
        SceneManager.LoadScene("LoadingScene");
        //SceneManager.LoadScene("Gameplay");
    }
    public void CheckLevelLocks()
    {
        for(int i=0;i<Levels.Length;i++)
        {
            if(i < PlayerPrefs.GetInt("UnlockedLevels"))
            {
                Levels[i].GetComponent<Button>().interactable=true;
                Levels[i].transform.GetChild(0).gameObject.SetActive(true);
            }
            else
            {
                Levels[i].GetComponent<Button>().interactable = false;
                Levels[i].transform.GetChild(0).gameObject.SetActive(false);
            }
        }
    }
    int soundIndex;
    public void SoundButtonAct()
    {
        if(soundIndex==0)
        {
            PlayerPrefs.SetString("Sound", "ON");
            soundIndex++;
            SoundButton.GetComponent<Image>().sprite = SoundOn;
        }
        else
            if(soundIndex==1)
        {
            PlayerPrefs.SetString("Sound", "OFF");
            soundIndex = 0;
            SoundButton.GetComponent<Image>().sprite = SoundOff;
        }
        CheckSound();
    }
    void CheckSound()
    {
        if(PlayerPrefs.GetString("Sound")=="ON")
        {
            gameObject.GetComponent<AudioSource>().mute = false;
            SoundButton.GetComponent<Image>().sprite = SoundOn;
            soundIndex = 1;
        }
        else
            if(PlayerPrefs.GetString("Sound") == "OFF")
        {
            gameObject.GetComponent<AudioSource>().mute = true;
            SoundButton.GetComponent<Image>().sprite = SoundOff;
            soundIndex = 0;
        }
    }
    public int AllLevelsValue, AllBallsValue;
    public GameObject AllLevelBuyButton, AllBallsBuyButton;
    public void UnlockAllBalls()
    {
        if (AllLevelsValue <= PlayerPrefs.GetInt("TotalCoins"))
        {
            UpdateCoins(AllBallsValue, false);
            PlayerPrefs.SetInt("UnlockAllBalls", 1);
            UnlockAllStatus();
            for (int i=0;i<PlayBalls.Length;i++)
            {
                PlayerPrefs.SetInt("PlayerLock" + i, 1);
            }
        }
        else
        {
            //PurchaseCoinsPage.SetActive(true);
            NoCoinsPopup.SetActive(true);
        }
    }
    
    public void unlockAllLevels()
    {
        if(AllLevelsValue <= PlayerPrefs.GetInt("TotalCoins"))
        {
            PlayerPrefs.SetInt("UnlockAllLevels", 1);
            UpdateCoins(AllLevelsValue, false);
            PlayerPrefs.SetInt("UnlockedLevels",20);
            CheckLevelLocks();
            UnlockAllStatus();
        }
        else
        {
            //PurchaseCoinsPage.SetActive(true);
            NoCoinsPopup.SetActive(true);
        }
        
    }
    void UnlockAllStatus()
    { 
        if(PlayerPrefs.GetInt("UnlockAllBalls" )==1)
        {
            AllBallsBuyButton.SetActive(false);
        }
        else
        {
            AllBallsBuyButton.SetActive(true);
        }

        if(PlayerPrefs.GetInt("UnlockAllLevels") == 1)
        {
            AllLevelBuyButton.SetActive(false);
        }
        else
        {
            AllLevelBuyButton.SetActive(true);
        }
    }
    public void PurChaseCoins(int amount)
    {
        //if buys coins via IAP or ADS

        int tempCoins = PlayerPrefs.GetInt("TotalCoins");
        tempCoins += amount;
        PlayerPrefs.SetInt("TotalCoins", tempCoins);
        TextTotalCoins.text=""+ PlayerPrefs.GetInt("TotalCoins");
    }
    public void FillLives(int amount)
    {
        if(amount <= PlayerPrefs.GetInt("TotalCoins"))
        {
            PlayerPrefs.SetInt("SpawnChances", 3);

        }
        else
        {
            //PurchaseCoinsPage.SetActive(true);
            NoCoinsPopup.SetActive(true);
        }
    }
}
