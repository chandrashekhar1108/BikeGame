using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollider : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "checkpoint")
        {
            GameManager.instance.CurrentSpawnPoint = other.transform;
            other.gameObject.SetActive(false);
        }
        if(other.tag=="Fail")
        {
            GameManager.instance.LevelFail();
            gameObject.SetActive(false);
        }
        if(other.tag=="Coin")
        {
            other.gameObject.SetActive(false);
            GameManager.instance.CollectedCoins += 100;
            GameManager.instance.CoinCollectedIngame();
            GameManager.instance.CoinSound.Play();
        }
    }
}
