using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

using Proyecto26;
using UnityEngine.SceneManagement;

using PortKey.Assets.Script;


public class GameControllerTut : MonoBehaviour
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

    public TextMeshProUGUI player1;
    public TextMeshProUGUI player2;
    public Image controlLeft1;
    public Image controlRight1;
    public Image controlLeft2;
    public Image controlRight2;

    public Image spotlightLeft;
    public Image spotlightRight;
    public Image spotlightIconLeft1;
    public Image spotlightIconLeft2;
    public Image spotlightIconRight1;
    public Image spotlightIconRight2;

    private RectTransform controlLeft1Rect;
    private RectTransform controlRight1Rect;
    private RectTransform controlLeft2Rect;
    private RectTransform controlRight2Rect;
    private bool move = false;

    private float currentLeftScore = 0f;

    private float currentRightScore = 0f;

    private float scoreMultiplier = 1.0f;

    private readonly float baseScore = 1.0f;

    // game duration, unit is second
    private float gameDuration = 40f;

    //analytics helper variables
    public int totalCtrlSwitchPropCollectedRight = 0;
    public int totalCtrlSwitchPropCollectedLeft = 0;

    public int level;

    public int reasonforFinshingLevel;

    public int collisionDueToCtrlFlipLeft;
    public int collisionDueToCtrlFlipRight;

    public TextMeshProUGUI rightLostPointsMsg;
    public TextMeshProUGUI leftLostPointsMsg;

    private int bulletPropImpact = 6;

    void Awake()
    {
        if (rightLostPointsMsg != null)
        { rightLostPointsMsg.gameObject.SetActive(false); }

        if (leftLostPointsMsg != null)
        { leftLostPointsMsg.gameObject.SetActive(false); }
    }

    void Start()
    {

        spotlightLeft.enabled = false;
        spotlightRight.enabled = false;
        spotlightIconLeft1.enabled = false;
        spotlightIconLeft2.enabled = false;
        spotlightIconRight1.enabled = false;
        spotlightIconRight2.enabled = false;

        navArea.gameObject.SetActive(false);

        Time.timeScale = 1;

        if (leftScore != null)
        {
            InvokeRepeating("CalculateScoreLeft", 1, 1);
        }

        if (rightScore != null)
        {
            InvokeRepeating("CalculateScoreRight", 1, 1);
        }

        controlLeft1Rect = controlLeft1.GetComponent<RectTransform>();
        controlRight1Rect = controlRight1.GetComponent<RectTransform>();
        controlLeft2Rect = controlLeft2.GetComponent<RectTransform>();
        controlRight2Rect = controlRight2.GetComponent<RectTransform>();

        StartCoroutine(CountdownTimer());
    }


    void Update()
    {
        if (move)
        {
            controlLeft1Rect.Translate(new Vector3(0, -200.0f * Time.deltaTime, 0));
            controlRight1Rect.Translate(new Vector3(0, -200.0f * Time.deltaTime, 0));
            controlLeft2Rect.Translate(new Vector3(0, -200.0f * Time.deltaTime, 0));
            controlRight2Rect.Translate(new Vector3(0, -200.0f * Time.deltaTime, 0));
            if (controlLeft1Rect.anchoredPosition.y < -502)
            {
                move = false;
                controlLeft1.enabled = false;
                controlRight1.enabled = false;
                controlLeft2.enabled = false;
                controlRight2.enabled = false;
            }
        }
    }

    IEnumerator CountdownTimer()
    {
        while (gameDuration > 0)
        {
            TimerMsg.text = "" + Mathf.Ceil(gameDuration).ToString() + "s";
            if (gameDuration == 35)
            {
                StartCoroutine(FadeOutText(player1));
                StartCoroutine(FadeOutText(player2));
                move = true;
            }
            yield return new WaitForSeconds(1f);
            // Decrease game duration by 1 second
            gameDuration -= 1f;
        }
        TimerMsg.text = "0s";
        // Pause the game when the game duration is over
        PauseGame();
        reasonforFinshingLevel = 2;
        Anaytics();
        navArea.gameObject.SetActive(true);
        broadcastMsg.text = "CONTINUE TO\nLEVEL 1";
        broadcastMsg.color = Color.black;
        //SceneManager.LoadScene("Level1");
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
        if (carName == "CarLeft")
        {
            CarLeftStop();
        }
        else
        {
            CarRightStop();
        }
    }

    void Anaytics()
    {
        //parsing the current level number
        string levelName = SceneManager.GetActiveScene().name;
        char levelLastChar = levelName[levelName.Length - 1];
        int levelNumber;
        if (int.TryParse(levelLastChar.ToString(), out levelNumber))
        {
            Debug.Log("The last character as an integer: " + levelNumber);
        }
        else
        {
            Debug.LogWarning("The last character is not a valid number.");
        }

        //posting the analytics to the firebase
        PlayerData playerData = new PlayerData();
        playerData.name = "player";
        playerData.level = 0;
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

    public void EnemyControlReverse(string carName)
    {
        if (carName == "CarLeft")
        {
            carRight.GetComponent<CarMoveTut>().carSpeed *= -1;
            carRight.GetComponent<CarMoveTut>().reversed = !carRight.GetComponent<CarMoveTut>().reversed;
            Sprite oldleft = imageLeft.sprite;
            Sprite oldright = imageRight.sprite;
            imageLeft.sprite = oldright;
            imageRight.sprite = oldleft;
            StartCoroutine(Flashing(imageLeft, imageRight));
            StartCoroutine(Spotlight(spotlightIconRight1, 1.5f));
            StartCoroutine(Spotlight(spotlightIconRight2, 1.5f));
        }
        else
        {
            carLeft.GetComponent<CarMoveTut>().carSpeed *= -1;
            carLeft.GetComponent<CarMoveTut>().reversed = !carLeft.GetComponent<CarMoveTut>().reversed;
            Sprite oldA = imageA.sprite;
            Sprite oldD = imageD.sprite;
            imageA.sprite = oldD;
            imageD.sprite = oldA;
            StartCoroutine(Flashing(imageA, imageD));
            StartCoroutine(Spotlight(spotlightIconLeft1, 1.5f));
            StartCoroutine(Spotlight(spotlightIconLeft2, 1.5f));
        }
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
        carLeft.GetComponent<CarMoveTut>().canMove = false;

        // start CarRight movement
        carRight.GetComponent<CarMoveTut>().canMove = true;
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
        carRight.GetComponent<CarMoveTut>().canMove = false;

        // start CarLeft movement
        carLeft.GetComponent<CarMoveTut>().canMove = true;
    }

    public void StopScoreCalculation(string carName)
    {
        if (carName == "CarLeft")
        {
            CancelInvoke("CalculateScoreLeft");
        }
        else if (carName == "CarRight")
        {
            CancelInvoke("CalculateScoreRight");
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
        if (carName == "CarLeft")
        {
            currentLeftScore += 5;
            leftScore.text = "" + currentLeftScore.ToString("F0");
            StartCoroutine(FlashScore(leftScore, Color.magenta));
            StartCoroutine(Spotlight(spotlightLeft, 1.5f));
        }
        else
        {
            currentRightScore += 5;
            rightScore.text = "" + currentRightScore.ToString("F0");
            StartCoroutine(FlashScore(rightScore, Color.magenta));
            StartCoroutine(Spotlight(spotlightRight, 1.5f));
        }
    }

    private IEnumerator Spotlight(Image spotlight, float delay)
    {
        spotlight.enabled = true;
        yield return new WaitForSeconds(delay);
        spotlight.enabled = false;
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

    public void ActivateBonus()
    {
        StartCoroutine(Prop2BonusScore());
    }

    IEnumerator Prop2BonusScore()
    {
        scoreMultiplier = 2.0f;
        yield return new WaitForSeconds(5.0f);
        scoreMultiplier = 1.0f;
    }


    public void DisplayLeftLostPointsMsg()
    {
        BulletImpactForLeftPlayer();
        leftLostPointsMsg.color = Color.blue;
        leftLostPointsMsg.gameObject.SetActive(true);
        StartCoroutine(HideSwitchMessage(1f));
    }

    void BulletImpactForLeftPlayer()
    {
        if (currentLeftScore - bulletPropImpact < 0)
        {
            currentLeftScore = 0f;
        }
        else
        {
            currentLeftScore -= bulletPropImpact;
        }
        StartCoroutine(FlashScore(leftScore, Color.blue));
    }

    public void DisplayRightLostPointsMsg()
    {
        BulletImpactForRightPlayer();
        rightLostPointsMsg.color = Color.blue;
        rightLostPointsMsg.gameObject.SetActive(true);
        StartCoroutine(HideSwitchMessage(1f));
    }

    void BulletImpactForRightPlayer()
    {
        if (currentRightScore - bulletPropImpact < 0)
        {
            currentRightScore = 0f;
        }
        else
        {
            currentRightScore -= bulletPropImpact;
        }
        StartCoroutine(FlashScore(rightScore, Color.blue));
    }

    IEnumerator HideSwitchMessage(float delay)
    {
        yield return new WaitForSeconds(delay);
        rightLostPointsMsg.gameObject.SetActive(false);
        leftLostPointsMsg.gameObject.SetActive(false);
    }
}
