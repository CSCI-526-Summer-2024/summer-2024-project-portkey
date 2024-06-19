using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletShooter : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        //  if collides with Obstacle, EnemyControlReverse, or ScoreUp prefabs
        if (collision.CompareTag("Obstacle") || collision.gameObject.name.Contains("EnemyControlReverse") ||
            collision.gameObject.name.Contains("ScoreUp"))
        {
            // destroy the objects
            Destroy(collision.gameObject);
            //Destroy(gameObject);
        }
    }
}
