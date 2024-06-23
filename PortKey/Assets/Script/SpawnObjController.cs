using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpawnObjController : MonoBehaviour
{
    public GameObject obstacleLeft;

    public GameObject obstacleRight;

    public GameObject obstacleGapLeft;

    public GameObject obstacleGapRight;

    public GameObject enemyControlReverse;

    public GameObject scoreUp;

    public GameObject slowEnemy;

    public bool isStopSpawn = false;

    public float leftOffset = -200f;

    public float rightOffset = 200f;

    private readonly float[] spawnObstacleTime = { 1, 2 };

    private readonly float[] enemyControlReverseTime = { 5, 8 };

    private readonly float[] scoreUpTime = { 5, 8 };

    private readonly float[] slowEnemyTime = { 5, 8 };

    private bool firstSpawn = true;
    GameObject lastPos;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnObstacle());
        StartCoroutine(SpawnEnemyControlReverse());
        StartCoroutine(SpawnScoreUp());
        StartCoroutine(SpawnSlowEnemy());
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
                int leftOrRight = Random.Range(0, 3);
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
                }
                else
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

    IEnumerator SpawnEnemyControlReverse()
    {
        while (true)
        {
            yield return new WaitUntil(() => !isStopSpawn);
            // Randomly spawn objects

            yield return new WaitForSeconds(Random.Range(enemyControlReverseTime[0], enemyControlReverseTime[1]));

            if (!isStopSpawn)
            {
                GameObject cloneEnemyControlReverse = Instantiate(enemyControlReverse, transform);
                cloneEnemyControlReverse.transform.position = new Vector2(Random.Range(leftOffset, rightOffset), 4.0f);
            }
        }

    }

    IEnumerator SpawnScoreUp()
    {

        while (true)
        {
            yield return new WaitUntil(() => !isStopSpawn);
            // Randomly spawn objects

            yield return new WaitForSeconds(Random.Range(scoreUpTime[0], scoreUpTime[1]));

            if (!isStopSpawn)
            {
                GameObject cloneScoreUp = Instantiate(scoreUp, transform);
                cloneScoreUp.transform.position = new Vector2(Random.Range(leftOffset, rightOffset), 4.0f);
            }
        }

    }

    IEnumerator SpawnSlowEnemy()
    {
        while (true)
        {
            Debug.Log("Spawn Slow Enemy");
            yield return new WaitUntil(() => !isStopSpawn);
            // Randomly spawn objects

            yield return new WaitForSeconds(Random.Range(slowEnemyTime[0], slowEnemyTime[1]));

            if (!isStopSpawn)
            {
                GameObject cloneSlowEnemy = Instantiate(slowEnemy, transform);
                cloneSlowEnemy.transform.position = new Vector2(Random.Range(leftOffset, rightOffset), 4.0f);
            }
        }
    }
}
