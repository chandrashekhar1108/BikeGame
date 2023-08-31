using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public static MenuManager instance;
    public GameObject MenuPage,ShopPage, SettingsPage,VehicleSelectionPage,LevelSelectionPage,NoCoinsPopup;
    public GameObject[] Vehicles;
    public int[] VehiclePrice;
    public GameObject Prev_ArrowObj,Next_ArrowObj,LockObj,BuyButton,SoundButton;
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
        if (!PlayerPrefs.HasKey("CurrentVehicle"))
        {
            PlayerPrefs.SetInt("CurrentVehicle", 0);
        }
        if (!PlayerPrefs.HasKey("TotalCoins"))
        {
            PlayerPrefs.SetInt("TotalCoins", 0);
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
        PlayerPrefs.GetInt("CurrentVehicle");
        PlayerPrefs.GetInt("TotalCoins");
        if(PlayerPrefs.GetInt("PlayerLock" + 0)==1)
        {
            BuyButton.transform.GetChild(0).GetComponent<Text>().text = "Next";
            TextPrice.transform.parent.gameObject.SetActive(false);
            LockObj.SetActive(false);
            Prev_ArrowObj.SetActive(false);
            Next_ArrowObj.SetActive(true);
        }
        TextTotalCoins.text=""+ PlayerPrefs.GetInt("TotalCoins");
        CheckLevelLocks();
        UnlockAllStatus();
        if (PlayerPrefs.GetInt("NextSelected") == 1)
        {
            LevelSelectionPage.SetActive(true);
            PlayerPrefs.SetInt("NextSelected", 0);
        }
        else
        {
            MenuPage.SetActive(true);
        }
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
        VehicleSelectionPage.SetActive(false);
        MenuPage.SetActive(true);
    }
    public void BackToStore()
    {
        VehicleSelectionPage.SetActive(true);
        LevelSelectionPage.SetActive(false);
    }
    public void OnClickMoreGames()
    {
        Application.OpenURL("https://play.google.com/store/apps/developer?id=KRYS+STUDIO&hl=en-IN");
    }
    public void PlayButtonAct()
    {
        VehicleSelectionPage.SetActive(true);
        MenuPage.SetActive(false);
    }
    public int p = 0;
    public void PrevArrow()
    {
        p--;
        Vehicle(p); 
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
        Vehicle(p);
        if (p == Vehicles.Length-1)
        {
            Next_ArrowObj.SetActive(false);
        }
        else
        {
            Prev_ArrowObj.SetActive(true);
        }

    }
    public void Vehicle(int vehicleIndex)
    {
        for (int i = 0; i < Vehicles.Length; i++)
        {
            if (i == vehicleIndex)
            {
                Vehicles[i].SetActive(true);
                TextPrice.text = "" + VehiclePrice[i];
                if (PlayerPrefs.GetInt("PlayerLock" + p) == 1)
                {
                    LockObj.SetActive(false);
                    BuyButton.transform.GetChild(0).GetComponent<Text>().text = "Next";
                    TextPrice.transform.parent.gameObject.SetActive(false);
                }
                else
                {
                    LockObj.SetActive(true);
                    BuyButton.transform.GetChild(0).GetComponent<Text>().text = "Buy";
                    TextPrice.transform.parent.gameObject.SetActive(true);
                }
            }
            else
            {
                Vehicles[i].SetActive(false);
            }
        }
    }

    public void BuyOrNext()
    {
        if(PlayerPrefs.GetInt("PlayerLock" + p) == 1)
        {
            NextAct_VehicleSelection();
        }
        else
        {
            if (PlayerPrefs.GetInt("TotalCoins") >= VehiclePrice[p])
            {
                UpdateCoins(VehiclePrice[p], false);
                PlayerPrefs.SetInt("PlayerLock" + p, 1);
                TextPrice.transform.parent.gameObject.SetActive(false);
                LockObj.SetActive(false);
                BuyButton.transform.GetChild(0).GetComponent<Text>().text = "Next";
            }
            else
            {
                //Debug.Log("not enough Coins");
                NoCoinsPopup.SetActive(true);
            }
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
    public void NextAct_VehicleSelection()
    {
        PlayerPrefs.SetInt("CurrentVehicle",p);
        Debug.Log("SelectedPlayer Index :::"+p);
        LevelSelectionPage.SetActive(true);
        VehicleSelectionPage.SetActive(false);
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
    public int AllLevelsValue, AllVehiclesValue;
    public GameObject AllLevelBuyButton, AllVehiclesBuyButton;
    public void UnlockAllVehicles()
    {
        if (AllLevelsValue <= PlayerPrefs.GetInt("TotalCoins"))
        {
            UpdateCoins(AllVehiclesValue, false);
            PlayerPrefs.SetInt("UnlockAllVehicles", 1);
            UnlockAllStatus();
            for (int i=0;i<Vehicles.Length;i++)
            {
                PlayerPrefs.SetInt("PlayerLock" + i, 1);
            }
        }
        else
        {
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
            NoCoinsPopup.SetActive(true);
        }
        
    }
    void UnlockAllStatus()
    { 
        if(PlayerPrefs.GetInt("UnlockAllVehicles" )==1)
        {
            AllVehiclesBuyButton.SetActive(false);
        }
        else
        {
            AllVehiclesBuyButton.SetActive(true);
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
            NoCoinsPopup.SetActive(true);
        }
    }
}
