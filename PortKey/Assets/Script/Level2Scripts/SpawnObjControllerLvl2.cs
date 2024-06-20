using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpawnObjControllerLvl2 : MonoBehaviour
{
    public GameObject obstacleLeft;

    public GameObject obstacleRight;

    public GameObject obstacleMiddle;

    public GameObject obstacleGapLeft;

    public GameObject obstacleGapRight;

    public GameObject EnemyControlReverse;

    public GameObject ScoreUp;

    public GameObject leftBulletPrefab;

    public GameObject rightBulletPrefab;

    public bool isStopSpawn = false;

    public float leftOffset = -200f;

    public float rightOffset = 200f;

    private readonly float[] spawnObstacleTime = { 1, 2};

    private readonly float[] spawnProp1Time = { 5, 8 };

    private readonly float[] spawnProp2Time = { 5, 8 };

    private bool firstSpawn = true;
    GameObject lastPos;
    Transform leftParent;
    Transform rightParent;


    // Start is called before the first frame update
    void Start()
    {
        leftParent = GameObject.Find("ZoomLeft").transform;
        rightParent = GameObject.Find("ZoomRight").transform;
        StartCoroutine(SpawnObstacle());
        StartCoroutine(SpawnProp1());
        StartCoroutine(SpawnProp2());
        InvokeRepeating("SpawnLeftBulletProps", 3f, 3f);
        InvokeRepeating("SpawnRightBulletProps", 3f, 3f);
    }

    // Update is called once per frame
    void Update()
    {
    }

    void SpawnRightBulletProps()
    {
        if (isStopSpawn == false && rightBulletPrefab != null)
        {
            Instantiate(rightBulletPrefab, RandomizeSpawnPosition(rightBulletPrefab), rightBulletPrefab.transform.rotation, rightParent);
        }
    }


    void SpawnLeftBulletProps()
    {
        if (isStopSpawn == false && leftBulletPrefab != null)
        {
            Instantiate(leftBulletPrefab, RandomizeSpawnPosition(leftBulletPrefab), leftBulletPrefab.transform.rotation ,leftParent);
        }
    }

    Vector3 RandomizeSpawnPosition(GameObject bulletPrefab)
    {
        Vector3 curr = bulletPrefab.transform.position;
        float newX = curr.x + Random.Range(0, 5);
        return new Vector3(newX, curr.y, curr.z);

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
                int leftOrRight = Random.Range(0, 4);
                GameObject obstacle;
                GameObject obstacle2;
                if (leftOrRight == 0)
                {
                    obstacle = obstacleLeft;
                    obstacle2 = null;
                }
                else if (leftOrRight == 1)
                {
                    obstacle = obstacleRight;
                    obstacle2 = null;
                } else if (leftOrRight == 2)
                {
                    obstacle = obstacleMiddle;
                    obstacle2 = null;
                } else
                {
                    obstacle = obstacleGapLeft;
                    obstacle2 = obstacleGapRight;
                }
                if (firstSpawn)
                {
                    if (obstacle2 == null)
                    {
                        GameObject cloneObstacle = Instantiate(obstacle, transform);
                        lastPos = cloneObstacle;
                    }
                    else
                    {
                        GameObject cloneObstacle = Instantiate(obstacle, transform);
                        GameObject cloneObstacle2 = Instantiate(obstacle2, transform);
                        lastPos = cloneObstacle2;
                    }
                    firstSpawn = false;
                }
                else
                {
                    if (obstacle2 == null)
                    {
                        GameObject cloneObstacle = Instantiate(obstacle, transform);
                        Debug.Log(lastPos.transform.position.y + " and " + cloneObstacle.transform.position.y);
                        if (cloneObstacle.transform.position.y - lastPos.transform.position.y <= 4.0f)
                        {
                            Debug.Log("DO NOT PLACE");
                            Destroy(cloneObstacle);
                        }
                        else
                        {
                            Debug.Log("PLACE");
                            lastPos = cloneObstacle;
                        }
                    }
                    else
                    {
                        GameObject cloneObstacle = Instantiate(obstacle, transform);
                        GameObject cloneObstacle2 = Instantiate(obstacle2, transform);
                        Debug.Log(lastPos.transform.position.y + " and " + cloneObstacle2.transform.position.y);
                        if (cloneObstacle2.transform.position.y - lastPos.transform.position.y <= 4.0f)
                        {
                            Debug.Log("DO NOT PLACE");
                            Destroy(cloneObstacle);
                            Destroy(cloneObstacle2);
                        }
                        else
                        {
                            Debug.Log("PLACE");
                            lastPos = cloneObstacle2;
                        }
                    }
                }
            }
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
