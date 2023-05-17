using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusPoints : MonoBehaviour
{
    public float points;
    [SerializeField] private float alpha_decrease;
    [SerializeField] private float y_increase;
    [SerializeField] private TMPro.TextMeshProUGUI text;

    // Start is called before the first frame update
    void Start()
    {
        RectTransform rt = GetComponent<RectTransform>();
        rt.pivot = transform.parent.GetComponent<RectTransform>().pivot;

        text = GetComponent<TMPro.TextMeshProUGUI>();
        text.text = "+" + points.ToString() + " FP";
    }

    // Update is called once per frame
    void Update()
    {
        Color c = text.color;
        c.a -= alpha_decrease * Time.deltaTime;
        text.color = c;
        
        Vector3 pos = transform.localPosition;
        pos.y += y_increase * Time.deltaTime;
        transform.localPosition = pos;
        
        if (c.a <= 0)
        {
            Destroy(gameObject);
        }
    }
}
