using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HireCount : MonoBehaviour
{
    [SerializeField] private float scale;

    [SerializeField] private Color uncompleted;
    [SerializeField] private Color completed;


    [SerializeField] private float yy_start;
    [SerializeField] private float yy_end;
    [SerializeField] private float smooth;

    private float yy;

    [SerializeField] private float angle_spd;
    [SerializeField] private float angle_max;
    private float angle;

    [SerializeField] private TMPro.TMP_Text count_text;
    [SerializeField] private TMPro.TMP_Text limit_text;
    
    public int count = 0;
    public int limit = 0;

    // Start is called before the first frame update
    void Start()
    {
        transform.localScale = Vector3.one * scale;
        yy = yy_start;
        transform.localPosition = Vector3.up * yy;
    }

    // Update is called once per frame
    void Update()
    {
        yy += ((yy_end-yy) / (smooth/60)) * Time.deltaTime;

        transform.localPosition = Vector3.up * yy;

        angle = Mathf.Sin(Time.realtimeSinceStartup/(angle_spd/60))*angle_max;
        transform.localRotation = Quaternion.Euler(0,0,angle);

        if (count >= limit)
        {
            count_text.color = completed;
            limit_text.color = completed;
        }
        else
        {
            count_text.color = uncompleted;
            limit_text.color = uncompleted;
        }

        //updates text content
        count_text.text = count.ToString();
        limit_text.text = limit.ToString();
    }
}
