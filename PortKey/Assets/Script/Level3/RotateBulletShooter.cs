using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateBulletShooter : MonoBehaviour
{
    //helper variables for determining whether the its left player or right player
    public GameObject playerLeft;
    public GameObject playerRight;

    public float rotationSpeed = 2f; 
    public GameObject bulletPrefab; 
    public float bulletSpeed = 10f; 
    public Transform bulletSpawnPoint;
    string defaultRightPlayerName = "CarRight";
    string defaultLeftPlayerName = "CarLeft";

    float zRoatationRange = 70f;
    float rotation;
    Transform parent;

    void Start()
    {
        rotation = transform.eulerAngles.z;
        parent = transform.parent;
    }


    void Update()
    {
        //rotation = transform.eulerAngles.z;
        RotatePointer();
        RotatePoiterAndShootBullets();

    }

    void RotatePoiterAndShootBullets()
    {
        LeftCarShooting();
        RightCarShooting();
    }


    void RotatePointer()
    {

        rotation += rotationSpeed * Time.deltaTime;
        CheckRotationRange();
        transform.rotation = Quaternion.Euler(0, 0, rotation);
        
    }

    void CheckRotationRange()
    {
        if (rotation > zRoatationRange)
        {
            rotation = -zRoatationRange;
        }

        if (rotation < -zRoatationRange)
        {
            rotation = zRoatationRange;
        }

    }


    void LeftCarShooting()
    {
        //if (playerLeft != null && playerRight == null)
        if (parent.name == defaultLeftPlayerName)
        {
            KeyCode shootKey = KeyCode.S;

            // if left players presses the key to shoot
            if (Input.GetKeyDown(shootKey))
            {
                Shoot();
            }
        }
       
    }

    void RightCarShooting()
    {
        //if (playerLeft == null && playerRight != null)

        if(parent.name == defaultRightPlayerName)
        {
            KeyCode shootKey = KeyCode.UpArrow;

            // if right players presses the key to shoot
            if (Input.GetKeyDown(shootKey))
            {
                Shoot();
            }
        }
        
    }

    void Shoot()
    {

        // instantiate the bullet at the spawn point and sets it forward velocity
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.velocity = bulletSpawnPoint.up * bulletSpeed;
       
    }
}
