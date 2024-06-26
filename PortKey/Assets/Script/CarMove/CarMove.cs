using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CarMove : MonoBehaviour
{
    private string LEFT_CAR = "CarLeft";

    private string RIGHT_CAR = "CarRight";

    public float carSpeed = 300f;

    public bool actionAD = false;

    public float boundaryLeft = -850f;

    public float boundaryRight = -50f;

    public bool canMove = true;

    public TextMeshProUGUI deathText;

    public TextMeshProUGUI winText;

    public TextMeshProUGUI broadcastMsg;

    public Image navArea;

    public bool reversed = false;

    Quaternion originalRotation;
    Rigidbody2D playerRb;

    //healthbar helper variables
    float playerLefthealth = 100;
    float playerRighthealth = 100;
    float maxHealth = 100;
    float obstacleImpact = 15f;
    private HealthBar leftHealthBar;
    private HealthBar rightHealthBar;
    GameController gameController;

    void Start()
    {
        originalRotation = transform.localRotation;
        gameController = FindObjectOfType<GameController>();
        UploadHealthBars();
        FreezeRigidbodyMovements();
    }

    void Update()
    {
        MovePlayerCar();
    }

    void FreezeRigidbodyMovements()
    {
        playerRb = GetComponent<Rigidbody2D>();
        playerRb.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezePosition;
    }

    void UploadHealthBars()
    {

        leftHealthBar = GameObject.FindWithTag("LeftHealthBar").GetComponent<HealthBar>();
        rightHealthBar = GameObject.FindWithTag("RightHealthBar").GetComponent<HealthBar>();


        if (leftHealthBar != null)
        {
            leftHealthBar.UpdateLeftPlayerHealthBar(playerLefthealth, maxHealth);
        }
        else
        {
            Debug.LogError("Left HealthBar component not found.");
        }

        if (rightHealthBar != null)
        {
            rightHealthBar.UpdateRightPlayerHealthBar(playerRighthealth, maxHealth);
        }
        else
        {
            Debug.LogError("Left HealthBar component not found.");
        }
    }


    void MovePlayerCar()
    {
        if (canMove == true)
        {
            float posX = transform.position.x;
            if (transform.name == LEFT_CAR)
            {
                ReadLeftCarMovementInput(posX);

            }

            if (transform.name == RIGHT_CAR)
            {
                ReadRightCarMovementInput(posX);
            }
        }
    }


    void ReadLeftCarMovementInput(float posX)
    {
        if (!reversed)
        {
            if (Input.GetKey(KeyCode.A) && posX > boundaryLeft)
            {
                transform.Translate(Vector3.left * carSpeed * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.D) && posX < boundaryRight)
            {
                transform.Translate(Vector3.right * carSpeed * Time.deltaTime);
            }
        }
        else
        {
            if (Input.GetKey(KeyCode.A) && posX < boundaryRight)
            {
                transform.Translate(Vector3.left * carSpeed * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.D) && posX > boundaryLeft)
            {
                transform.Translate(Vector3.right * carSpeed * Time.deltaTime);
            }
        }
    }

    void ReadRightCarMovementInput(float posX)
    {
        if (!reversed)
        {
            if (Input.GetKey(KeyCode.LeftArrow) && posX > boundaryLeft)
            {
                transform.Translate(Vector3.left * carSpeed * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.RightArrow) && posX < boundaryRight)
            {
                transform.Translate(Vector3.right * carSpeed * Time.deltaTime);
            }
        }
        else
        {
            if (Input.GetKey(KeyCode.LeftArrow) && posX < boundaryRight)
            {
                transform.Translate(Vector3.left * carSpeed * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.RightArrow) && posX > boundaryLeft)
            {
                transform.Translate(Vector3.right * carSpeed * Time.deltaTime);
            }
        }
    }



    void UpdateHealthBars()
    {
        //decrement healthbar accordingly
        if (transform.name == LEFT_CAR)
        {
            playerLefthealth -= obstacleImpact;
            leftHealthBar.UpdateLeftPlayerHealthBar(playerLefthealth, maxHealth);
        }
        else
        {
            playerRighthealth -= obstacleImpact;
            rightHealthBar.UpdateRightPlayerHealthBar(playerRighthealth, maxHealth);
        }
    }

    void UpdateControlFlipMetric()
    {
        // Collisions after Control Flip Metric #4
        if (transform.name == LEFT_CAR)
        {
            gameController.collisionDueToCtrlFlipLeft += 1;
        }
        else
        {
            gameController.collisionDueToCtrlFlipRight += 1;
        }
    }

    void CheckPlayerHealth()
    {
        //updates the ui if one of the players lost all of their hp
        if (playerLefthealth <= 0 || playerRighthealth <= 0)
        {
            Time.timeScale = 0;
            gameController.StopFlashing();
            deathText.gameObject.SetActive(true);
            deathText.text = "YOU DIE";
            deathText.color = Color.red;
            winText.gameObject.SetActive(true);
            winText.text = "YOU WIN";
            winText.color = Color.green;

            navArea.gameObject.SetActive(true);
            broadcastMsg.text = "GAME OVER";
            broadcastMsg.color = Color.black;

            // Level Completion Reason Metric #2 
            gameController.reasonforFinshingLevel = 1;

            gameController.StopScoreCalculation(transform.name);
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.gameObject.tag == "Obstacle") //For Obstacle Collision
        {
            
            ShakePlayerObjectOnHealthLoss();
            UpdateHealthBars();
            Destroy(other.gameObject); //destroy the obstacle on collision
            UpdateControlFlipMetric();
            CheckPlayerHealth();
        }

        HandleEnemyControlReverseCollision(other); //For EnemyControlReverse Collision

        if (other.gameObject.name.Contains("ScoreUp")) //For ScoreUp Collision 
        {
            Destroy(other.gameObject);
            gameController.OneTimeBonus(transform.name);
        }

        HandleReduceEnemyHealthCollision(other); //For ReduceEnemyScore Collision 

        HandleSlowEnemyCollision(other); // For SlowEnemy Collision
    }

    void HandleEnemyControlReverseCollision(Collider2D other)
    {
        if (other.gameObject.name.Contains("EnemyControlReverse"))
        {
            DisplaySwitchMessage();
            Destroy(other.gameObject);
            gameController.EnemyControlReverse(transform.name);

            // Metric #3
            if (transform.name == LEFT_CAR)
            {
                gameController.totalCtrlSwitchPropCollectedLeft += 1;
            }
            else
            {
                gameController.totalCtrlSwitchPropCollectedRight += 1;
            }

        }

    }

    void HandleReduceEnemyHealthCollision(Collider2D other)
    { 
        if (other.gameObject.name.Contains("ReduceEnemyScore"))
        {
            Destroy(other.gameObject);
            //decrement opponents healthbar accordingly
            if (gameObject.name == RIGHT_CAR)
            {
                Debug.Log("Right player collected minus prop , reducing left health ");
                playerLefthealth -= obstacleImpact;
                leftHealthBar.UpdateLeftPlayerHealthBar(playerLefthealth, maxHealth);
                gameController.DisplayHealthStolenMsgForLeftPlayer();
                GameObject.Find(LEFT_CAR).GetComponent<CarMove>().ShakePlayerObjectOnHealthLoss();

            }
            else
            {
                Debug.Log("Left player collected minus prop , reducing right health ");
                playerRighthealth -= obstacleImpact;
                rightHealthBar.UpdateRightPlayerHealthBar(playerRighthealth, maxHealth);
                gameController.DisplayHealthStolenMsgForRightPlayer();
                GameObject.Find(RIGHT_CAR).GetComponent<CarMove>().ShakePlayerObjectOnHealthLoss();
            }
        }
    }


    void HandleSlowEnemyCollision(Collider2D other)
    {
        SpeedController speedController = FindObjectOfType<SpeedController>();
        if (other.gameObject.name.Contains("SlowEnemy"))
        {
            Destroy(other.gameObject);
            if (speedController == null)
            {
                Debug.LogError("SpeedController not found");
            }
            else
            {
                speedController.SlowDownCarTemporarily(transform.name, 0.5f, 4f);
                gameController.ShowSpeedSlowMsg(transform.name);
            }
        }
    }

    public void ShakePlayerObjectOnHealthLoss()
    {
        StartCoroutine(ShakePlayer());
    }

    IEnumerator ShakePlayer()
    {
        float time = 0.0f;
        //Quaternion originalRotation = transform.localRotation;
        float yLocation = transform.position.y;
        while (time < 0.5f)
        {
            float shake = Random.Range(-1f, 1f) * 6.0f;
            transform.localRotation = Quaternion.Euler(0, 0, originalRotation.eulerAngles.z + shake);
            time += Time.deltaTime;
            if (Time.timeScale == 0)
            {
                transform.localRotation = originalRotation;
            }
            yield return null;
        }
        transform.localRotation = originalRotation;
        transform.position =  new Vector2(transform.position.x,  yLocation);
    }


    void DisplaySwitchMessage()
    {
        winText.text = "CONTROLS SWITCHED!";
        winText.color = Color.blue;
        winText.gameObject.SetActive(true);
        StartCoroutine(HideSwitchMessage(1f));
    }

    IEnumerator HideSwitchMessage(float delay)
    {
        yield return new WaitForSeconds(delay);
        winText.gameObject.SetActive(false);
    }
}
