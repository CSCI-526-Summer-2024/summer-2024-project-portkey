using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateBulletShooter : MonoBehaviour
{
    //helper variables for determining whether the its left player or right player
    public GameObject playerLeft;
    public GameObject playerRight;

    public float rotationSpeed = 100f; 
    public GameObject bulletPrefab; 
    public float bulletSpeed = 10f; 
    public Transform bulletSpawnPoint; 

    void Update()
    {
        // get the current rotation
        float rotation = transform.eulerAngles.z;

        //setting control keys for right player
        KeyCode upKey = KeyCode.UpArrow;
        KeyCode downKey = KeyCode.DownArrow;
        KeyCode shootKey = KeyCode.RightControl;

        //setting control keys for left player
        if (playerLeft != null)
        {
            upKey = KeyCode.W;
            downKey = KeyCode.S;
            shootKey = KeyCode.E;
        }

        // rotate the bullet clockwise/counter when user presses corresponding up/down keys
        if (Input.GetKey(upKey))
        {
            rotation += rotationSpeed * Time.deltaTime;
        }
        else if (Input.GetKey(downKey))
        {
            rotation -= rotationSpeed * Time.deltaTime;
        }

        transform.rotation = Quaternion.Euler(0, 0, rotation);

        // if players presses the key to shoot
        if (Input.GetKeyDown(shootKey))
        {
            Shoot();
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
