using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelFinish : MonoBehaviour
{
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if(PlayerPrefs.GetInt("SelectedLevel") == PlayerPrefs.GetInt("UnlockedLevels"))
            {
                PlayerPrefs.SetInt("UnlockedLevels", PlayerPrefs.GetInt("SelectedLevel") + 1);
            }

            GameManager.instance.LevelFinish();
            GameManager.instance.CurrentPlayer.GetComponent<Rigidbody>().isKinematic = true;
            gameObject.SetActive(false);
        }
    }
}
