using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedLimits
{
    public float defaultCarSpeed = 3.0f;

    public float defaultSpawnSpeed = 2.0f;

    public float carSpeedMultiplier = 1.15f;

    public float spawnSpeedMultiplier = 1.05f;

    public float carMaxSpeed = 10.0f;

    public float spawnMaxSpeed = 5.0f;

    public float speedIncreaseDuration = 30.0f;

    private float elapsedTime = 0f;

    private bool canIncreaseSpeed = true;

    private bool canIncreaseSpawnSpeed = true;

    public bool CanIncreaseSpawnSpeed
    {
        get { return canIncreaseSpawnSpeed; }
    }

    public bool CanIncreaseSpeed
    {
        get { return canIncreaseSpeed; }
    }

    public void UpdateCarElapsedTime(float deltaTime)
    {
        elapsedTime += deltaTime;
        if (elapsedTime > speedIncreaseDuration)
        {
            canIncreaseSpeed = false;
        }
    }

    public void UpdateSpawnElapsedTime(float deltaTime)
    {
        elapsedTime += deltaTime;
        if (elapsedTime > speedIncreaseDuration)
        {
            canIncreaseSpawnSpeed = false;
        }
    }

    public void SetLevelParameters(int level)
    {
        switch (level)
        {
            case 1:
                defaultCarSpeed = 3.0f;
                defaultSpawnSpeed = 2.0f;
                carSpeedMultiplier = 1.05f;
                spawnSpeedMultiplier = 1.03f;
                carMaxSpeed = 6.0f;
                spawnMaxSpeed = 5.0f;
                speedIncreaseDuration = 20.0f;
                break;
            case 2:
                defaultCarSpeed = 3.0f;
                defaultSpawnSpeed = 2.0f;
                carSpeedMultiplier = 1.08f;
                spawnSpeedMultiplier = 1.05f;
                carMaxSpeed = 7.0f;
                spawnMaxSpeed = 6.0f;
                speedIncreaseDuration = 30.0f;
                break;
            case 3:
                defaultCarSpeed = 3.0f;
                defaultSpawnSpeed = 2.0f;
                carSpeedMultiplier = 1.10f;
                spawnSpeedMultiplier = 1.08f;
                carMaxSpeed = 8.0f;
                spawnMaxSpeed = 7.0f;
                speedIncreaseDuration = 40.0f;
                break;
            case 4:
                defaultCarSpeed = 3.0f;
                defaultSpawnSpeed = 2.0f;
                carSpeedMultiplier = 1.12f;
                spawnSpeedMultiplier = 1.10f;
                carMaxSpeed = 9.0f;
                spawnMaxSpeed = 8.0f;
                speedIncreaseDuration = 50.0f;
                break;
            default:
                Debug.LogError("Unknown level: " + level);
                break;
        }
    }
}

public class SpeedController : MonoBehaviour
{
    private CarMove carLeftMove;

    private CarMove carRightMove;

    private SpeedLimits speedLimits = new SpeedLimits();

    private int level = 1;

    private float carFrequency = 0.2f;

    private float spawnFrequency = 0.5f;

    private Transform zoomLeft;

    private Transform zoomRight;

    private float carSpeed = 2.0f;

    private float spawnSpeed = 2.0f;


    // Start is called before the first frame update
    void Start()
    {
        level = GameLevelsManager.Instance.Level;
        Debug.Log("Level: " + level);
        speedLimits.SetLevelParameters(level);

        GameObject carLeft = GameObject.Find("CarLeft");
        GameObject carRight = GameObject.Find("CarRight");

        if (carLeft != null)
        {
            carLeftMove = carLeft.GetComponent<CarMove>();
            if (carLeftMove == null)
            {
                Debug.LogError("CarLeft does not have a CarMove component!");
            }
        }
        else
        {
            Debug.LogError("CarLeft object not found!");
        }

        if (carRight != null)
        {
            carRightMove = carRight.GetComponent<CarMove>();
            if (carRightMove == null)
            {
                Debug.LogError("CarRight does not have a CarMove component!");
            }
        }
        else
        {
            Debug.LogError("CarRight object not found!");
        }

        // // Initialize zoomLeft and zoomRight
        zoomLeft = GameObject.Find("ZoomLeft").transform;
        if (zoomLeft == null)
        {
            Debug.LogError("ZoomLeft object not found!");
        }

        zoomRight = GameObject.Find("ZoomRight").transform;
        if (zoomRight == null)
        {
            Debug.LogError("ZoomRight object not found!");
        }

        if (carLeftMove != null && carRightMove != null)
        {
            carLeftMove.carSpeed = speedLimits.defaultCarSpeed;
            carRightMove.carSpeed = speedLimits.defaultCarSpeed;

            StartCoroutine(UpdateCarSpeed());
        }

        if (zoomLeft != null && zoomRight != null)
        {
            spawnSpeed = speedLimits.defaultSpawnSpeed;
            StartCoroutine(UpdateSpawnSpeed());
        }
    }

    private IEnumerator UpdateCarSpeed()
    {
        while (speedLimits.CanIncreaseSpeed)
        {
            speedLimits.UpdateCarElapsedTime(carFrequency);

            if (Mathf.Abs(carLeftMove.carSpeed) < speedLimits.carMaxSpeed)
            {
                carLeftMove.carSpeed *= speedLimits.carSpeedMultiplier;
            }
            if (Mathf.Abs(carRightMove.carSpeed) < speedLimits.carMaxSpeed)
            {
                carRightMove.carSpeed *= speedLimits.carSpeedMultiplier;
            }
            // Debug.Log($"Updated speeds: Left: {carLeftMove.carSpeed}, Right: {carRightMove.carSpeed}");

            yield return new WaitForSeconds(carFrequency);
        }
    }

    private IEnumerator UpdateSpawnSpeed()
    {
        while (true)
        {
            if (speedLimits.CanIncreaseSpawnSpeed)
            {
                spawnSpeed *= speedLimits.spawnSpeedMultiplier;
                speedLimits.UpdateSpawnElapsedTime(spawnFrequency);
            }

            if (Mathf.Abs(spawnSpeed) > speedLimits.spawnMaxSpeed)
            {
                spawnSpeed = speedLimits.spawnMaxSpeed;
            }

            if (zoomLeft != null)
            {
                foreach (Transform t in zoomLeft)
                {
                    ObjMove objMove = t.GetComponent<ObjMove>();
                    if (objMove != null)
                    {
                        objMove.speed = spawnSpeed;
                    }

                    BulletManager bulletManager = t.GetComponent<BulletManager>();
                    if (bulletManager != null)
                    {
                        bulletManager.speed = spawnSpeed;
                    }

                    ObjMoveMiddle objMoveMiddle = t.GetComponent<ObjMoveMiddle>();
                    if (objMoveMiddle != null)
                    {
                        objMoveMiddle.speed = spawnSpeed;
                    }
                }
            }
            else
            {
                Debug.LogError("ZoomLeft is null");
            }

            if (zoomRight != null)
            {
                foreach (Transform t in zoomRight)
                {
                    ObjMove objMove = t.GetComponent<ObjMove>();
                    if (objMove != null)
                    {
                        objMove.speed = spawnSpeed;
                    }

                    BulletManager bulletManager = t.GetComponent<BulletManager>();
                    if (bulletManager != null)
                    {
                        bulletManager.speed = spawnSpeed;
                    }

                    ObjMoveMiddle objMoveMiddle = t.GetComponent<ObjMoveMiddle>();
                    if (objMoveMiddle != null)
                    {
                        objMoveMiddle.speed = spawnSpeed;
                    }
                }
            }
            else
            {
                Debug.LogError("ZoomRight is null");
            }


            yield return new WaitForSeconds(spawnFrequency);
        }
    }
}
