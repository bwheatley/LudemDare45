using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager instance;

    public float moveSpeed = 5f;
    public float defaultCameraSize = 34.2f;

    public GameObject player;


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



}
