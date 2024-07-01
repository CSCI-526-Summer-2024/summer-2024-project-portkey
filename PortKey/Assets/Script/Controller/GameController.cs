using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

using Proyecto26;
using UnityEngine.SceneManagement;
using PortKey.Assets.Script.SwitchLevel;
using PortKey.Assets.Script;

// for firebase analytics
[System.Serializable]
public class PlayerData
{
    public string name;
    public float scoreLeft;
    public float scoreRight;
    public int level;
    public int reasonforFinshingLevel; //1 = obstacle collision. 2= time up
    public int totalCtrlSwitchPropCollectedLeft;
    public int totalCtrlSwitchPropCollectedRight;
    public int collisionDueToCtrlFlipLeft;
    public int collisionDueToCtrlFlipRight;
}


public class GameController : MonoBehaviour
{
    public Transform zoom1;

    public Transform zoom2;

    public TextMeshProUGUI leftScore;

    public TextMeshProUGUI rightScore;

    public TextMeshProUGUI leftMsg;

    public TextMeshProUGUI rightMsg;

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

    private float currentLeftScore = 0f;

    private float currentRightScore = 0f;

    private float scoreMultiplier = 1.0f;

    private readonly float baseScore = 1.0f;

    // game duration, unit is second
    private float gameDuration = 60f;

    //analytics helper variables
    public int totalCtrlSwitchPropCollectedRight = 0;

    public int totalCtrlSwitchPropCollectedLeft = 0;

    public int level;

    public int reasonforFinshingLevel;

    public int collisionDueToCtrlFlipLeft;

    public int collisionDueToCtrlFlipRight;

    public TextMeshProUGUI LostHealthMsgRight;

    public TextMeshProUGUI LostHealthMsgLeft;

    int countDownBeforeStartDuration = 3;

    public Image CountDownNavArea;
    public TextMeshProUGUI CountDownLeftText;

    public Image ShootingTextNavAreaLeft;
    public Image ShootingTextNavAreaRight;
    public TextMeshProUGUI ShootingTextLeft;
    public TextMeshProUGUI ShootingTextRight;

    void Awake()
    {
        if (LostHealthMsgRight != null)
        { LostHealthMsgRight.gameObject.SetActive(false); }

        if (LostHealthMsgLeft != null)
        { LostHealthMsgLeft.gameObject.SetActive(false); }
    }


    void Start()
    {
        DisplayCountDown();
        StartCoroutine(HideShootingText());
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

    string GetMessageToDisplay(int timeLeft)
    {
        string msgToDisplay;
        if (timeLeft == 3)
        {
            msgToDisplay = timeLeft.ToString() + "\n" + "READY";
        }
        else if (timeLeft == 2)
        {
            msgToDisplay = timeLeft.ToString() + "\n" + "SET";
        }
        else if (timeLeft == 1)
        {
            msgToDisplay = timeLeft.ToString() + "\n" + "GO";
        }
        else
        {
            msgToDisplay = timeLeft.ToString();
        }
        return msgToDisplay;
    }

    IEnumerator HideCountDownMsg()
    {
        if (CountDownLeftText != null)
        {
            CountDownLeftText.text = "SURVIVE AND SCORE HIGHER";
            yield return new WaitForSeconds(0.5f);
            int time = countDownBeforeStartDuration;
            while (time > 0)
            {
                CountDownLeftText.text = GetMessageToDisplay(time);
                yield return new WaitForSeconds(1f);
                time -= 1;
            }
            CountDownLeftText.gameObject.SetActive(false);

        }
        if (CountDownNavArea != null)
        {
            CountDownNavArea.gameObject.SetActive(false);
        }
    }
    
    IEnumerator HideShootingText()
    {

        yield return new WaitForSeconds(3); // Wait for 3 seconds
        if (ShootingTextNavAreaLeft != null)
        {
            ShootingTextNavAreaLeft.gameObject.SetActive(false);
        }
        if (ShootingTextNavAreaRight != null)
        {
            ShootingTextNavAreaRight.gameObject.SetActive(false);
        }
        if (ShootingTextLeft != null)
        {
            ShootingTextLeft.gameObject.SetActive(false);
        }
        if (ShootingTextRight != null)
        {
            ShootingTextRight.gameObject.SetActive(false);
        }
    }

    void DisplayCountDown()
    {
        StartCoroutine(HideCountDownMsg());
    }

    void AddDelayAndStartGame()
    {
        StartCoroutine(WaitAndStartGame());
    }

    void StopGame()
    {
        StopPlayer();
        StopSpawingProps();
    }

    void StartPlayer()
    {
        carLeft.GetComponent<CarMove>().canMove = true;
        carRight.GetComponent<CarMove>().canMove = true;
    }

    void StartSpawingProps()
    {
        zoom1.GetComponent<SpawnObjController>().isStopSpawn = false;
        zoom2.GetComponent<SpawnObjController>().isStopSpawn = false;
    }

    void StopPlayer()
    {
        carLeft.GetComponent<CarMove>().canMove = false;
        carRight.GetComponent<CarMove>().canMove = false;
    }

    void StopSpawingProps()
    {
        zoom1.GetComponent<SpawnObjController>().isStopSpawn = true;
        zoom2.GetComponent<SpawnObjController>().isStopSpawn = true;
    }

    IEnumerator WaitAndStartGame()
    {
        StopGame();
        yield return new WaitForSeconds(countDownBeforeStartDuration);
        StartGame();
    }

    void Update()
    {
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


    IEnumerator CountdownTimer()
    {
        while (gameDuration > 0)
        {
            TimerMsg.text = "" + Mathf.Ceil(gameDuration).ToString() + "s";
            yield return new WaitForSeconds(1f);
            // Decrease game duration by 1 second
            gameDuration -= 1f;
        }
        TimerMsg.text = "0s";

        //  Metric #2 
        reasonforFinshingLevel = 2;
        //posting the analytics to the firebase
        Anaytics();

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
        if (currentLeftScore > currentRightScore)
        {
            broadcastMsg.text = "TIMES UP!";
            broadcastMsg.color = Color.black;
            broadcastMsgLeft.gameObject.SetActive(true);
            broadcastMsgLeft.text = "YOU WIN";
            broadcastMsgLeft.color = Color.green;
            broadcastMsgRight.gameObject.SetActive(true);
            broadcastMsgRight.text = "YOU LOSE";
            broadcastMsgRight.color = Color.red;
        }
        else if (currentLeftScore < currentRightScore)
        {
            broadcastMsg.text = "TIMES UP!";
            broadcastMsg.color = Color.black;
            broadcastMsgRight.gameObject.SetActive(true);
            broadcastMsgRight.text = "YOU WIN";
            broadcastMsgRight.color = Color.green;
            broadcastMsgLeft.gameObject.SetActive(true);
            broadcastMsgLeft.text = "YOU LOSE";
            broadcastMsgLeft.color = Color.red;
        }
        else
        { //tie condition
            broadcastMsg.text = "TIMES UP!\nTIE!";
            broadcastMsg.color = Color.yellow;
        }

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
        if (carName == ConstName.LEFT_CAR)
        {
            carRight.GetComponent<CarMove>().carSpeed *= -1;
            carRight.GetComponent<CarMove>().reversed = !carRight.GetComponent<CarMove>().reversed;
            Sprite oldleft = imageLeft.sprite;
            Sprite oldright = imageRight.sprite;
            imageLeft.sprite = oldright;
            imageRight.sprite = oldleft;
            StartCoroutine(Flashing(imageLeft, imageRight));
        }
        else
        {
            //Debug.Log(ConstName.RIGHT_CAR);
            //Debug.Log("Before: " + carLeft.GetComponent<CarMove>().carSpeed);
            carLeft.GetComponent<CarMove>().carSpeed *= -1;
            carLeft.GetComponent<CarMove>().reversed = !carLeft.GetComponent<CarMove>().reversed;
            Sprite oldA = imageA.sprite;
            Sprite oldD = imageD.sprite;
            imageA.sprite = oldD;
            imageD.sprite = oldA;
            StartCoroutine(Flashing(imageA, imageD));
        }
    }

    public void ShowSpeedSlowMsg(string carName)
    {
        if (carName == ConstName.LEFT_CAR)
        {
            //broadcastMsgRight.text = "Your speed has been reduced!";
            //broadcastMsgRight.gameObject.SetActive(true);
            //StartCoroutine(HideRightMessage());
            rightMud.enabled = true;
            StartCoroutine(HideMud(rightMud));
        }
        else
        {
            //broadcastMsgLeft.text = "Your speed has been reduced!";
            //broadcastMsgLeft.gameObject.SetActive(true);
            //StartCoroutine(HideLeftMessage());
            leftMud.enabled = true;
            StartCoroutine(HideMud(leftMud));
        }
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

    IEnumerator HideMud(Image mud)
    {
        yield return new WaitForSeconds(4f);
        mud.enabled = false;
    }

    IEnumerator Flashing(Image left, Image right)
    {
        left.GetComponentInChildren<TextMeshProUGUI>().color = Color.green;
        right.GetComponentInChildren<TextMeshProUGUI>().color = Color.green;
        for (int i = 0; i < 3; i++)
        {
            left.enabled = false;
            right.enabled = false;
            yield return new WaitForSeconds(0.2f);
            left.enabled = true;
            right.enabled = true;
            yield return new WaitForSeconds(0.2f);
        }
        left.GetComponentInChildren<TextMeshProUGUI>().color = Color.black;
        right.GetComponentInChildren<TextMeshProUGUI>().color = Color.black;
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
        carLeft.GetComponent<CarMove>().canMove = false;

        // start CarRight movement
        carRight.GetComponent<CarMove>().canMove = true;
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
        carRight.GetComponent<CarMove>().canMove = false;

        // start CarLeft movement
        carLeft.GetComponent<CarMove>().canMove = true;
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

        if (ConstName.SEND_ANALYTICS == true)
        {
            RestClient.Post("https://portkey-2a1ae-default-rtdb.firebaseio.com/playtesting1_analytics.json", playerData);
            Debug.Log("Analytics sent to firebase");
        }
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
        if (carName == ConstName.LEFT_CAR)
        {
            currentLeftScore += 5;
            leftScore.text = "" + currentLeftScore.ToString("F0");
            StartCoroutine(FlashScore(leftScore, Color.magenta));
        }
        else
        {
            currentRightScore += 5;
            rightScore.text = "" + currentRightScore.ToString("F0");
            StartCoroutine(FlashScore(rightScore, Color.magenta));
        }
    }

    IEnumerator FlashScore(TextMeshProUGUI score, Color col)
    {
        score.color = col;
        for (int i = 0; i < 3; i++)
        {
            score.enabled = false;
            yield return new WaitForSeconds(0.2f);
            score.enabled = true;
            yield return new WaitForSeconds(0.2f);
        }
        score.color = Color.black;
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
                carLeft.GetComponent<CarMove>().playerHealth += impact;
                float currentHealth = carLeft.GetComponent<CarMove>().playerHealth;
                float maxHealth = carLeft.GetComponent<CarMove>().maxHealth;
                carLeft.GetComponent<CarMove>().healthBar.UpdateLeftPlayerHealthBar(currentHealth, maxHealth);
                carLeft.GetComponent<CarMove>().ShakePlayerOnHealthLoss();
            }
            else
            {
                carRight.GetComponent<CarMove>().playerHealth += impact;
                float currentHealth = carRight.GetComponent<CarMove>().playerHealth;
                float maxHealth = carRight.GetComponent<CarMove>().maxHealth;
                carRight.GetComponent<CarMove>().healthBar.UpdateRightPlayerHealthBar(currentHealth, maxHealth);
                carRight.GetComponent<CarMove>().ShakePlayerOnHealthLoss();
            }
        }
        else
        {
            if (carName != ConstName.LEFT_CAR)
            {
                carLeft.GetComponent<CarMove>().playerHealth += impact;
                float currentHealth = carLeft.GetComponent<CarMove>().playerHealth;
                float maxHealth = carLeft.GetComponent<CarMove>().maxHealth;
                carLeft.GetComponent<CarMove>().healthBar.UpdateLeftPlayerHealthBar(currentHealth, maxHealth);
                carLeft.GetComponent<CarMove>().ShakePlayerOnHealthLoss();
            }
            else
            {
                carRight.GetComponent<CarMove>().playerHealth += impact;
                float currentHealth = carRight.GetComponent<CarMove>().playerHealth;
                float maxHealth = carRight.GetComponent<CarMove>().maxHealth;
                carRight.GetComponent<CarMove>().healthBar.UpdateRightPlayerHealthBar(currentHealth, maxHealth);
                carRight.GetComponent<CarMove>().ShakePlayerOnHealthLoss();
            }
        }

    }
}
