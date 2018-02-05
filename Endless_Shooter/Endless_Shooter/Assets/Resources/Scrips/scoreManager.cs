namespace VRTK {

    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;
    using UnityEngine.SceneManagement;

    public enum Timer
    {
        countUp, countDown, varingRunningSpeed, constantRunningSpeed
    }

    public class scoreManager : MonoBehaviour {
        public float timerMaxValue = 180;
        public float timerMinValue = 90;
        public float score = 0;
        public float karmaMax = 100f;
        public float karmaMin = -100f;

        public playerHit playerHit;
        private float timeLeft;
        private float secondsCount;
        private int mintueCount;
        //private Text timerText;
        //public Text finalScoreText;
        private GameObject playerWatch;
        private scoreManager managerInstance;
        public Text scoreText;
        [SerializeField]private GameObject VRPlayArea;
        bool isGameEnd = false;

        public Timer timer;
        public Timer runningPattern;

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
            //Invoke("GetTheFuckingText", 0.2f);
            VRPlayArea = GameObject.Find("PlayArea");
            // Debug.Log(GameObject.Find("Camera (eye)"));
            // playerHit = GameObject.Find("Camera (eye)").GetComponent<playerHit>();

        }

        // Update is called once per frame
        void Update() {
            if (isGameEnd)
                return;

            //Debug.Log(VRPlayArea.GetComponent<VRTK_MoveInPlace>().GetSpeed());
            switch (runningPattern)
            {
                case Timer.varingRunningSpeed:
                    if (VRPlayArea.GetComponent<VRTK_MoveInPlace>().GetSpeed() > 0f)
                    {
                        VRPlayArea.GetComponent<VRTK_MoveInPlace>().speedScale += Time.deltaTime;
                    }
                    else if (VRPlayArea.GetComponent<VRTK_MoveInPlace>().GetSpeed() == 0f)
                    {
                        VRPlayArea.GetComponent<VRTK_MoveInPlace>().speedScale = 5f;
                    }
                    break;
                case Timer.constantRunningSpeed:
                    VRPlayArea.GetComponent<VRTK_MoveInPlace>().speedScale = 10f;
                    break;
                default:
                    break;

            }

            //timerText.text = "Time Left: " + (Mathf.Round (timeLeft));
            secondsCount += Time.deltaTime;
            if (Mathf.RoundToInt(secondsCount) >= 60)
            {
                mintueCount++;
                secondsCount = 0;
            }

            switch (timer)
            {
                case Timer.countUp:
                    playerWatch.GetComponent<VRTK_ControllerTooltips>().UpdateText(VRTK_ControllerTooltips.TooltipButtons.TouchpadTooltip, mintueCount.ToString() + ":" + (Mathf.Round(secondsCount)).ToString());
                    break;
                case Timer.countDown:
                    timeLeft -= Time.deltaTime;
                    playerWatch.GetComponent<VRTK_ControllerTooltips>().UpdateText(VRTK_ControllerTooltips.TooltipButtons.TouchpadTooltip, "Time Left: " + (Mathf.Round(timeLeft)).ToString());
                    break;
                default:
                    break;
            }

            //Display karma
            playerWatch.GetComponent<VRTK_ControllerTooltips>().UpdateText(VRTK_ControllerTooltips.TooltipButtons.ButtonOneTooltip, "Karma: " + (score + playerHit.playerHealth).ToString());
            print(score - playerHit.playerHealth);
            //Load scene depends on how much karma player has
            if ((score - playerHit.playerHealth) < karmaMin)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1, LoadSceneMode.Single);
            }
            if((score - playerHit.playerHealth) > karmaMax)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1, LoadSceneMode.Single);
            }

            if (SceneManager.GetActiveScene().name == "Game_Over")
            {
                scoreText = GameObject.Find("yourScore").GetComponent<Text>();
                scoreText.text = "Body desecrated" + '\n' + "Your Score: " + score;
            }

            /*if (GameObject.FindGameObjectsWithTag("enemy").Length < 5)
            {
                print("Enemy eliminated");
                isGameEnd = true;
                scoreText.text="Your Time: " + '\n' + mintueCount + ":" + Mathf.RoundToInt(secondsCount);
                //Time.timeScale=0f;
            }*/

            if (timeLeft <= 0f)
            {
                SceneManager.LoadScene("Game_Over", LoadSceneMode.Single);
                scoreText.text = "Time Up" + '\n' + "Your Score: " + score;
            }


    }

        public void LoadGameOverScene() {
            //SceneManager.LoadScene(1, LoadSceneMode.Single);
            /*finalScoreText = GameObject.Find ("Final Score").GetComponent<Text> ();
            Debug.Log (finalScoreText.name);
            finalScoreText.text = "Final Score: " + score;*/
        }
    }
}
