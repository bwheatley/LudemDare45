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
    public GameObject GameStartUI;

    public bool IsDead = false;
    public bool IsPaused = true;
    public Vector3 PauseVelocity = new Vector3();

    public AudioClip CoinEffect;
    public AudioClip HitEffect;
    public AudioClip VictoryEffect;
    public AudioClip DefeatEffect;
    public AudioClip FlapEffect;
    public GameObject masterEffectSource;
    public GameObject masterSoundsSource;
    public GameObject GameMusic;


    public void PlayClip(string soundToPlay, string group = "Sounds") {
        AudioSource _myAudio = masterEffectSource.GetComponent<AudioSource>();
        if (group != "Sounds") {
            _myAudio = masterSoundsSource.GetComponent<AudioSource>();
        }

        //LowerCasing it since it bite m in the ass once
        switch ( soundToPlay.ToLower()) {
            case "flap":
                _myAudio.clip = FlapEffect;
                break;
            case "coin":
                _myAudio.clip = CoinEffect;
                break;
            case "hit":
                _myAudio.clip = HitEffect;
                break;
            case "victory":
                _myAudio.clip = VictoryEffect;
                break;
            case "dead":
                _myAudio.clip = DefeatEffect;
                break;
            // case "menu_dropdown":
            //     _myAudio.clip = menu_dropdown;
            //     break;
            // case "ui_submit":
            //     _myAudio.clip = ui_submit;
            //     break;
            // case "victory":
            //     //Util.WriteDebugLog("Play Victory");
            //     _myAudio.clip = VictoryAudioClip;
            //     break;
        }

        //Play the selected clip
        //Util.WriteDebugLog(string.Format("Clip Played {0} Audio Manager {1} Clip {2}", soundToPlay, _myAudio.name, _myAudio.clip.name), GameManager.LogLevel_Debug);
        _myAudio.Play();

    }

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
        GameStartUI.SetActive(!GameStartUI.activeSelf);
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

    public void Dead(bool Exit = false) {
        IsDead = true;
        PauseGame();

        if (Exit) {
            GameOver_MessageUI.GetComponent<TextMeshProUGUI>().text =
                string.Format("You have escaped! Congratulations!", RandomDeath());
            GameManager.instance.PlayClip("victory");

        }
        else {
            GameOver_MessageUI.GetComponent<TextMeshProUGUI>().text =
                string.Format("You have {0} and died", RandomDeath());

        }

        GameOver_CoinsUI.GetComponent<TextMeshProUGUI>().text = string.Format("{0:N0}", CoinsCollected);
        // UnityEditor.EditorApplication.isPlaying = false;

        GameOverUI.SetActive(!GameOverUI.activeSelf);



    }


    public string RandomDeath() {
        return Deathmessage[Random.Range(0, Deathmessage.Length)];
    }


    public void CollectCoin(GameObject coin) {
        var coinValue = coin.GetComponent<Coin>().CoinValue;

        CoinsCollected = CoinsCollected + coinValue;
        IncrementCoin();
        GameManager.instance.PlayClip("coin");

        Debug.Log(string.Format("Coin Collected we have {0} coins now!", CoinsCollected));
        Destroy(coin);
    }

    public void IncrementCoin() {
        CoinCountUI.GetComponent<TextMeshProUGUI>().text = string.Format("{0:N0}", CoinsCollected);
    }


    /// <summary>
    /// Pause the physics of objects
    /// </summary>
    public void PauseGame() {

        if (!IsPaused) {
            PauseVelocity = player.GetComponent<Rigidbody2D>().velocity;
            player.GetComponent<Rigidbody2D>().isKinematic = true;
            IsPaused = true;
        }
        else {
            player.GetComponent<Rigidbody2D>().velocity = PauseVelocity;
            PauseVelocity = new Vector3();
            player.GetComponent<Rigidbody2D>().isKinematic = false;
            IsPaused = false;
        }

    }

    public void StartGame() {
        GameStartUI.SetActive(!GameStartUI.activeSelf);
        PauseGame();
    }

    public void RetryGame() {
        Application.LoadLevel(Application.loadedLevel);
    }

}
