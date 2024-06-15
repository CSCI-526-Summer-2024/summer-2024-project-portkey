using System.Collections;
using UnityEngine;
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

    public bool reversed = false;

    public GameObject bulletPrefab;
    public float bulletSpeed = 300f;


    void Start()
    {
    }

    void Update()
    {
        ShootBullet();
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
            Time.timeScale = 0;
            deathText.gameObject.SetActive(true);
            deathText.text = "YOU LOSE";
            deathText.color = Color.red;
            winText.gameObject.SetActive(true);
            winText.text = "YOU WIN";
            winText.color = Color.green;

            // Deaths after Control Flip Metric #4
            gameController.deathDueToControlsFlip = reversed;
            // First Level Completion Metric #2 
            gameController.reasonforFinshingLevel1 = 1;

            gameController.StopScoreCalculation(transform.name);
        }

        if (other.gameObject.name.Contains("EnemyControlReverse"))
        {
            DisplaySwithcMessage();
            Destroy(other.gameObject);
            gameController.EnemyControlReverse(transform.name);

            // Metric #3
            gameController.totalSwitchingPropCollected += 1;
        }

        if (other.gameObject.name.Contains("ScoreUp"))
        {
            Destroy(other.gameObject);
            gameController.OneTimeBonus(transform.name);
        }
    }

    public void DisplaySwithcMessage()
    {
        winText.text = "CONTROLS SWITCHED!";
        winText.color = Color.blue;
        winText.gameObject.SetActive(true);
        StartCoroutine(HideSwitchMessage(1f));
    }

    private IEnumerator HideSwitchMessage(float delay)
    {
        yield return new WaitForSeconds(delay);
        winText.gameObject.SetActive(false);
    }
}
