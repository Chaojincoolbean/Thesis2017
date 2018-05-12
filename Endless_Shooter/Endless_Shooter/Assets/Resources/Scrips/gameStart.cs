using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using VRTK;
using VRTK.UnityEventHelper;


public class gameStart : MonoBehaviour {
    private VRTK_Button_UnityEvents buttonEvents;
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
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1, LoadSceneMode.Single);
    }
}
