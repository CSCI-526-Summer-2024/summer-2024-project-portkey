using System.Collections;
using UnityEngine;
using TMPro;

public class CarMove : MonoBehaviour
{
    public float carSpeed = 300f;

    public bool actionAD = false;

    public float boundaryLeft = -850f;

    public float boundaryRight = -50f;

    public bool canMove = true;

    public TextMeshProUGUI deathText;

    public TextMeshProUGUI winText;

    public bool reversed = false;


    void Start()
    {
    }

    void Update()
    {
        if (!canMove)
        {
            return;
        }

        float posX = transform.GetComponent<RectTransform>().anchoredPosition.x;

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
            } else
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
            } else {
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
