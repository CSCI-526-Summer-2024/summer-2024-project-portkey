using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateBulletShooter : MonoBehaviour
{
    // Helper variables for determining whether it's left player or right player
    public GameObject playerLeft;
    public GameObject playerRight;

    public float rotationSpeed = .5f;
    public GameObject bulletPrefab;
    public float bulletSpeed = 100f;
    public Transform bulletSpawnPoint;
    string defaultRightPlayerName = "CarRight";
    string defaultLeftPlayerName = "CarLeft";

    float zRotationRange = 80f;
    float rotation;
    Transform parent;
    int reversed = 1;

    public int currentBulletsLeft = 7;
    public int currentBulletsRight = 7;

    void Start()
    {
        rotation = transform.eulerAngles.z;
        parent = transform.parent;
    }

    void Update()
    {
        RotatePointer();
        RotatePointerAndShootBullets();
    }

    void RotatePointerAndShootBullets()
    {
        LeftCarShooting();
        RightCarShooting();
    }

    void RotatePointer()
    {
        rotation += rotationSpeed * Time.deltaTime * reversed;
        CheckRotationRange();
        transform.rotation = Quaternion.Euler(0, 0, rotation);
    }

    void CheckRotationRange()
    {
        if (rotation > zRotationRange)
        {
            rotation = zRotationRange;
            reversed = -1;
        }

        if (rotation < -zRotationRange)
        {
            rotation = -zRotationRange;
            reversed = 1;
        }
    }

    void LeftCarShooting()
    {
        //if (playerLeft != null && playerRight == null)
        if (parent.name == defaultLeftPlayerName)
        {
            KeyCode shootKey = KeyCode.W;

            // if left players presses the key to shoot
            if (Input.GetKeyDown(shootKey))
            {
                Shoot("left");
            }
        }
    }

    void RightCarShooting()
    {
        //if (playerLeft == null && playerRight != null)
        if (parent.name == defaultRightPlayerName)
        {
            KeyCode shootKey = KeyCode.UpArrow;

            // if right players presses the key to shoot
            if (Input.GetKeyDown(shootKey))
            {
                Shoot("right");
            }
        }
    }
    void Shoot(string isLeftOrRight)
    {
        int currentBullet = (isLeftOrRight == "left") ? currentBulletsLeft : currentBulletsRight;

        // Only shoot if designated player has enough bullets
        if (currentBullet > 0)
        {
            // Instantiate the bullet at the spawn point and set its forward velocity
            GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.velocity = bulletSpawnPoint.up * bulletSpeed;

            if (isLeftOrRight == "left")
            {
                currentBulletsLeft -= 1;
            }
            else
            {
                currentBulletsRight -= 1;
            }
        }
    }


    public void IncreaseBulletCountLeft()
    {
        currentBulletsLeft += 1;
    }

    public void IncreaseBulletCountRight()
    {
        currentBulletsRight += 1;
    }

    public void InitializeBullets()
    {
        currentBulletsLeft = 7;
        currentBulletsRight = 7;
    }
}