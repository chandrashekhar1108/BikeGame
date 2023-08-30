using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Cinemachine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject Levels;
    public GameObject[] Players;
    public Transform PlayerPositions,CurrentSpawnPoint;
    public GameObject LevelCompletePage, LevelFailPage,PausePage,InstructionPopup;
    public int TempTotalCoins = 0, SpawnChances, CollectedCoins = 0;
    public GameObject[] SpawnChance_Obj,Stars_Obj;
    public AudioSource CoinSound;
    public GameObject CurrentPlayer,RespawnButton,FadeEffect;
    public Text Text_LevelReward,Text_TotalEarned,Text_CoinsCollected,Text_CollectedCoinIngame,Text_TotalCoins;
    
    private void Awake()
    {
        instance = this;
        if(Time.timeScale==0)
        {
            Time.timeScale = 1;
        }
        //PlayerPrefs.SetInt("CurrentVehicle", 2);
        //PlayerPrefs.SetInt("SelectedLevel", 1);
        PlayerPrefs.SetInt("SpawnChances", 3);

        PlayerPrefs.GetInt("SelectedLevel");
        PlayerPrefs.GetInt("CurrentVehicle");
        PlayerPrefs.GetInt("TotalCoins");
        EnablePlayer();
    }
    void Start()
    {
        EnableLevel();
        //EnablePlayer();
        CheckSound();
        //if (!PlayerPrefs.HasKey("SpawnChances"))
        //{
        //    PlayerPrefs.SetInt("SpawnChances", 3);
        //}
        if (!PlayerPrefs.HasKey("Instruction"))
        {
            PlayerPrefs.SetString("Instruction", "false");
        }
        PlayerPrefs.GetString("Instruction");
        SpawnChances = PlayerPrefs.GetInt("SpawnChances");
        InstructionStatus();
        CheckSpawnChancesStatus();
        Text_TotalCoins.text=""+ PlayerPrefs.GetInt("TotalCoins"); ;
    }
    void InstructionStatus()
    {
        if (PlayerPrefs.GetString("Instruction") == "false")
        {
            InstructionPopup.SetActive(true);
            PlayerPrefs.SetString("Instruction", "true");
        }
        else
        {
            InstructionPopup.SetActive(false);
        }
    }
    void EnableLevel()
    {
        for(int i=0;i< Levels.transform.childCount;i++)
        {
            if(i== PlayerPrefs.GetInt("SelectedLevel")-1)
            {
                Levels.transform.GetChild(i).gameObject.SetActive(true);
            }
            else
            {
                Levels.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
        
    }
    void EnablePlayer()
    {
        for (int i = 0; i < Players.Length; i++)
        {
            if (i == PlayerPrefs.GetInt("CurrentVehicle"))
            {
                CurrentPlayer = Players[i].gameObject;
                Players[i].transform.position = PlayerPositions.transform.GetChild(PlayerPrefs.GetInt("SelectedLevel") - 1).transform.position;
                Players[i].transform.rotation = PlayerPositions.transform.GetChild(PlayerPrefs.GetInt("SelectedLevel") - 1).transform.rotation;
                Players[i].transform.gameObject.SetActive(true);
                Camera.main.GetComponent<BikeCamera>().target = Players[i].transform;
            }
            else
            {
                Players[i].transform.gameObject.SetActive(false);
            }
        }
    }
    void CheckSound()
    {
        if (PlayerPrefs.GetString("Sound") == "ON")
        {
            gameObject.GetComponent<AudioSource>().mute = false;
        }
        else
            if (PlayerPrefs.GetString("Sound") == "OFF")
        {
            gameObject.GetComponent<AudioSource>().mute = true;
        }
    }
    public void UpdateSpawnChances(int amount,bool add)
    {
        if(PlayerPrefs.GetInt("SpawnChances") >= 0)
        {
            if (add)
            {
                SpawnChances = PlayerPrefs.GetInt("SpawnChances");
                SpawnChances += amount;
                PlayerPrefs.SetInt("SpawnChances", SpawnChances);
            }
            else
            {
                SpawnChances = PlayerPrefs.GetInt("SpawnChances");
                SpawnChances -= amount;
                PlayerPrefs.SetInt("SpawnChances", SpawnChances);
            }
        }
        

        CheckSpawnChancesStatus();

    }
    void CheckSpawnChancesStatus()
    {
        for (int i = 0; i < SpawnChance_Obj.Length; i++)
        {
            if (i < PlayerPrefs.GetInt("SpawnChances"))
            {
                SpawnChance_Obj[i].transform.GetChild(0).gameObject.SetActive(true);
            }
            else
            {
                SpawnChance_Obj[i].transform.GetChild(0).gameObject.SetActive(false);
            }

        }
        if (PlayerPrefs.GetInt("SpawnChances") <= 3 && PlayerPrefs.GetInt("SpawnChances") > 0 && CurrentSpawnPoint != null)
        {
            //RespawnButton.SetActive(true);
        }
        else if(PlayerPrefs.GetInt("SpawnChances") == 0 || CurrentSpawnPoint == null)
        {
            //RespawnButton.SetActive(false);
        }
    }
    bool RespawnClicked;
    public void RespawnPlayer()
    {
        if(PlayerPrefs.GetInt("SpawnChances") > 0 && CurrentSpawnPoint != null)
        {
            CurrentPlayer.transform.localPosition = new Vector3(CurrentSpawnPoint.position.x,CurrentSpawnPoint.position.y+0.45f, CurrentSpawnPoint.position.z);
            CurrentPlayer.transform.localRotation = CurrentSpawnPoint.localRotation;
            CurrentPlayer.transform.gameObject.SetActive(true);
            CurrentPlayer.transform.GetComponent<Rigidbody>().isKinematic = false;
            LevelFailPage.SetActive(false);
        }
        else if(PlayerPrefs.GetInt("SpawnChances") > 0 && CurrentSpawnPoint == null)
        {
            CurrentPlayer.transform.position = PlayerPositions.transform.GetChild(PlayerPrefs.GetInt("SelectedLevel") - 1).transform.position;
            CurrentPlayer.transform.localRotation = PlayerPositions.transform.GetChild(PlayerPrefs.GetInt("SelectedLevel") - 1).transform.localRotation;
            CurrentPlayer.transform.gameObject.SetActive(true);
            CurrentPlayer.transform.GetComponent<Rigidbody>().isKinematic = false;
            LevelFailPage.SetActive(false);
        }
        else
        {
            // No chances
        }
    }
    public void FixStruckVehicle()
    {
        if(CurrentSpawnPoint != null)
        {
            CurrentPlayer.transform.localPosition = new Vector3(CurrentSpawnPoint.position.x, CurrentSpawnPoint.position.y + 0.45f, CurrentSpawnPoint.position.z);
            CurrentPlayer.transform.localRotation = CurrentSpawnPoint.localRotation;
        }
        else
        {
            CurrentPlayer.transform.position = PlayerPositions.transform.GetChild(PlayerPrefs.GetInt("SelectedLevel") - 1).transform.position;
            CurrentPlayer.transform.localRotation = PlayerPositions.transform.GetChild(PlayerPrefs.GetInt("SelectedLevel") - 1).transform.localRotation;
        }
    }
    public void UpdateCoins(int amount, bool Add)
    {
        int TempCoins = PlayerPrefs.GetInt("TotalCoins");
        if (Add == true)
        {
            TempCoins += amount;
        }
        else
        {
            TempCoins -= amount;
        }
        PlayerPrefs.SetInt("TotalCoins", TempCoins);
    }
    public void OnClickMoreGames()
    {
        Application.OpenURL("https://play.google.com/store/apps/developer?id=KRYS+STUDIO&hl=en-IN");
    }
    public void OnClicRateUs()
    {
        Application.OpenURL("https://play.google.com/store/apps/details?id=" + Application.identifier);
    }
    void CheckStars()
    {
        for(int i=0;i< Stars_Obj.Length;i++)
        {
            if(i < SpawnChances)
            {
                Stars_Obj[i].transform.GetChild(0).gameObject.SetActive(true);
            }
            else
            {
                Stars_Obj[i].transform.GetChild(0).gameObject.SetActive(false);
            }
        }
    }
    public void LevelFinish()
    {
        CheckStars();
        LevelCompletePage.SetActive(true);
        int LevelReward = 1000 + CollectedCoins;
        //int TotalEarned = LevelReward * SpawnChances;
        UpdateCoins(LevelReward, true);
        Text_LevelReward.text = "Level Reward : 1000";
        Text_CoinsCollected.text = "Coins collected : " + CollectedCoins;
        Text_TotalEarned.text = "Total Earned : " + LevelReward;
    }
    public void CoinCollectedIngame()
    {
        Text_CollectedCoinIngame.text = "" + CollectedCoins;
    }
    public void LevelFail()
    {
        CheckSpawnChancesStatus();
        UpdateSpawnChances(1, false);
        if (SpawnChances > 0 && CurrentSpawnPoint != null || SpawnChances > 0 && CurrentSpawnPoint == null)
        {
            FadeEffect.SetActive(true);
            Invoke("RespawnPlayer", 1f);
        }
        else
        {
            LevelFailPage.SetActive(true);
        }
        CurrentPlayer.GetComponent<Rigidbody>().isKinematic = true;
    }
    public void FillAllLives()
    {
        // if Ad successful
        PlayerPrefs.SetInt("SpawnChances", 3);
        LevelFailPage.SetActive(false);
        CheckSpawnChancesStatus();
        RespawnPlayer();
    }
    public void Next()
    {
        PlayerPrefs.SetInt("NextSelected", 1);
        PlayerPrefs.SetInt("CurrentScene", 1);
        SceneManager.LoadScene("LoadingScene");
    }
    public void Home()
    {
        PlayerPrefs.SetInt("CurrentScene", 1);
        SceneManager.LoadScene("LoadingScene");
    }
    public void Restart()
    {
        PlayerPrefs.SetInt("CurrentScene", 2);
        SceneManager.LoadScene("LoadingScene");
    }
    public void PauseButtonAct()
    {
        Time.timeScale = 0;
        PausePage.SetActive(true);
    }
    public void ResumeButtonAct()
    {
        Time.timeScale = 1;
        PausePage.SetActive(false);
    }
    
}
