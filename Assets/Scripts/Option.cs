using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Option : MonoBehaviour
{
    [Serializable]
    public struct OptionStruc
    {
        public string text;
        public DialogueStruc[] dialogue_output;
    }
    
    [SerializeField] private GameObject question_template;

    //Enter and exit animation
    private float yy;
    [SerializeField] private AnimCurveValue yy_enter_curve;
    [SerializeField] private AnimCurveValue yy_exit_curve;
    private int animation = 0;//0 - intro     1 - idle     2 - exiting

    //Options list
    public OptionStruc[] options;
    private OptionImage[] option_objs; //objects
    private int choosen_option = -1;

    void Start()
    {
        option_objs = new OptionImage[options.Length];

        for (int i = 0; i < options.Length; i++)
        {
            var opt = Instantiate(question_template,new Vector3(0,0,0), Quaternion.identity, transform);
            option_objs[i] = opt.GetComponent<OptionImage>();

            option_objs[i].text = options[i].text;
        }
        Destroy(question_template);

        Canvas canvas = FindObjectOfType<Canvas>();

        yy_enter_curve.val_start = -canvas.GetComponent<RectTransform>().rect.height;                                      
        yy_enter_curve.val_end = transform.localPosition.y;
        
        yy_exit_curve.val_start = yy_enter_curve.val_end;
        yy_exit_curve.val_end = yy_enter_curve.val_start;
    }

    void Update()
    {
        switch (animation)
        {
            case 0:
            {
                Enter();
                
                for (int i = 0; i < option_objs.Length; i++)
                {
                    option_objs[i].control = false;
                }

                if (yy_enter_curve.GetRawValue() == 1) animation++;
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
                        animation++;
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

                if (yy_enter_curve.GetRawValue() == 1)
                {

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
