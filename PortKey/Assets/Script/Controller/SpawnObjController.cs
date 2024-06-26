using System.Collections;
using System.Collections.Generic;
using PortKey.Assets.Script.SwitchLevel;
using Unity.VisualScripting;
using UnityEngine;

public class SpawnObjInterval
{
    public readonly float[] spawnObstacleTime = { 1, 2 };

    public readonly float[] enemyControlReverseTime = { 5, 8 };

    public readonly float[] scoreUpTime = { 5, 8 };

    public readonly float[] reduceEnemyHealthTime = { 5, 8 };

    public readonly float[] slowEnemyTime = { 5, 8 };

    public void SetLevelParameters(int level)
    {
        switch (level)
        {
            case 1:
                spawnObstacleTime[0] = 1;
                spawnObstacleTime[1] = 2;
                enemyControlReverseTime[0] = 5;
                enemyControlReverseTime[1] = 8;
                scoreUpTime[0] = 5;
                scoreUpTime[1] = 8;
                reduceEnemyHealthTime[0] = 5;
                reduceEnemyHealthTime[1] = 8;
                slowEnemyTime[0] = 5;
                slowEnemyTime[1] = 8;
                break;
            case 2:
                spawnObstacleTime[0] = 0.8f;
                spawnObstacleTime[1] = 1.8f;
                enemyControlReverseTime[0] = 4.8f;
                enemyControlReverseTime[1] = 7.8f;
                scoreUpTime[0] = 4.8f;
                scoreUpTime[1] = 7.8f;
                reduceEnemyHealthTime[0] = 4.8f;
                reduceEnemyHealthTime[1] = 7.8f;
                slowEnemyTime[0] = 4.8f;
                slowEnemyTime[1] = 7.8f;
                break;
            case 3:
                spawnObstacleTime[0] = 0.6f;
                spawnObstacleTime[1] = 1.6f;
                enemyControlReverseTime[0] = 4.6f;
                enemyControlReverseTime[1] = 7.6f;
                scoreUpTime[0] = 4.6f;
                scoreUpTime[1] = 7.6f;
                reduceEnemyHealthTime[0] = 4.6f;
                reduceEnemyHealthTime[1] = 7.6f;
                slowEnemyTime[0] = 4.6f;
                slowEnemyTime[1] = 7.6f;
                break;
            case 4:
                spawnObstacleTime[0] = 0.5f;
                spawnObstacleTime[1] = 1.5f;
                enemyControlReverseTime[0] = 3;
                enemyControlReverseTime[1] = 6;
                scoreUpTime[0] = 3;
                scoreUpTime[1] = 6;
                reduceEnemyHealthTime[0] = 3;
                reduceEnemyHealthTime[1] = 6;
                slowEnemyTime[0] = 3;
                slowEnemyTime[1] = 6;
                break;
            default:
                Debug.Log("Invalid level");
                spawnObstacleTime[0] = 1;
                spawnObstacleTime[1] = 2;
                enemyControlReverseTime[0] = 5;
                enemyControlReverseTime[1] = 8;
                scoreUpTime[0] = 5;
                scoreUpTime[1] = 8;
                reduceEnemyHealthTime[0] = 5;
                reduceEnemyHealthTime[1] = 8;
                slowEnemyTime[0] = 5;
                slowEnemyTime[1] = 8;
                break;
        }
    }
}

public class SpawnObjController : MonoBehaviour
{
    private SpawnObjInterval interval = new SpawnObjInterval();

    public GameObject obstacleLeft;

    public GameObject obstacleRight;

    public GameObject obstacleMiddle;

    public GameObject obstacleGapLeft;

    public GameObject obstacleGapRight;

    public GameObject enemyControlReverse;

    public GameObject scoreUp;

    public GameObject slowEnemy;

    public GameObject reduceEnemyHealth;

    public bool isStopSpawn = false;

    public float leftOffset = -200f;

    public float rightOffset = 200f;

    public SpeedLimits speedLimits;

    private bool firstSpawn = true;

    GameObject lastPos;

    private int level;

    private float showUpYPos = 7.0f;

    // Start is called before the first frame update
    void Start()
    {
        // Set level parameters
        level = LevelInfo.Instance.Level;
        interval.SetLevelParameters(level);

        // for level 1, only spawn immovable obstacles
        if (obstacleMiddle == null)
        {
            StartCoroutine(SpawnImmovableObstacle());
        }
        else // for further levels, spawn movable obstacles
        {
            StartCoroutine(SpawnMovableObstacle());
        }


        if (enemyControlReverse != null)
        {
            StartCoroutine(SpawnEnemyControlReverse());
        }

        if (scoreUp != null)
        {
            StartCoroutine(SpawnScoreUp());
        }

        if (reduceEnemyHealth != null)
        {
            StartCoroutine(SpawnReduceEnemyHealth());
        }


        if (slowEnemy != null)
        {
            StartCoroutine(SpawnSlowEnemy());
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    IEnumerator SpawnImmovableObstacle()
    {
        while (true)
        {
            // Wait for a while before spawning the next obstacle
            yield return new WaitUntil(() => !isStopSpawn);

            // Randomly spawn objects
            yield return new WaitForSeconds(Random.Range(interval.spawnObstacleTime[0], interval.spawnObstacleTime[1]));

            if (!isStopSpawn)
            {
                int leftOrRight = Random.Range(0, 3);
                GameObject obstacle = null;
                GameObject obstacle2 = null;

                switch (leftOrRight)
                {
                    case 0:
                        obstacle = obstacleLeft;
                        break;
                    case 1:
                        obstacle = obstacleRight;
                        break;
                    case 2:
                        obstacle = obstacleGapLeft;
                        obstacle2 = obstacleGapRight;
                        break;
                }

                GameObject cloneObstacle = Instantiate(obstacle, transform);
                GameObject cloneObstacle2 = (obstacle2 != null) ? Instantiate(obstacle2, transform) : null;

                if (firstSpawn)
                {
                    lastPos = (cloneObstacle2 != null) ? cloneObstacle2 : cloneObstacle; // Use the second obstacle if available
                    firstSpawn = false;
                }
                else
                {
                    if (lastPos != null && (cloneObstacle2 != null || cloneObstacle != null))
                    {
                        GameObject relevantObstacle = (cloneObstacle2 != null) ? cloneObstacle2 : cloneObstacle;
                        if (Mathf.Abs(relevantObstacle.transform.position.y - lastPos.transform.position.y) <= 4.0f)
                        {
                            Debug.Log("DO NOT PLACE");
                            Destroy(relevantObstacle);
                            if (cloneObstacle2 != null) Destroy(cloneObstacle2);  // Make sure to clean up both if there are two
                        }
                        else
                        {
                            Debug.Log("PLACE");
                            lastPos = relevantObstacle; // Update lastPos to the last successfully placed obstacle
                        }
                    }
                }
            }
        }
    }

    IEnumerator SpawnMovableObstacle()
    {
        while (true)
        {
            yield return new WaitUntil(() => !isStopSpawn);

            // Randomly spawn objects
            yield return new WaitForSeconds(Random.Range(interval.spawnObstacleTime[0], interval.spawnObstacleTime[1]));

            if (!isStopSpawn)
            {
                int leftOrRight = Random.Range(0, 4);
                GameObject obstacle = null;
                GameObject obstacle2 = null;

                switch (leftOrRight)
                {
                    case 0:
                        obstacle = obstacleLeft;
                        break;
                    case 1:
                        obstacle = obstacleRight;
                        break;
                    case 2:
                        obstacle = obstacleMiddle;
                        break;
                    case 3:
                        obstacle = obstacleGapLeft;
                        obstacle2 = obstacleGapRight;
                        break;
                }

                GameObject cloneObstacle = Instantiate(obstacle, transform);
                GameObject cloneObstacle2 = (obstacle2 != null) ? Instantiate(obstacle2, transform) : null;

                if (firstSpawn)
                {
                    lastPos = cloneObstacle2 ?? cloneObstacle;
                    firstSpawn = false;
                }
                else
                {
                    if (lastPos != null && (cloneObstacle2 != null || cloneObstacle != null))
                    {
                        GameObject relevantObstacle = cloneObstacle2 ?? cloneObstacle;
                        if (Mathf.Abs(relevantObstacle.transform.position.y - lastPos.transform.position.y) <= 4.0f)
                        {
                            Debug.Log("DO NOT PLACE");
                            Destroy(relevantObstacle);
                            if (cloneObstacle2 != null) Destroy(cloneObstacle2);
                        }
                        else
                        {
                            Debug.Log("PLACE");
                            lastPos = relevantObstacle;
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

            yield return new WaitForSeconds(Random.Range(interval.enemyControlReverseTime[0], interval.enemyControlReverseTime[1]));

            if (!isStopSpawn)
            {
                GameObject cloneEnemyControlReverse = Instantiate(enemyControlReverse, transform);
                cloneEnemyControlReverse.transform.position = new Vector2(Random.Range(leftOffset, rightOffset), showUpYPos);
            }
        }

    }

    IEnumerator SpawnScoreUp()
    {

        while (true)
        {
            yield return new WaitUntil(() => !isStopSpawn);
            // Randomly spawn objects

            yield return new WaitForSeconds(Random.Range(interval.scoreUpTime[0], interval.scoreUpTime[1]));

            if (!isStopSpawn)
            {
                GameObject cloneScoreUp = Instantiate(scoreUp, transform);
                cloneScoreUp.transform.position = new Vector2(Random.Range(leftOffset, rightOffset), showUpYPos);
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

            yield return new WaitForSeconds(Random.Range(interval.slowEnemyTime[0], interval.slowEnemyTime[1]));

            if (!isStopSpawn)
            {
                GameObject cloneSlowEnemy = Instantiate(slowEnemy, transform);
                cloneSlowEnemy.transform.position = new Vector2(Random.Range(leftOffset, rightOffset), showUpYPos);
            }
        }
    }

    IEnumerator SpawnReduceEnemyHealth()
    {
        while (true)
        {
            yield return new WaitUntil(() => !isStopSpawn);
            // Randomly spawn objects

            yield return new WaitForSeconds(Random.Range(interval.reduceEnemyHealthTime[0], interval.reduceEnemyHealthTime[1]));
            if (!isStopSpawn)
            {
                GameObject cloneReduceEnemyScore = Instantiate(reduceEnemyHealth, transform);
                cloneReduceEnemyScore.transform.position = new Vector2(Random.Range(leftOffset, rightOffset), showUpYPos);
            }
        }

    }

}
