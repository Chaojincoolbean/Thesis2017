using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using VRTK;
using VRTK.UnityEventHelper;


public class gameStart : MonoBehaviour {
    private VRTK_Button_UnityEvents buttonEvents;
    public bool isStartGame = false;
    // Use this for initialization
    void Start () {
        buttonEvents = GetComponent<VRTK_Button_UnityEvents>();
        if (buttonEvents == null)
        {
            buttonEvents = gameObject.AddComponent<VRTK_Button_UnityEvents>();
        }
        buttonEvents.OnPushed.AddListener(handlePush);
    }

    private void handlePush(object sender, Control3DEventArgs e)
    {
        VRTK_Logger.Info("Pushed");
        print("Pushed");
        gameObject.GetComponent<AudioSource>().Play();
        if (isStartGame == true)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1, LoadSceneMode.Single);
        }
    }
}
