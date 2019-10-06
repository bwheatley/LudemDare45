using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour {

    public static GameManager instance;

    public float moveSpeed = 5f;
    public float defaultCameraSize = 34.2f;

    public GameObject player;
    public float jumpHeight = 1.0f;
    public bool OnGround = true;
    public string[] Deathmessage;

    public int CoinsCollected = 0;
    public GameObject CoinCountUI;

    public GameObject GameOverUI;
    public GameObject GameOver_CoinsUI;
    public GameObject GameOver_MessageUI;


    private void Awake() {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool CheckArrays_Color32(Color32[] array1, Color32[] array2)
    {
        var check = true;
        Debug.Log(string.Format("CHECK ARRAY LENGTH: Array1: {0} Array2: {1}", array1.Length, array2.Length));

        if(array1.Length == array2.Length)
        {
            for (int i = 0; i < array1.Length; i++) {
                if (array1[i].ToString() != array2[i].ToString()) {
                    Debug.Log(string.Format("CHECK CHECK CHECK: Array1: {0} Array2: {1}", array1[i].ToString(), array2[i].ToString()));
                    check = false;
                    return check;
                }
            }
        }else{
            check = false;
        }
        return check;
    }

    public void QuitGame() {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    /// <summary>

    public void Dead() {
        GameOver_MessageUI.GetComponent<TextMeshProUGUI>().text = string.Format("You have {0} and died", RandomDeath());
        GameOver_CoinsUI.GetComponent<TextMeshProUGUI>().text = string.Format("{0:N0}", CoinsCollected);
        // UnityEditor.EditorApplication.isPlaying = false;

        GameOverUI.SetActive(!GameOverUI.activeSelf);



    }


    public string RandomDeath() {
        return Deathmessage[Random.Range(0, Deathmessage.Length)];
    }


    public void CollectCoin(GameObject coin) {
        CoinsCollected++;
        IncrementCoin();
        Debug.Log(string.Format("Coin Collected we have {0} coins now!", CoinsCollected));
        Destroy(coin);
    }

    public void IncrementCoin() {
        CoinCountUI.GetComponent<TextMeshProUGUI>().text = string.Format("{0:N0}", CoinsCollected);
    }


}
