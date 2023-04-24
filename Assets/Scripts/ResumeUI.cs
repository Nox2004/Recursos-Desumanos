using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System;
//using TMPro.TextMeshPro;

public class ResumeUI : MonoBehaviour
{
    private int stage = 0;
    [SerializeField] private float back_alpha;
    private float yy;
    //Y movement box curves
    [SerializeField] private float start_yy;
    [SerializeField] private AnimCurveValue yy_enter_curve;
    [SerializeField] private AnimCurveValue yy_exit_curve;

    //Game Objects
    [SerializeField] private GameObject name_obj;
    [SerializeField] private GameObject age_obj;
    [SerializeField] private GameObject id_obj;
    [SerializeField] private GameObject competence_template;
    [SerializeField] private GameObject competence_layout;

    public string name;
    public int age;
    public string id;
    public string[] competence_descs;

    //text components
    private TMPro.TextMeshProUGUI name_text;
    private TMPro.TextMeshProUGUI age_text;
    private TMPro.TextMeshProUGUI id_text;

    private Image black_background; 

    private void Start()
    {
        Debug.Log(back_alpha);
        black_background = GameObject.FindGameObjectsWithTag("BlackCanvas")[0].GetComponent<Image>();

        name_text = name_obj.GetComponent<TMPro.TextMeshProUGUI>();
        age_text = age_obj.GetComponent<TMPro.TextMeshProUGUI>();
        id_text = id_obj.GetComponent<TMPro.TextMeshProUGUI>();

        foreach (string comp in competence_descs)
        {
            var _obj = Instantiate(competence_template,Vector3.zero, Quaternion.identity, competence_layout.transform);
            _obj.GetComponent<TMPro.TextMeshProUGUI>().text = "-" + comp;
        }
        Destroy(competence_template);

        name_text.text = name;
        age_text.text = age + " anos";
        id_text.text = id;

        //Sets up procedural animation stuff
        yy = start_yy;
        yy_enter_curve.val_start = start_yy;                   yy_exit_curve.val_end = yy_enter_curve.val_start;
        yy_enter_curve.val_end = transform.localPosition.y;    yy_exit_curve.val_start = yy_enter_curve.val_end;
        
        transform.localPosition = new Vector3(transform.localPosition.x,yy,transform.localPosition.z);
    }

    private void Update()
    {
        switch (stage)
        {
            case 0: {
                enter();
                if (yy_enter_curve.GetRawValue()>=1) stage++;
                black_background.color = new Color(0,0,0,yy_enter_curve.GetRawValue()*back_alpha);
            }
            break;
            case 1: {
                if (InGameCursor.get_button_down(0)) stage++;
            }
            break;
            case 2: {
                hide();
                
                black_background.color = new Color(0,0,0,(1-yy_exit_curve.GetRawValue())*back_alpha);
            }
            break;
        }

        transform.localPosition = new Vector3(transform.localPosition.x,yy,transform.localPosition.z);
    }

    public bool finished_intro()
    {
        if (yy_enter_curve.GetRawValue()>=1) return true;
        return false;
    }

    public bool finished_outro()
    {
        if (yy_exit_curve.GetRawValue()>=1) return true;
        return false;
    }

    private void enter()
    {
        yy = yy_enter_curve.Update(Time.deltaTime);
    }
    
    private void hide()
    {
        yy = yy_exit_curve.Update(Time.deltaTime);
    }
}
