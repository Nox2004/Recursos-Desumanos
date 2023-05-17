using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DayCount : MonoBehaviour
{
    [SerializeField] private GameObject previous_day_obj;
    [SerializeField] private float days_distance;
    [SerializeField] private int days_to_show;
    [SerializeField] private AnimationCurve curve;
    [SerializeField] private float animation_duration;
    
    private AnimCurveValue offset_curve;
    [SerializeField] private Image background;
    [SerializeField] private TMPro.TextMeshProUGUI day_text;
    
    private GameObject[] day_objs;

    public int day;

    [Header("Day range")]
    [Space(10)]
    [SerializeField] private int min_day;
    [SerializeField] private int max_day;
    
    private float day_yy;
    private float alpha = 1;
    [SerializeField] private float alpha_decrease;


    //private GameObject[] days;

    // Start is called before the first frame update
    void Start()
    {
        Singleton.Instance.game_paused = true;

        day--; //Makes it the previous day

        //Sets curve up
        offset_curve = new AnimCurveValue();
        offset_curve.animation_duration = animation_duration;
        offset_curve.curve = curve;
        offset_curve.val_start = 0;
        offset_curve.val_end = days_distance;

        day_yy = previous_day_obj.transform.localPosition.y;
        day_objs = new GameObject[days_to_show];

        //sets everything position
        int days_middle = Mathf.FloorToInt((days_to_show-1)/2);

        for (int i = 0; i < days_to_show; i++)
        {
            if (i == days_middle)
            {
                day_objs[i] = previous_day_obj;
                day_objs[i].GetComponent<TMPro.TextMeshProUGUI>().text = (day >= min_day && day <= max_day) ? day.ToString() : "";
                continue;
            }

            int offset_from_middle = (i - days_middle);

            day_objs[i] = Instantiate(previous_day_obj, previous_day_obj.transform.parent);
            
            Vector3 tmp = previous_day_obj.transform.localPosition;
            tmp.y = day_yy + offset_from_middle * days_distance;
            day_objs[i].transform.localPosition = tmp;
            int final_day = day + offset_from_middle;
            day_objs[i].GetComponent<TMPro.TextMeshProUGUI>().text = (final_day >= min_day && final_day <= max_day) ? final_day.ToString() : "";
        }
    }

    // Update is called once per frame
    void Update()
    {
        float yy = offset_curve.Update(Time.deltaTime);
        float off = offset_curve.GetValue();

        //sets everything position
        int days_middle = Mathf.FloorToInt((days_to_show-1)/2);

        for (int i = 0; i < days_to_show; i++)
        {
            int offset_from_middle = (i - days_middle);
            
            Vector3 tmp = previous_day_obj.transform.localPosition;
            tmp.y = day_yy + offset_from_middle * days_distance - off;
            day_objs[i].transform.localPosition = tmp;

            //Gets distance from middle
            float y_distance_from_middle = Mathf.Abs(tmp.y - day_yy);
            
            float value = 1 - (y_distance_from_middle / (days_distance * 1.5f));
            value = Mathf.Clamp(value, 0, 1);

            //Sets size and alpha to be smaller the further away from the middle
            day_objs[i].transform.localScale = Vector3.one * Mathf.Lerp(0.5f, 1, value);
            
            Color tmp_color = day_objs[i].GetComponent<TMPro.TextMeshProUGUI>().color;
            tmp_color.a = value; day_objs[i].GetComponent<TMPro.TextMeshProUGUI>().color = tmp_color;
        }        

        if (offset_curve.GetRawValue() >= 1)
        {
            alpha -= (alpha_decrease*60) * Time.deltaTime;

            Color tmp; 
            tmp = background.color;
            tmp.a = alpha;
            background.color = tmp;

            tmp = day_text.color;
            tmp.a = alpha;
            day_text.color = tmp;

            //Sets alpha of days
            for (int i = 0; i < days_to_show; i++)
            {
                Color tmp_color = day_objs[i].GetComponent<TMPro.TextMeshProUGUI>().color;
                tmp_color.a *= alpha; day_objs[i].GetComponent<TMPro.TextMeshProUGUI>().color = tmp_color;
            }

            if (alpha <= 0)
            {
                Destroy(gameObject);
            }
        }
    }

    void OnDestroy()
    {
        Singleton.Instance.game_paused = false;
    }
}
