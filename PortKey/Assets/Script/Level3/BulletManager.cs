using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    public float speed = 5f;
    readonly float distanceThreshold = 1500f;
    Rigidbody2D rb;

    Vector3 lastPosition;


    // Start is called before the first frame update
    void Start()
    {
        lastPosition = GetComponent<Rigidbody2D>().transform.position;
        rb = GetComponent<Rigidbody2D>();
        rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
    }

    // Update is called once per frame
    void Update()
    {
        MoveDown();
        CheckOutOfBounds();
    }

    void MoveDown()
    {
        transform.Translate(Vector3.down * speed * Time.deltaTime);
    }


    void CheckOutOfBounds()
    {
        /*
        for some unknown reason using x and y positions
        to destroy out of bounds was not working correctly.
        so using distance for now.
        */
        float distanceFromPlayer = Vector3.Distance(lastPosition, transform.position);
        if (distanceFromPlayer >= distanceThreshold)
        {
            Destroy(gameObject);
            // Debug.Log("Bullet destroyed at y: " + transform.position.y + ", distance: " + distanceFromPlayer);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Debug.Log("Bullet Trigger: "+collision.name);

        //if (collision.gameObject.CompareTag("Obstacle"))
        //{
        //    Destroy(collision.gameObject);
        //    Destroy(gameObject);
        //}

        if (collision.gameObject.CompareTag("Player") || collision.CompareTag("Player"))
        {
            if (collision.name == "CarLeft")
            {
                Debug.Log("Deduct Right Player Score");
                GameObject.Find("GameContorller").GetComponent<GameController>().DisplayRightLostPointsMsg();
            }
            else
            {
                GameObject.Find("GameContorller").GetComponent<GameController>().DisplayLeftLostPointsMsg();
                Debug.Log("Deduct Left Player Score");
            }

            Destroy(gameObject);
        }
    }
}
