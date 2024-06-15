using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpawnObjController : MonoBehaviour
{
    public GameObject obstacleLeft;

    public GameObject obstacleRight;

    public GameObject EnemyControlReverse;

    public GameObject ScoreUp;

    public bool isStopSpawn = false;

    public float leftOffset = -200f;

    public float rightOffset = 200f;

    private readonly float[] spawnObstacleTime = { 2, 3 };

    private readonly float[] spawnProp1Time = { 5, 8 };

    private readonly float[] spawnProp2Time = { 5, 8 };

    private bool firstSpawn = true;
    GameObject lastPos;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnObstacle());
        StartCoroutine(SpawnProp1());
        StartCoroutine(SpawnProp2());
    }

    // Update is called once per frame
    void Update()
    {
    }

    IEnumerator SpawnObstacle()
    {
        // wait for a while before spawning the next obstacle
        yield return new WaitForSeconds(3);

        while (true)
        {
            // Wait for a while before spawning the next obstacle
            yield return new WaitUntil(() => !isStopSpawn);

            // Randomly spawn objects
            yield return new WaitForSeconds(Random.Range(spawnObstacleTime[0], spawnObstacleTime[1]));

            if (!isStopSpawn)
            {
                GameObject cloneProp1 = Instantiate(obstacleLeft, transform);
            }

            // if (!isStopSpawn)
            // {
            //     int leftOrRight = Random.Range(0, 2);
            //     GameObject obstacle;
            //     if (leftOrRight == 0)
            //     {
            //         obstacle = obstacleLeft;
            //     }
            //     else
            //     {
            //         obstacle = obstacleRight;
            //     }
            //     if (firstSpawn)
            //     {
            //         GameObject cloneObstacle = Instantiate(obstacle, transform);
            //         firstSpawn = false;
            //         lastPos = cloneObstacle;
            //     }
            //     else
            //     {
            //         GameObject cloneObstacle = Instantiate(obstacle, transform);
            //         Debug.Log(lastPos.transform.position.y + " and " + cloneObstacle.transform.position.y);
            //         if (cloneObstacle.transform.position.y - lastPos.transform.position.y <= 2.50f)
            //         {
            //             Debug.Log("DO NOT PLACE");
            //             Destroy(cloneObstacle);
            //         }
            //         else
            //         {
            //             Debug.Log("PLACE");
            //             lastPos = cloneObstacle;
            //         }
            //     }
            // }
        }

    }

    IEnumerator SpawnProp1()
    {
        while (true)
        {
            yield return new WaitUntil(() => !isStopSpawn);
            // Randomly spawn objects

            yield return new WaitForSeconds(Random.Range(spawnProp1Time[0], spawnProp1Time[1]));

            if (!isStopSpawn)
            {
                GameObject cloneProp1 = Instantiate(EnemyControlReverse, transform);
                cloneProp1.transform.position = new Vector2(Random.Range(leftOffset, rightOffset), 3.58f);
            }
        }

    }

    IEnumerator SpawnProp2()
    {

        while (true)
        {
            yield return new WaitUntil(() => !isStopSpawn);
            // Randomly spawn objects

            yield return new WaitForSeconds(Random.Range(spawnProp2Time[0], spawnProp2Time[1]));

            if (!isStopSpawn)
            {
                GameObject cloneProp2 = Instantiate(ScoreUp, transform);
                cloneProp2.transform.position = new Vector2(Random.Range(leftOffset, rightOffset), 3.58f);
            }
        }

    }


}
