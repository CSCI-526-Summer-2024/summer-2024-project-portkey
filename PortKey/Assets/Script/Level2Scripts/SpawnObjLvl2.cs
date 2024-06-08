using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpawnObjLvl2 : MonoBehaviour
{
    public GameObject obstacleLeft;

    public GameObject obstacleRight;

    public GameObject prop1;

    public GameObject prop2;

    public bool isStopSpawn = false;

    public float leftOffset = -200f;

    public float rightOffset = 200f;

    private readonly float[] spawnObstacleTime = { 1, 4 };

    private readonly float[] spawnProp1Time = { 3, 6 };

    private readonly float[] spawnProp2Time = { 3, 6 };

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
                int leftOrRight = Random.Range(0, 2);
                if (leftOrRight == 0)
                {
                    GameObject cloneObstacleLeft = Instantiate(obstacleLeft, transform);
                }
                else
                {
                    GameObject cloneObstacleRight = Instantiate(obstacleRight, transform);
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
                GameObject cloneProp1 = Instantiate(prop1, transform);
                cloneProp1.transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(Random.Range(leftOffset, rightOffset), 565);
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
                GameObject cloneProp2 = Instantiate(prop2, transform);
                cloneProp2.transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(Random.Range(leftOffset, rightOffset), 565);
            }
        }

    }


}
