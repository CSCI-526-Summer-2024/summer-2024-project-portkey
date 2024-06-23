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
                canIncreaseSpeed = true;
                canIncreaseSpawnSpeed = true;
                defaultCarSpeed = 3.0f;
                defaultSpawnSpeed = 2.0f;
                carSpeedMultiplier = 1.05f;
                spawnSpeedMultiplier = 1.01f;
                carMaxSpeed = 6.0f;
                spawnMaxSpeed = 5.0f;
                speedIncreaseDuration = 20.0f;
                break;
            case 2:
                canIncreaseSpeed = true;
                canIncreaseSpawnSpeed = true;
                defaultCarSpeed = 3.0f;
                defaultSpawnSpeed = 2.0f;
                carSpeedMultiplier = 1.08f;
                spawnSpeedMultiplier = 1.02f;
                carMaxSpeed = 7.0f;
                spawnMaxSpeed = 6.0f;
                speedIncreaseDuration = 30.0f;
                break;
            case 3:
                canIncreaseSpeed = true;
                canIncreaseSpawnSpeed = true;
                defaultCarSpeed = 3.0f;
                defaultSpawnSpeed = 2.0f;
                carSpeedMultiplier = 1.10f;
                spawnSpeedMultiplier = 1.03f;
                carMaxSpeed = 8.0f;
                spawnMaxSpeed = 7.0f;
                speedIncreaseDuration = 40.0f;
                break;
            case 4:
                canIncreaseSpeed = true;
                canIncreaseSpawnSpeed = true;
                defaultCarSpeed = 3.0f;
                defaultSpawnSpeed = 2.0f;
                carSpeedMultiplier = 1.12f;
                spawnSpeedMultiplier = 1.04f;
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

    private float spawnFrequency = 0.1f;

    private Transform zoomLeft;

    private Transform zoomRight;

    private float carSpeed = 2.0f;

    public float spawnSpeed = 2.0f;

    private ObjMove objMove;

    private ObjMoveMiddle objMoveMiddle;

    private BulletManager bulletManager;

    private bool leftCarSlowDown = false;

    private bool rightCarSlowDown = false;

    private float slowDownFactor = 0.5f;


    // Start is called before the first frame update
    void Start()
    {
        if (GameLevelsManager.Instance == null)
        {
            Debug.LogError("GameLevelsManager is null");
            level = 1;
        }
        else
        {
            level = GameLevelsManager.Instance.Level;
        }
        Debug.Log("Level: " + level);
        if (level < 1 || level > 4)
        {
            Debug.LogError("Invalid level: " + level);
            level = 1;
        }
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

        if (carLeftMove != null && carRightMove != null && speedLimits.CanIncreaseSpeed)
        {
            carLeftMove.carSpeed = speedLimits.defaultCarSpeed;
            carRightMove.carSpeed = speedLimits.defaultCarSpeed;

            StartCoroutine(UpdateCarSpeed());
        }

        if (zoomLeft != null && zoomRight != null && speedLimits.CanIncreaseSpawnSpeed)
        {
            spawnSpeed = speedLimits.defaultSpawnSpeed;
            StartCoroutine(UpdateSpawnSpeed());
        }

        objMove = GameObject.Find("ObjMove").GetComponent<ObjMove>();
        if (objMove == null)
        {
            Debug.LogError("ObjMove is null");
        }

        objMoveMiddle = GameObject.Find("ObjMoveMiddle").GetComponent<ObjMoveMiddle>();
        if (objMoveMiddle == null)
        {
            Debug.LogError("ObjMoveMiddle is null");
        }

        bulletManager = GameObject.Find("BulletManager").GetComponent<BulletManager>();
        if (bulletManager == null)
        {
            Debug.LogError("BulletManager is null");
        }
    }

    public void SlowDownCarTemporarily(string carName, float Factor, float duration)
    {
        slowDownFactor = Factor;
        if (carName == "CarLeft" && carRightMove != null)
        {
            rightCarSlowDown = true;
            StartCoroutine(SlowDownCoroutine(carRightMove, duration));
        }
        else if (carName == "CarRight" && carLeftMove != null)
        {
            leftCarSlowDown = true;
            StartCoroutine(SlowDownCoroutine(carLeftMove, duration));
        }
    }

    private IEnumerator SlowDownCoroutine(CarMove carMove, float duration)
    {
        yield return new WaitForSeconds(duration);

        if (carMove == carLeftMove)
        {
            leftCarSlowDown = false;
        }
        else
        {
            rightCarSlowDown = false;
        }

        Debug.Log("rightCarSlowDown: " + rightCarSlowDown + ", leftCarSlowDown: " + leftCarSlowDown);
    }

    private IEnumerator UpdateCarSpeed()
    {
        while (speedLimits.CanIncreaseSpeed)
        {
            speedLimits.UpdateCarElapsedTime(carFrequency);

            float[] leftCarSpeeds = SetCarSpeed(carSpeed, carLeftMove.carSpeed, speedLimits.carMaxSpeed, speedLimits.carSpeedMultiplier, leftCarSlowDown, 0.5f);
            carLeftMove.carSpeed = leftCarSpeeds[1];

            float[] rightCarSpeeds = SetCarSpeed(carSpeed, carRightMove.carSpeed, speedLimits.carMaxSpeed, speedLimits.carSpeedMultiplier, rightCarSlowDown, 0.5f);
            carRightMove.carSpeed = rightCarSpeeds[1];

            carSpeed = leftCarSpeeds[0];

            Debug.Log($"Updated speeds: Left: {carLeftMove.carSpeed}, Right: {carRightMove.carSpeed}");

            yield return new WaitForSeconds(carFrequency);
        }
    }

    private float[] SetCarSpeed(float carSpeed, float carSpeedInGame, float maxCarSpeed, float carSpeedMultiplier, bool slowDown, float slowDownFactor)
    {
        {
            int direction = carSpeedInGame > 0 ? 1 : -1;
            carSpeed *= carSpeedMultiplier;
            if (Mathf.Abs(carSpeed) > maxCarSpeed)
            {
                carSpeed = maxCarSpeed;
            }

            if (!slowDown)
            {
                carSpeedInGame = carSpeed * direction;
            }
            else if (slowDownFactor > 0)
            {
                carSpeedInGame = carSpeed * direction * slowDownFactor;
            }

            return new float[] { carSpeed, carSpeedInGame };
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

            // Update the speed of the main objects
            if (objMove != null)
            {
                objMove.speed = spawnSpeed;
            }

            if (objMoveMiddle != null)
            {
                objMoveMiddle.speed = spawnSpeed;
            }

            if (bulletManager != null)
            {
                bulletManager.speed = spawnSpeed;
            }


            yield return new WaitForSeconds(spawnFrequency);
        }
    }
}
