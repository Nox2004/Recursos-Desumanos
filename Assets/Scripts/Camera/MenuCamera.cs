using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCamera : MonoBehaviour
{
    [SerializeField] private AnimCurveValue curve;
    [SerializeField] private MenuButtons[] menu_buttons;

    [SerializeField] private float activate_buttons_theshold;

    private float yy;
    // Start is called before the first frame update

    void Awake()
    {
        yy = curve.val_start;
        transform.position = new Vector3(transform.position.x,yy,transform.position.z);

        //turn off all components in menu_buttons
        for (int i = 0; i < menu_buttons.Length; i++)
        {
            menu_buttons[i].enabled = false;
        }
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        yy = curve.Update(Time.deltaTime);
        transform.position = new Vector3(transform.position.x,yy,transform.position.z);

        if (curve.GetRawValue() > activate_buttons_theshold) //turn on all components in menu_buttons
        {
            for (int i = 0; i < menu_buttons.Length; i++)
            {
                menu_buttons[i].enabled = true;
            }
        }
    }
}
