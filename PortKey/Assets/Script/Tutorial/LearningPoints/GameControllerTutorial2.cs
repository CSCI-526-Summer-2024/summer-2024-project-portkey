using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

using Proyecto26;
using UnityEngine.SceneManagement;
using PortKey.Assets.Script.SwitchLevel;
using PortKey.Assets.Script;


public class GameControllerTutorial2 : MonoBehaviour
{
    public int levelNext;

    public Transform zoom1;

    public Transform zoom2;

    public TextMeshProUGUI leftScore;

    public TextMeshProUGUI rightScore;

    public TextMeshProUGUI leftMsg;

    public TextMeshProUGUI rightMsg;

    public TextMeshProUGUI broadcast;

    public TextMeshProUGUI broadcastMsg;

    public TextMeshProUGUI broadcastMsgLeft;

    public TextMeshProUGUI broadcastMsgRight;

    public TextMeshProUGUI TimerMsg;

    public Transform carLeft;

    public Transform carRight;

    public Sprite spriteA;

    public Sprite spriteD;

    public Image imageA;

    public Image imageD;

    public Sprite spriteLeft;

    public Sprite spriteRight;

    public Image imageLeft;

    public Image imageRight;

    public Image navArea;

    public Image leftMud;

    public Image rightMud;

    public Image spotlightLivesRight;
    public Image spotlightLivesLeft;

    public Image skullLeft;
    public Image skullRight;

    public Image spotlightMudLeft;
    public Image spotlightMudRight;

    public Image controlLeft1;
    public Image controlRight1;
    public Image controlLeft2;
    public Image controlRight2;

    private float currentLeftScore = 0f;

    private float currentRightScore = 0f;

    private float scoreMultiplier = 1.0f;

    private readonly float baseScore = 1.0f;

    // game duration, unit is second
    public float gameDuration = 20f;

    //analytics helper variables
    public int totalCtrlSwitchPropCollectedRight = 0;

    public int totalCtrlSwitchPropCollectedLeft = 0;

    public int level;

    public int reasonforFinshingLevel;

    public int collisionDueToCtrlFlipLeft;

    public int collisionDueToCtrlFlipRight;

    public TextMeshProUGUI LostHealthMsgRight;

    public TextMeshProUGUI LostHealthMsgLeft;

    public Image CountDownNavArea;
    public TextMeshProUGUI CountDownLeftText;

    public GameObject ShootLeft;
    public GameObject ShootRight;
    public Image spotLeft;
    public Image spotRight;
    public Image spotlightBulletsLeft;
    public Image spotlightBulletsRight;
    public TextMeshProUGUI plusLeft;
    public TextMeshProUGUI minusLeft;
    public TextMeshProUGUI plusRight;
    public TextMeshProUGUI minusRight;

    void Awake()
    {
        if (LostHealthMsgRight != null)
        { LostHealthMsgRight.gameObject.SetActive(false); }

        if (LostHealthMsgLeft != null)
        { LostHealthMsgLeft.gameObject.SetActive(false); }
    }


    void Start()
    {
        if (spotlightLivesLeft != null)
        {
            spotlightLivesRight.enabled = false;
            spotlightLivesLeft.enabled = false;
            skullLeft.enabled = false;
            skullRight.enabled = false;
        }
        if (spotlightMudLeft != null)
        {
            spotlightMudLeft.enabled = false;
            spotlightMudRight.enabled = false;
        }

        if (ShootLeft != null)
        {
            ShootLeft.gameObject.SetActive(false);
            ShootRight.gameObject.SetActive(false);
            spotLeft.enabled = false;
            spotRight.enabled = false;
            spotlightBulletsLeft.enabled = false;
            spotlightBulletsRight.enabled = false;
            plusLeft.enabled = false;
            minusLeft.enabled = false;
            plusRight.enabled = false;
            minusRight.enabled = false;
        }

        navArea.gameObject.SetActive(false);
        if (leftMud != null && rightMud != null)
        {
            leftMud.enabled = false;
            rightMud.enabled = false;
        }
        level = LevelInfo.Instance.Level;
        Time.timeScale = 1;

        AddDelayAndStartGame();
    }

    void AddDelayAndStartGame()
    {
        WaitAndStartGame();
    }

    void StopGame()
    {
        StopPlayer();
        StopSpawingProps();
    }

    void StartPlayer()
    {
        carLeft.GetComponent<CarMoveTutorial2>().canMove = true;
        carRight.GetComponent<CarMoveTutorial2>().canMove = true;
    }

    void StartSpawingProps()
    {
        //zoom1.GetComponent<SpawnObjController>().isStopSpawn = false;
        //zoom2.GetComponent<SpawnObjController>().isStopSpawn = false;
    }

    void StopPlayer()
    {
        carLeft.GetComponent<CarMoveTutorial2>().canMove = false;
        carRight.GetComponent<CarMoveTutorial2>().canMove = false;
    }

    void StopSpawingProps()
    {
        //zoom1.GetComponent<SpawnObjController>().isStopSpawn = true;
        //zoom2.GetComponent<SpawnObjController>().isStopSpawn = true;
    }

    void WaitAndStartGame()
    {
        StopGame();
        StartGame();
    }

    void StartGame()
    {
        StartPlayer();
        StartSpawingProps();

        if (leftScore != null)
        {
            InvokeRepeating("CalculateScoreLeft", 1, 1);
        }

        if (rightScore != null)
        {
            InvokeRepeating("CalculateScoreRight", 1, 1);
        }

        StartCoroutine(CountdownTimer());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            if (spotlightBulletsLeft != null)
            {
                StartCoroutine(SpotlightBullets(spotlightBulletsLeft, 1.5f, minusLeft));
            }
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (spotlightBulletsRight != null)
            {
                StartCoroutine(SpotlightBullets(spotlightBulletsRight, 1.5f, minusRight));
            }
        }
    }

    private IEnumerator SpotlightBullets(Image spotlight, float delay, TextMeshProUGUI minus)
    {
        spotlight.enabled = true;
        minus.enabled = true;
        yield return new WaitForSecondsRealtime(delay);
        spotlight.enabled = false;
        minus.enabled = false;
    }

    public void Bullets(string carName)
    {
        if (carName == ConstName.LEFT_CAR)
        {
            StartCoroutine(SpotlightBulletsLeft());
        }
        else
        {
            StartCoroutine(SpotlightBulletsRight());
        }
    }

    private IEnumerator SpotlightBulletsLeft()
    {
        spotlightBulletsLeft.enabled = true;
        plusLeft.enabled = true;
        yield return new WaitForSecondsRealtime(1.5f);
        spotlightBulletsLeft.enabled = false;
        plusLeft.enabled = false;
    }

    private IEnumerator SpotlightBulletsRight()
    {
        spotlightBulletsRight.enabled = true;
        plusRight.enabled = true;
        yield return new WaitForSecondsRealtime(1.5f);
        spotlightBulletsRight.enabled = false;
        plusRight.enabled = false;
    }

    IEnumerator CountdownTimer()
    {
        float timer = gameDuration;
        broadcast.gameObject.SetActive(true);
        while (gameDuration > 0)
        {
            TimerMsg.text = "" + Mathf.Ceil(gameDuration).ToString() + "s";
            if ((timer - gameDuration) == 2f)
            {
                broadcast.gameObject.SetActive(false);
            }
            if ((timer - gameDuration) == 5f && levelNext == 5)
            {
                StartCoroutine(PauseRight());
            }
            if ((timer - gameDuration) == 11f && levelNext == 5)
            {
                StartCoroutine(PauseLeft());
            }
            yield return new WaitForSeconds(1f);
            // Decrease game duration by 1 second
            gameDuration -= 1f;
        }
        TimerMsg.text = "0s";

        //  Metric #2 
        reasonforFinshingLevel = 2;
        //posting the analytics to the firebase
        //Anaytics();

        // Pause the game when the game duration is over
        PauseGame();
        navArea.gameObject.SetActive(true);
        if (LostHealthMsgRight != null && LostHealthMsgLeft != null)
        {
            LostHealthMsgLeft.gameObject.SetActive(false);
            LostHealthMsgRight.gameObject.SetActive(false);
        }
        //making sure everything that might be falshing will be visible!
        StopFlashing();
        broadcastMsg.text = "BEGIN\nLEVEL " + levelNext;
        broadcastMsg.color = Color.black;
        broadcastMsg.gameObject.SetActive(true);

    }

    IEnumerator PauseLeft()
    {
        Time.timeScale = 0;
        ShootLeft.gameObject.SetActive(true);
        spotLeft.enabled = true;
        while (!Input.GetKeyDown(KeyCode.W))
        {
            yield return null; // Wait for the next frame
        }
        Time.timeScale = 1;
        ShootLeft.gameObject.SetActive(false);
        spotLeft.enabled = false;
    }

    IEnumerator PauseRight()
    {
        Time.timeScale = 0;
        ShootRight.gameObject.SetActive(true);
        spotRight.enabled = true;
        while (!Input.GetKeyDown(KeyCode.UpArrow))
        {
            yield return null; // Wait for the next frame
        }
        Time.timeScale = 1;
        ShootRight.gameObject.SetActive(false);
        spotRight.enabled = false;
    }

    IEnumerator FadeOutText(TextMeshProUGUI text)
    {
        float time = 0f;
        while (time < 2.0f)
        {
            time += Time.deltaTime;
            float alpha = Mathf.Lerp(1, 0, time / 2.0f);
            text.color = new Color(Color.black.r, Color.black.g, Color.black.b, alpha);
            yield return null;
        }
        text.gameObject.SetActive(false);
    }

    public void StopFlashing()
    {
        leftScore.color = Color.black;
        leftScore.enabled = true;
        rightScore.color = Color.black;
        rightScore.enabled = true;
        imageLeft.GetComponentInChildren<TextMeshProUGUI>().color = Color.black;
        imageRight.GetComponentInChildren<TextMeshProUGUI>().color = Color.black;
        imageLeft.enabled = true;
        imageRight.enabled = true;
        imageA.GetComponentInChildren<TextMeshProUGUI>().color = Color.black;
        imageD.GetComponentInChildren<TextMeshProUGUI>().color = Color.black;
        imageA.enabled = true;
        imageD.enabled = true;
    }

    void PauseGame()
    {
        // Stop the game
        Time.timeScale = 0;
        Debug.Log("Game Paused! Time's up.");
    }

    public void SwitchScreen(string carName)
    {
        if (carName == ConstName.LEFT_CAR)
        {
            CarLeftStop();
        }
        else
        {
            CarRightStop();
        }
    }

    public void EnemyControlReverse(string carName)
    {
        if (carName == "CarLeft")
        {
            carRight.GetComponent<CarMoveTutorial2>().carSpeed *= -1;
            carRight.GetComponent<CarMoveTutorial2>().reversed = !carRight.GetComponent<CarMoveTutorial2>().reversed;
            Time.timeScale = 0;
            //StartCoroutine(Flashing(imageLeft, imageRight, flipRight));
            //StartCoroutine(Spotlight(spotlightIconRight1, 4f));
            //StartCoroutine(Spotlight(spotlightIconRight2, 4f));
        }
        else
        {
            carLeft.GetComponent<CarMoveTutorial2>().carSpeed *= -1;
            carLeft.GetComponent<CarMoveTutorial2>().reversed = !carLeft.GetComponent<CarMoveTutorial2>().reversed;
            Time.timeScale = 0;
            //StartCoroutine(Flashing(imageA, imageD, flipLeft));
            //StartCoroutine(Spotlight(spotlightIconLeft1, 4f));
            //StartCoroutine(Spotlight(spotlightIconLeft2, 4f));
        }
    }

    IEnumerator Flashing(Image left, Image right, Image flip)
    {
        for (int i = 0; i < 5; i++)
        {
            if (i == 2)
            {
                left.GetComponentInChildren<TextMeshProUGUI>().color = Color.green;
                right.GetComponentInChildren<TextMeshProUGUI>().color = Color.green;
                Sprite oldleft = left.sprite;
                Sprite oldright = right.sprite;
                left.sprite = oldright;
                right.sprite = oldleft;
                flip.enabled = true;
            }
            left.enabled = false;
            right.enabled = false;
            yield return new WaitForSecondsRealtime(0.4f);
            left.enabled = true;
            right.enabled = true;
            yield return new WaitForSecondsRealtime(0.4f);
        }
        left.GetComponentInChildren<TextMeshProUGUI>().color = Color.black;
        right.GetComponentInChildren<TextMeshProUGUI>().color = Color.black;
        flip.enabled = false;
        Time.timeScale = 1;
    }

    public void ShowSpeedSlowMsg(string carName)
    {
        if (carName == ConstName.LEFT_CAR)
        {
            //broadcastMsgRight.text = "Your speed has been reduced!";
            //broadcastMsgRight.gameObject.SetActive(true);
            //StartCoroutine(HideRightMessage());
            rightMud.enabled = true;
            spotlightMudRight.enabled = true;
            StartCoroutine(HideMud(rightMud, spotlightMudRight));
            StartCoroutine(shrinkIcons(controlRight1.rectTransform, controlRight2.rectTransform));
        }
        else
        {
            //broadcastMsgLeft.text = "Your speed has been reduced!";
            //broadcastMsgLeft.gameObject.SetActive(true);
            //StartCoroutine(HideLeftMessage());
            leftMud.enabled = true;
            spotlightMudLeft.enabled = true;
            StartCoroutine(HideMud(leftMud, spotlightMudLeft));
            StartCoroutine(shrinkIcons(controlLeft1.rectTransform, controlLeft2.rectTransform));
        }
    }

    private IEnumerator shrinkIcons(RectTransform control1, RectTransform control2)
    {
        Vector3 originalScale1 = control1.localScale;
        Vector3 originalScale2 = control2.localScale;
        Vector3 smallerScale1 = originalScale1 / 2f;
        Vector3 smallerScale2 = originalScale2 / 2f;
        control1.localScale = smallerScale1;
        control2.localScale = smallerScale2;
        yield return new WaitForSeconds(4f);
        control1.localScale = originalScale1;
        control2.localScale = originalScale2;
    }

    IEnumerator HideRightMessage()
    {
        yield return new WaitForSeconds(1f);
        broadcastMsgRight.gameObject.SetActive(false);
    }

    IEnumerator HideLeftMessage()
    {
        yield return new WaitForSeconds(1f);
        broadcastMsgLeft.gameObject.SetActive(false);
    }

    IEnumerator HideMud(Image mud, Image spotlight)
    {
        yield return new WaitForSeconds(4f);
        mud.enabled = false;
        spotlight.enabled = false;
    }

    public void CarLeftStop()
    {
        // stop zoom1 object movement
        zoom1.GetComponent<SpawnObjController>().isStopSpawn = true;
        foreach (Transform t in zoom1)
        {
            t.GetComponent<ObjMove>().speed = 0;
        }

        // start zoom2 object movement
        zoom2.GetComponent<SpawnObjController>().isStopSpawn = false;
        foreach (Transform t in zoom2)
        {
            t.GetComponent<ObjMove>().speed = t.GetComponent<ObjMove>().moveSpeed;
        }

        // stop CarLeft movement
        carLeft.GetComponent<CarMoveTutorial2>().canMove = false;

        // start CarRight movement
        carRight.GetComponent<CarMoveTutorial2>().canMove = true;
    }

    public void CarRightStop()
    {
        // stop zoom2 object movement
        zoom2.GetComponent<SpawnObjController>().isStopSpawn = true;
        foreach (Transform t in zoom2)
        {
            t.GetComponent<ObjMove>().speed = 0;
        }

        // start zoom1 object movement
        zoom1.GetComponent<SpawnObjController>().isStopSpawn = false;
        foreach (Transform t in zoom1)
        {
            t.GetComponent<ObjMove>().speed = t.GetComponent<ObjMove>().moveSpeed;
        }

        // stop CarRight movement
        carRight.GetComponent<CarMoveTutorial2>().canMove = false;

        // start CarLeft movement
        carLeft.GetComponent<CarMoveTutorial2>().canMove = true;
    }

    public void StopScoreCalculation(string carName)
    {
        if (carName == ConstName.LEFT_CAR)
        {
            CancelInvoke("CalculateScoreLeft");
        }
        else if (carName == ConstName.RIGHT_CAR)
        {
            CancelInvoke("CalculateScoreRight");
        }

        //posting the analytics to the firebase
        Anaytics();

    }

    void Anaytics()
    {
        //parsing the current level number
        string levelName = SceneManager.GetActiveScene().name;
        char levelLastChar = levelName[levelName.Length - 1];
        int levelNumber;
        int.TryParse(levelLastChar.ToString(), out levelNumber);

        //posting the analytics to the firebase
        PlayerData playerData = new PlayerData();
        playerData.name = "player";
        playerData.level = levelNumber;
        playerData.scoreLeft = currentLeftScore;
        playerData.scoreRight = currentRightScore;
        playerData.reasonforFinshingLevel = reasonforFinshingLevel;
        playerData.totalCtrlSwitchPropCollectedRight = totalCtrlSwitchPropCollectedRight;
        playerData.totalCtrlSwitchPropCollectedLeft = totalCtrlSwitchPropCollectedLeft;
        playerData.collisionDueToCtrlFlipLeft = collisionDueToCtrlFlipLeft;
        playerData.collisionDueToCtrlFlipRight = collisionDueToCtrlFlipRight;

        //string json = JsonUtility.ToJson(playerData);

        //if (ConstName.SEND_ANALYTICS == true)
        //{
        //    RestClient.Post("https://portkey-2a1ae-default-rtdb.firebaseio.com/playtesting1_analytics.json", playerData);
        //    Debug.Log("Analytics sent to firebase");
        //}
    }

    void CalculateScoreLeft()
    {
        currentLeftScore += baseScore * scoreMultiplier;
        leftScore.text = "" + currentLeftScore.ToString("F0");
    }

    void CalculateScoreRight()
    {
        currentRightScore += baseScore * scoreMultiplier;
        rightScore.text = "" + currentRightScore.ToString("F0");
    }

    public void OneTimeBonus(string carName)
    {
        if (carName == "CarLeft")
        {
            Time.timeScale = 0;
            //StartCoroutine(FlashScore(leftScore, new Color(1.0f, 0.788f, 0.282f), true, fiveLeft));
            //StartCoroutine(Spotlight(spotlightLeft, 4f));
        }
        else
        {
            Time.timeScale = 0;
            //StartCoroutine(FlashScore(rightScore, new Color(1.0f, 0.788f, 0.282f), false, fiveRight));
            //StartCoroutine(Spotlight(spotlightRight, 4f));
        }
    }

    private IEnumerator Spotlight(Image spotlight, float delay)
    {
        spotlight.enabled = true;
        yield return new WaitForSecondsRealtime(delay);
        spotlight.enabled = false;
    }

    private IEnumerator SpotlightEnemy(Image spotlight, float delay, Image skull)
    {
        spotlight.enabled = true;
        yield return new WaitForSecondsRealtime(0.5f);
        skull.enabled = true;
        yield return new WaitForSecondsRealtime(delay - 1.0f);
        skull.enabled = false;
        yield return new WaitForSecondsRealtime(0.5f);
        spotlight.enabled = false;
    }

    public void SpotlightLives(string carName, bool flipped)
    {
        if (flipped)
        {
            if (carName == ConstName.LEFT_CAR)
            {
                StartCoroutine(SpotlightEnemy(spotlightLivesRight, 3f, skullRight));
            }
            else
            {
                StartCoroutine(SpotlightEnemy(spotlightLivesLeft, 3f, skullLeft));
            }
        }
        else
        {
            if (carName == ConstName.LEFT_CAR)
            {
                StartCoroutine(Spotlight(spotlightLivesLeft, 2f));
            }
            else
            {
                StartCoroutine(Spotlight(spotlightLivesRight, 2f));
            }
        }
    }

    IEnumerator FlashScore(TextMeshProUGUI score, Color col, bool left, TextMeshProUGUI five)
    {
        for (int i = 0; i < 5; i++)
        {
            if (i == 2)
            {
                score.color = col;
                if (left)
                {
                    currentLeftScore += 5;
                    score.text = currentLeftScore.ToString("F0");
                }
                else
                {
                    currentRightScore += 5;
                    score.text = currentRightScore.ToString("F0");
                }
                five.gameObject.SetActive(true);
            }
            score.enabled = false;
            yield return new WaitForSecondsRealtime(0.4f);
            score.enabled = true;
            yield return new WaitForSecondsRealtime(0.4f);
        }
        score.color = Color.black;
        five.gameObject.SetActive(false);
        Time.timeScale = 1;
    }


    public void DisplayRightLostHealthMsg()
    {
        //BulletImpactForRightPlayer();
        LostHealthMsgRight.text = "Health Stolen";
        LostHealthMsgRight.color = Color.blue;
        LostHealthMsgRight.gameObject.SetActive(true);
        StartCoroutine(HideStolenHealthMessage(1f));
    }

    public void DisplayLeftLostHealthMsg()
    {
        //BulletImpactForLeftPlayer();
        LostHealthMsgLeft.text = "Health Stolen";
        LostHealthMsgLeft.color = Color.blue;
        LostHealthMsgLeft.gameObject.SetActive(true);
        StartCoroutine(HideStolenHealthMessage(1f));
    }


    IEnumerator HideStolenHealthMessage(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (LostHealthMsgRight != null && LostHealthMsgLeft != null)
        {
            LostHealthMsgLeft.gameObject.SetActive(false);
            LostHealthMsgRight.gameObject.SetActive(false);
        }
    }

    public void UpdateHealthBarOnCollision(string carName, float impact, bool impactOppositePlayer = false)
    {
        //decrement healthBar accordingly
        if (impactOppositePlayer == false)
        {

            if (carName == ConstName.LEFT_CAR)
            {
                carLeft.GetComponent<CarMoveTutorial2>().playerHealth += impact;
                float currentHealth = carLeft.GetComponent<CarMoveTutorial2>().playerHealth;
                float maxHealth = carLeft.GetComponent<CarMoveTutorial2>().maxHealth;
                carLeft.GetComponent<CarMoveTutorial2>().healthBar.UpdateLeftPlayerHealthBar(currentHealth, maxHealth);
                carLeft.GetComponent<CarMoveTutorial2>().ShakePlayerOnHealthLoss();
            }
            else
            {
                carRight.GetComponent<CarMoveTutorial2>().playerHealth += impact;
                float currentHealth = carRight.GetComponent<CarMoveTutorial2>().playerHealth;
                float maxHealth = carRight.GetComponent<CarMoveTutorial2>().maxHealth;
                carRight.GetComponent<CarMoveTutorial2>().healthBar.UpdateRightPlayerHealthBar(currentHealth, maxHealth);
                carRight.GetComponent<CarMoveTutorial2>().ShakePlayerOnHealthLoss();
            }
        }
        else
        {
            if (carName != ConstName.LEFT_CAR)
            {
                carLeft.GetComponent<CarMoveTutorial2>().playerHealth += impact;
                float currentHealth = carLeft.GetComponent<CarMoveTutorial2>().playerHealth;
                float maxHealth = carLeft.GetComponent<CarMoveTutorial2>().maxHealth;
                carLeft.GetComponent<CarMoveTutorial2>().healthBar.UpdateLeftPlayerHealthBar(currentHealth, maxHealth);
                carLeft.GetComponent<CarMoveTutorial2>().ShakePlayerOnHealthLoss();
            }
            else
            {
                carRight.GetComponent<CarMoveTutorial2>().playerHealth += impact;
                float currentHealth = carRight.GetComponent<CarMoveTutorial2>().playerHealth;
                float maxHealth = carRight.GetComponent<CarMoveTutorial2>().maxHealth;
                carRight.GetComponent<CarMoveTutorial2>().healthBar.UpdateRightPlayerHealthBar(currentHealth, maxHealth);
                carRight.GetComponent<CarMoveTutorial2>().ShakePlayerOnHealthLoss();
            }
        }

    }
}
