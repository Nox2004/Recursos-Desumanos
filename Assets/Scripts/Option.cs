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
        public DialogueStruc dialogue_output;
    }
    
    [SerializeField] private GameObject question_template;

    private float yy;
    //Image scale
    [SerializeField] private AnimCurveValue yy_enter_curve;
    [SerializeField] private AnimCurveValue yy_exit_curve;

    public OptionStruc[] options;
    private OptionImage[] option_objs;

    void Start()
    {
        //for (int i = 0; i < options.Length; i++)
        //{
            Instantiate(question_template,new Vector3(0,0,0), Quaternion.identity, transform);
        //}
        Destroy(question_template);

        Canvas canvas = FindObjectOfType<Canvas>();

        yy_enter_curve.val_start = -canvas.GetComponent<RectTransform>().rect.height;                                      
        yy_enter_curve.val_end = transform.localPosition.y;
        
        yy_exit_curve.val_start = yy_enter_curve.val_end;
        yy_exit_curve.val_end = yy_enter_curve.val_start;
    }

    void Update()
    {
        Enter();

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
