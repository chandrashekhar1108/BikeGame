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
    public GameObject Players;
    public Transform PlayerPositions,CurrentSpawnPoint;
    //public CinemachineVirtualCamera VirtualCamera;
    //public MoveOnSwipe_EightDirections moveOn_EightDirection;
    public GameObject LevelCompletePage, LevelFailPage,PausePage,InstructionPopup;
    public int CoinsCollectedValue = 0, TempTotalCoins=0,SpawnChances;
    public GameObject[] SpawnChance_Obj;
    public AudioSource CoinSound;
    private void Awake()
    {
        instance = this;
        if(Time.timeScale==0)
        {
            Time.timeScale = 1;
        }
        PlayerPrefs.GetInt("SelectedLevel");
        PlayerPrefs.GetInt("CurrentBall");
        PlayerPrefs.GetInt("TotalCoins");
    }
    void Start()
    {
        EnableLevel();
        EnablePlayer();
        CheckSound();
        if (!PlayerPrefs.HasKey("SpawnChances"))
        {
            PlayerPrefs.SetInt("SpawnChances", 3);
        }
        if (!PlayerPrefs.HasKey("Instruction"))
        {
            PlayerPrefs.SetString("Instruction", "false");
        }
        PlayerPrefs.GetString("Instruction");
        SpawnChances = PlayerPrefs.GetInt("SpawnChances");
        InstructionStatus();
        CheckSpawnChancesStatus();
        Invoke("SetCamFollow", 1f);
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
    void SetCamFollow()
    {
        //VirtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_BindingMode =
        //    CinemachineTransposer.BindingMode.SimpleFollowWithWorldUp;
        
    }
    void EnablePlayer()
    {
        for (int i = 0; i < Players.transform.childCount; i++)
        {
            if (i == PlayerPrefs.GetInt("CurrentBall"))
            {
                Players.transform.GetChild(i).position = PlayerPositions.transform.GetChild(PlayerPrefs.GetInt("SelectedLevel") - 1).transform.position;
                Players.transform.GetChild(i).rotation = PlayerPositions.transform.GetChild(PlayerPrefs.GetInt("SelectedLevel") - 1).transform.rotation;
                //VirtualCamera.Follow = Players.transform.GetChild(i).gameObject.transform;
                //VirtualCamera.LookAt = Players.transform.GetChild(i).gameObject.transform;
                Players.transform.GetChild(i).gameObject.SetActive(true);
                //moveOn_EightDirection.ballMovement = Players.transform.GetChild(i).GetComponent<BallMovement>();
            }
            else
            {
                Players.transform.GetChild(i).gameObject.SetActive(false);
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
        if (PlayerPrefs.GetInt("SpawnChances") <= 3)
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
        }
    }
    bool RespawnClicked;
    public void RespawnPlayer()
    {
        if(PlayerPrefs.GetInt("SpawnChances")>0&&CurrentSpawnPoint!=null)
        {
            //VirtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_BindingMode =
            //CinemachineTransposer.BindingMode.LockToTarget;

            Invoke("SetCamFollow", 1f);
            //UpdateSpawnChances(1, false);
            Players.transform.GetChild(PlayerPrefs.GetInt("CurrentBall")).localPosition = new Vector3(CurrentSpawnPoint.position.x,CurrentSpawnPoint.position.y+0.45f, CurrentSpawnPoint.position.z);
            Players.transform.GetChild(PlayerPrefs.GetInt("CurrentBall")).localRotation = CurrentSpawnPoint.localRotation;
            Players.transform.GetChild(PlayerPrefs.GetInt("CurrentBall")).gameObject.SetActive(true);
            Players.transform.GetChild(PlayerPrefs.GetInt("CurrentBall")).GetComponent<Rigidbody>().isKinematic = false;
            LevelFailPage.SetActive(false);
        }
        else if(PlayerPrefs.GetInt("SpawnChances") > 0 && CurrentSpawnPoint == null)
        {
            Players.transform.GetChild(PlayerPrefs.GetInt("CurrentBall")).position = PlayerPositions.transform.GetChild(PlayerPrefs.GetInt("SelectedLevel") - 1).transform.position;
            Players.transform.GetChild(PlayerPrefs.GetInt("CurrentBall")).localRotation = PlayerPositions.transform.GetChild(PlayerPrefs.GetInt("SelectedLevel") - 1).transform.localRotation;
            Players.transform.GetChild(PlayerPrefs.GetInt("CurrentBall")).gameObject.SetActive(true);
            Players.transform.GetChild(PlayerPrefs.GetInt("CurrentBall")).GetComponent<Rigidbody>().isKinematic = false;
            LevelFailPage.SetActive(false);
        }
        else
        {
            // No chances
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
        //TextTotalCoins.text = "" + PlayerPrefs.GetInt("TotalCoins");
    }
    public void OnClickMoreGames()
    {
        Application.OpenURL("https://play.google.com/store/apps/dev?id=6997972662360750451&gl=US");
    }
    public void OnClicRateUs()
    {
        Application.OpenURL("https://play.google.com/store/apps/details?id=" + Application.identifier);
    }
    public void LevelFinish()
    {
        LevelCompletePage.SetActive(true);
    }
    public void LevelFail()
    {
        LevelFailPage.SetActive(true);
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
    public void RateUsButtonAct()
    {
        Debug.Log("Rate Us");
    }
}
