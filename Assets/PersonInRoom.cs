using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonInRoom : MonoBehaviour
{
    public bool exiting = false;

    public float start_x;
    private float xx;
    [SerializeField] private AnimCurveValue xx_enter_curve;
    [SerializeField] private AnimCurveValue xx_exit_curve;

    // Start is called before the first frame update
    void Start()
    {
        xx_exit_curve.val_start = xx_enter_curve.val_end;
        xx_exit_curve.val_end = xx_enter_curve.val_start;

        transform.position = new Vector3(xx,transform.position.y,transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        if (exiting)
        {
            Exit();
        }
        else 
        {
            Enter();
        }

        transform.position = new Vector3(xx,transform.position.y,transform.position.z);
    }

    void Enter()
    {
        xx = xx_enter_curve.Update(Time.deltaTime);
    }

    void Exit()
    {
        xx = xx_exit_curve.Update(Time.deltaTime);
    }

    public bool HasEntered()
    {
        if ((!exiting) && (xx_enter_curve.GetRawValue()>=1)) return true;
        return false;
    }

    public bool HasExited()
    {
        if ((exiting) && (xx_exit_curve.GetRawValue()>=1)) return true;
        return false;
    }
}
