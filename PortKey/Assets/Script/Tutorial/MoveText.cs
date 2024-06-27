using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MoveText : MonoBehaviour
{
    private RectTransform rectTransform;
    private TextMeshProUGUI txt;
    private float time;
    private Color initCol = Color.red;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        txt = GetComponent<TextMeshProUGUI>();
        if (txt != null)
        {
            txt.color = initCol;
        }
    }

    void Update()
    {
        rectTransform.Translate(new Vector3(0, -215.0f * Time.deltaTime, 0));
        time += Time.deltaTime * 2.0f;
        if (txt != null)
        {
            float alpha = Mathf.Abs(Mathf.Sin(time));
            txt.color = new Color(initCol.r, initCol.g, initCol.b, alpha);
        }
    }
}
