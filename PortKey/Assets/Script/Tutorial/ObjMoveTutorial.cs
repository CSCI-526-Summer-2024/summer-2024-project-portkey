using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjMoveTutorial : MonoBehaviour
{
    public float moveSpeed = 2f;

    public float speed = 2f;

    GameControllerTutorial gameController;

    // Start is called before the first frame update
    void Start()
    {
        gameController = FindObjectOfType<GameControllerTutorial>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameController.canMove)
        {
            transform.Translate(Vector3.down * speed * Time.deltaTime);
        }
        if (transform.position.y <= -3f && transform.gameObject.tag != "Obstacle")
        {
            gameController.canMove = false;
        }
    }

    void OnDestroy()
    {
        gameController.canMove = true;
        Debug.Log("here");
    }
}
