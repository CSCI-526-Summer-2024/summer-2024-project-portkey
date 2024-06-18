using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

using Proyecto26;

// for firebase analytics
[System.Serializable]
public class PlayerData
{
    public string name;
    public float score;
    public int level;
    public int reasonforFinshingLevel1; //1 = obstacle collision. 2= time up
    public int totalSwitchingPropCollected;
    public bool deathDueToControlsFlip;
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

    private float currentLeftScore = 0f;

    private float currentRightScore = 0f;

    private float scoreMultiplier = 1.0f;

    private readonly float baseScore = 1.0f;

    // game duration, unit is second
    private float gameDuration = 30f;

    //analytics helper variables
    public int totalSwitchingPropCollected = 0;

    public int level;

    public int reasonforFinshingLevel1;

    public bool deathDueToControlsFlip;

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
        navArea.gameObject.SetActive(false);
        level = GameLevelsManager.Instance.Level; // setting the level using GameLevelsManager.cs
        
        Time.timeScale = 1;

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
    }

    IEnumerator CountdownTimer()
    {
        while (gameDuration > 0)
        {
            TimerMsg.text = "Time Remaining: " + Mathf.Ceil(gameDuration).ToString() + "s";
            yield return new WaitForSeconds(1f);
            // Decrease game duration by 1 second
            gameDuration -= 1f;
        }
        TimerMsg.text = "Time Remaining: 0s";

        //  Metric #2 
        reasonforFinshingLevel1 = 2;
        //posting the analytics to the firebase
        Anaytics();

        // Pause the game when the game duration is over
        PauseGame();
        navArea.gameObject.SetActive(true);
        if (rightLostPointsMsg != null && leftLostPointsMsg != null)
        {
            leftLostPointsMsg.gameObject.SetActive(false);
            rightLostPointsMsg.gameObject.SetActive(false);
        }
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
        } else if (currentLeftScore < currentRightScore)
        {
            broadcastMsg.text = "TIMES UP!";
            broadcastMsg.color = Color.black;
            broadcastMsgRight.gameObject.SetActive(true);
            broadcastMsgRight.text = "YOU WIN";
            broadcastMsgRight.color = Color.green;
            broadcastMsgLeft.gameObject.SetActive(true);
            broadcastMsgLeft.text = "YOU LOSE";
            broadcastMsgLeft.color = Color.red;
        } else
        { //tie condition
            broadcastMsg.text = "TIMES UP!\nTIE!";
            broadcastMsg.color = Color.yellow;
        }

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

    public void EnemyControlReverse(string carName)
    {
        if (carName == "CarLeft")
        {
            carRight.GetComponent<CarMove>().carSpeed *= -1;
            carRight.GetComponent<CarMove>().reversed = !carRight.GetComponent<CarMove>().reversed;
            imageLeft.sprite = spriteRight;
            imageRight.sprite = spriteLeft;
            StartCoroutine(Flashing(imageLeft, imageRight));
        }
        else
        {
            carLeft.GetComponent<CarMove>().carSpeed *= -1;
            carLeft.GetComponent<CarMove>().reversed = !carLeft.GetComponent<CarMove>().reversed;
            imageA.sprite = spriteD;
            imageD.sprite = spriteA;
            StartCoroutine(Flashing(imageA, imageD));
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
        if (carName == "CarLeft")
        {
            CancelInvoke("CalculateScoreLeft");
        }
        else if (carName == "CarRight")
        {
            CancelInvoke("CalculateScoreRight");
        }

        //posting the analytics to the firebase
        Anaytics();

    }

    void Anaytics()
    {
        //posting the analytics to the firebase
        PlayerData playerData = new PlayerData();
        playerData.name = "player";
        playerData.level = level;
        playerData.score = currentRightScore + currentLeftScore;
        playerData.reasonforFinshingLevel1 = reasonforFinshingLevel1;
        playerData.totalSwitchingPropCollected = totalSwitchingPropCollected;
        playerData.deathDueToControlsFlip = deathDueToControlsFlip;

        string json = JsonUtility.ToJson(playerData);
        RestClient.Post("https://portkey-2a1ae-default-rtdb.firebaseio.com/metric2_analytics.json", playerData);
        Debug.Log("Analytics sent to firebase");
    }

    void CalculateScoreLeft()
    {
        currentLeftScore += baseScore * scoreMultiplier;
        leftScore.text = "Score: " + currentLeftScore.ToString("F0");
    }

    void CalculateScoreRight()
    {
        currentRightScore += baseScore * scoreMultiplier;
        rightScore.text = "Score: " + currentRightScore.ToString("F0");
    }

    public void OneTimeBonus(string carName)
    {
        if (carName == "CarLeft")
        {
            currentLeftScore += 5;
            leftScore.text = "Score: " + currentLeftScore.ToString("F0");
            StartCoroutine(FlashScore(leftScore, Color.magenta));
        }
        else
        {
            currentRightScore += 5;
            rightScore.text = "Score: " + currentRightScore.ToString("F0");
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
