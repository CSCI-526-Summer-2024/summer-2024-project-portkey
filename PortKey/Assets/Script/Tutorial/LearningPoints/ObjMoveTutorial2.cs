using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjMoveTutorial2 : MonoBehaviour
{
    public float moveSpeed = 2f;

    public float speed = 2f;

    GameControllerTutorial2 gameController;
    Vector3 originalScale;
    bool isBlinking = false;

    // Start is called before the first frame update
    void Start()
    {
        gameController = FindObjectOfType<GameControllerTutorial2>();
        originalScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameController.canMove)
        {
            transform.Translate(Vector3.down * speed * Time.deltaTime);
            isBlinking = false;
        }
        if (transform.position.y <= -3f && transform.gameObject.tag != "Obstacle")
        {
            gameController.canMove = false;
            if (!isBlinking)
            {
                isBlinking = true;
                StartCoroutine(BlinkObject());
            }
        }
    }

    void OnDestroy()
    {
        gameController.canMove = true;

        // Stop blinking stuff
        StopCoroutine(BlinkObject());
        transform.localScale = originalScale;
        isBlinking = false;
    }

    IEnumerator BlinkObject()
    {
        Debug.Log("uhh hello?");
        while (isBlinking)
        {
            transform.localScale = Vector3.zero;
            yield return new WaitForSeconds(0.5f);

            transform.localScale = originalScale;
            yield return new WaitForSeconds(0.5f);
        }
    }
}
