using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    readonly float distanceThreshold = 1500f;
    Vector3 lastPosition;

    // Start is called before the first frame update
    void Start()
    {
        lastPosition = GetComponent<Rigidbody2D>().transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        CheckOutOfBounds();
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
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }
}
