using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuturePointCounter : MonoBehaviour
{
    [SerializeField] private float x_offset;
    [SerializeField] private float y_offset;

    private TMPro.TextMeshProUGUI text;
    private float points = 0;

    // Start is called before the first frame update
    void Start()
    {
        //set my rectransform pivot at 1, 1
        RectTransform rt = GetComponent<RectTransform>();
        rt.pivot = new Vector2(0.5f, 0);
        
        RectTransform canvas_rt = transform.parent.GetComponent<Canvas>().GetComponent<RectTransform>();
        //transform.localPosition = new Vector3(canvas_rt.rect.width/2 - x_offset, canvas_rt.rect.height/2 - y_offset, 0);
        transform.localPosition = new Vector3(x_offset, - canvas_rt.rect.height/2 + y_offset, 0);

        text = GetComponent<TMPro.TextMeshProUGUI>();
        points = Singleton.Instance.future_points;
    }

    // Update is called once per frame
    void Update()
    {
        points += ((Singleton.Instance.future_points - points) / 60) * Time.deltaTime;
        
        int rounded_points = (int) Mathf.Floor(points);
        text.text = rounded_points.ToString() + " FP";
    }
}
