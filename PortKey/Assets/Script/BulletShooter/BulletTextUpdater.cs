using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BulletTextUpdater : MonoBehaviour
{
    public RotateBulletShooter rotateBulletShooterLeft;
    public RotateBulletShooter rotateBulletShooterRight;

    public TextMeshProUGUI currentBulletsTextLeft;
    public TextMeshProUGUI currentBulletsTextRight;

    private void Start()
    {
        rotateBulletShooterLeft = GameObject.Find("PivotLeft").GetComponent<RotateBulletShooter>();
        rotateBulletShooterRight = GameObject.Find("PivotRight").GetComponent<RotateBulletShooter>();
    }

    void Update()
    {
        UpdateBulletText();
    }

    void UpdateBulletText()
    {
        if (currentBulletsTextLeft != null)
        {
            currentBulletsTextLeft.text = ":" + rotateBulletShooterLeft.currentBulletsLeft.ToString();
        }
        if (currentBulletsTextRight != null)
        {
            currentBulletsTextRight.text = ":" + rotateBulletShooterRight.currentBulletsRight.ToString();
        }
    }
}

