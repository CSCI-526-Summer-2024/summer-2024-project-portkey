using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CarMove : MonoBehaviour
{
    private string defaultLeftCarName = "CarLeft";
    private string defaultRightCarName = "CarRight";

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

    public GameObject bulletPrefab;
    public float bulletSpeed = 300f;

    //healthbar helper variables
    float playerLefthealth = 100;
    float playerRighthealth = 100;
    float maxHealth = 100;
    float obstacleImpact = 20f;
    private HealthBar leftHealthBar;
    private HealthBar rightHealthBar;

    void Start()
    {
        UploadHealthBars();
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

    void Update()
    {
        //ShootBullet();
        if (!canMove)
        {
            return;
        }

        float posX = transform.position.x;

        if (transform.name == "CarLeft")
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

        if (transform.name == "CarRight")
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


    }

    void ShootBullet()
    {
        LeftCarShooting();
        RightCarShooting();
    }

    void LeftCarShooting()
    {
        if (transform.name == defaultLeftCarName)
        {
            if (Input.GetKeyDown(KeyCode.S) && bulletPrefab != null)
            {

                CreateBullet(transform);
            }

        }
    }


    void RightCarShooting()
    {
        if (transform.name == defaultRightCarName)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow) && bulletPrefab != null)
            {
                CreateBullet(transform);
            }
        }
    }

    void CreateBullet(Transform tmpRef)
    {
        var bullet = Instantiate(bulletPrefab, tmpRef.position, tmpRef.rotation);

        bullet.GetComponent<Rigidbody2D>().velocity = tmpRef.up * bulletSpeed;

        GameObject obj = GameObject.Find("Canvas");

        // Important for bullet to be displayed on canvas
        if (obj != null)
        {
            bullet.transform.SetParent(obj.transform);
        }
        else
        {
            Debug.Log("Canvas obj not found");
            bullet.transform.SetParent(tmpRef);
        }

        bullet.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        GameController gameController = FindObjectOfType<GameController>();

        if (other.gameObject.tag == "Obstacle")
        {

            //decrement healthbar accordingly
            if (transform.name == "CarLeft")
            {
                playerLefthealth -= obstacleImpact;
                leftHealthBar.UpdateLeftPlayerHealthBar(playerLefthealth, maxHealth);
            }
            else
            {
                playerRighthealth -= obstacleImpact;
                rightHealthBar.UpdateRightPlayerHealthBar(playerRighthealth, maxHealth);
            }
            //destroy the obstacle on collision
            Destroy(other.gameObject);

            // Collisions after Control Flip Metric #4
            if (transform.name == "CarLeft")
            {
                gameController.collisionDueToCtrlFlipLeft += 1;
            }
            else
            {
                gameController.collisionDueToCtrlFlipRight += 1;
            }

            //updates the ui if one of the players lost all of their hp
            if (playerLefthealth <= 0 || playerRighthealth <= 0)
            {
                Time.timeScale = 0;
                gameController.StopFlashing();
                deathText.gameObject.SetActive(true);
                deathText.text = "YOU LOSE";
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

        if (other.gameObject.name.Contains("EnemyControlReverse"))
        {
            DisplaySwithcMessage();
            Destroy(other.gameObject);
            gameController.EnemyControlReverse(transform.name);

            // Metric #3
            if (transform.name == "CarLeft")
            {
                gameController.totalCtrlSwitchPropCollectedLeft += 1;
            }
            else
            {
                gameController.totalCtrlSwitchPropCollectedRight += 1;
            }
           
        }

        if (other.gameObject.name.Contains("ScoreUp"))
        {
            Destroy(other.gameObject);
            gameController.OneTimeBonus(transform.name);
        }
    }

    void DisplaySwithcMessage()
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
