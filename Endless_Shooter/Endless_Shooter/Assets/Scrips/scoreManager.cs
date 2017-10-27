namespace VRTK {

    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;
    using UnityEngine.SceneManagement;

    public class scoreManager : MonoBehaviour {
        public float timerMaxValue = 180;
        public float timerMinValue = 90;
        public float score = 0;

        private float timeLeft;
        private float secondsCount;
        private int mintueCount;
        //private Text timerText;
        //public Text finalScoreText;
        private GameObject playerWatch;
        private scoreManager managerInstance;
        private Text scoreText;
        bool isGameEnd = false;

        void Awake() {
            DontDestroyOnLoad(this);
            if (managerInstance == null) {
                managerInstance = this;
            } else {
                DestroyObject(gameObject);
            }
        }

        // Use this for initialization
        void Start() {
            timeLeft = Random.Range(timerMinValue, timerMaxValue);
            playerWatch = GameObject.Find("LeftControllerTooltips");
            //timerText = GameObject.Find ("Timer").GetComponent<Text> ();
            Invoke("LoadGameOverScene", timeLeft + 0.2f);
            Invoke("GetTheFuckingText", 0.2f);
            
        }

        // Update is called once per frame
        void Update() {
            if (isGameEnd)
                return;

            timeLeft -= Time.deltaTime;
            //timerText.text = "Time Left: " + (Mathf.Round (timeLeft));
            secondsCount += Time.deltaTime;
            if (Mathf.RoundToInt(secondsCount) >= 60)
            {
                mintueCount++;
                secondsCount = 0;
            }
            if (SceneManager.GetActiveScene().name == "VR_City_Single block" || SceneManager.GetActiveScene().name == "Handmade_Map")
            {
                playerWatch.GetComponent<VRTK_ControllerTooltips>().UpdateText(VRTK_ControllerTooltips.TooltipButtons.TouchpadTooltip, mintueCount.ToString() + ":" + (Mathf.Round(secondsCount)).ToString());
            }
            else
            {
                playerWatch.GetComponent<VRTK_ControllerTooltips>().UpdateText(VRTK_ControllerTooltips.TooltipButtons.TouchpadTooltip, "Time Left: " + (Mathf.Round(timeLeft)).ToString());
            }
            playerWatch.GetComponent<VRTK_ControllerTooltips>().UpdateText(VRTK_ControllerTooltips.TooltipButtons.ButtonOneTooltip, "Score: " + score.ToString());

            if (GameObject.FindGameObjectsWithTag("enemy").Length < 5)
            {
                print("Enemy eliminated");
                isGameEnd = true;
                scoreText.text="Your Time: " + '\n' + mintueCount + ":" + Mathf.RoundToInt(secondsCount);
                //Time.timeScale=0f;
            }
            else
            {
                print(GameObject.FindGameObjectWithTag("enemy").name);
            }

    }

        public void LoadGameOverScene() {
            //SceneManager.LoadScene(1, LoadSceneMode.Single);
            /*finalScoreText = GameObject.Find ("Final Score").GetComponent<Text> ();
            Debug.Log (finalScoreText.name);
            finalScoreText.text = "Final Score: " + score;*/
        }

        void GetTheFuckingText()
        {
            if (SceneManager.GetActiveScene().name == "VR_City_Single block" || SceneManager.GetActiveScene().name == "Handmade_Map")
            {
                scoreText = GameObject.Find("yourScore").GetComponent<Text>();
            }
        }
    }
}
