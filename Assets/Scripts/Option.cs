using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

//[Serializable]
public class OptionValues
{
    public string text;

    public OptionValues(string option)
    {
        text = option;
    }
}

public class PersonalQuestionOption : OptionValues
{
    public string text;
    public int question_index;

    public PersonalQuestionOption(string option, int index) : base(option)
    {
        question_index = index;
    }
}

public class TestCompetenceOption : OptionValues
{
    public string text;
    public int competence_index;
    public TestCompetence test;

    public TestCompetenceOption(string option, int index, TestCompetence this_test) : base(option)
    {
        competence_index = index;
        test = this_test;
    }
}

public class Option : MonoBehaviour
{   
    [SerializeField] private GameObject question_template;

    //Enter and exit stage
    private float yy;
    [SerializeField] private AnimCurveValue yy_enter_curve;
    [SerializeField] private AnimCurveValue yy_exit_curve;
    private int stage = 0;//0 - intro     1 - idle     2 - exiting

    //Options list
    public OptionValues[] options;
    private OptionImage[] option_objs; //objects
    public int choosen_option = -1;
    public OptionValues choosen_option_struc;

    public bool destroy = false;

    void Start()
    {
        option_objs = new OptionImage[options.Length];

        for (int i = 0; i < options.Length; i++)
        {
            var opt = Instantiate(question_template,new Vector3(0,0,0), Quaternion.identity, transform);
            option_objs[i] = opt.GetComponent<OptionImage>();

            option_objs[i].text = options[i].text;
            option_objs[i].my_canvas = transform.parent.GetComponent<Canvas>();
        }
        Destroy(question_template);

        Canvas canvas = FindObjectOfType<Canvas>();

        yy_enter_curve.val_start = -canvas.GetComponent<RectTransform>().rect.height;                                      
        yy_enter_curve.val_end = transform.localPosition.y;
        
        yy_exit_curve.val_start = yy_enter_curve.val_end;
        yy_exit_curve.val_end = yy_enter_curve.val_start;

        transform.localPosition = new Vector3(transform.localPosition.x,yy_enter_curve.val_start,transform.localPosition.z);

    }

    void Update()
    {
        switch (stage)
        {
            case 0:
            {
                Enter();
                
                for (int i = 0; i < option_objs.Length; i++)
                {
                    option_objs[i].control = false;
                }

                if (yy_enter_curve.GetRawValue() == 1) stage++;
            }
            break;
            case 1:
            {
                for (int i = 0; i < option_objs.Length; i++)
                {
                    option_objs[i].control = true;

                    if (option_objs[i].selected)
                    {
                        choosen_option = i;
                        stage++;
                        choosen_option_struc = options[choosen_option];
                        break;
                    }
                }
            }
            break;
            case 2:
            {
                Exit();

                for (int i = 0; i < option_objs.Length; i++)
                {
                    option_objs[i].control = false;
                }

                if ((yy_exit_curve.GetRawValue() == 1) && (destroy))
                {
                    Destroy(gameObject);
                }
            }
            break;
        }
        

        transform.localPosition = new Vector3(transform.localPosition.x,yy,transform.localPosition.z);
    }

    void Enter()
    {
        yy = yy_enter_curve.Update(Time.deltaTime);
    }

    void Exit()
    {
        yy = yy_exit_curve.Update(Time.deltaTime);
    }
}
