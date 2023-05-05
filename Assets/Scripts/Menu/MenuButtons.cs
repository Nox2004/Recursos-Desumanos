using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MenuButtons : MonoBehaviour
{
    public bool control = true;
    [SerializeField] private MenuButtons[] other_buttons;

    [SerializeField] private AnimCurveValue curve;
    private float yy;

    [SerializeField] private UnityEvent my_event;
    [SerializeField] private float scale_spd;
    [SerializeField] private float scale_variation;

    [SerializeField] private float normal_scale;
    [SerializeField] private float mouse_over_scale;
    [SerializeField] private Color normal_color;
    [SerializeField] private Color mouse_over_color;

    Image image;
    ImageButton button;

    void Awake()
    {
        yy = curve.val_start;
        transform.position = new Vector3(transform.position.x,yy,transform.position.z);
    }
    
    void Start()
    {
        image = GetComponent<Image>();
        button = new ImageButton();
    }

    // Update is called once per frame
    void Update()
    {
        bool mouse_over = button.cursor_over(image);
        if (!control) mouse_over = false;

        var scale = (mouse_over ? mouse_over_scale : normal_scale) + (Mathf.Sin(Time.realtimeSinceStartup/((scale_spd/2)/60)) * (scale_variation));
        transform.localScale = Vector3.one * scale;

        image.color = (mouse_over ? mouse_over_color : normal_color);

        if (mouse_over)
        {
            if (InGameCursor.get_button_down(0))
            {
                my_event.Invoke();
                for (int i = 0; i < other_buttons.Length; i++)
                {
                    other_buttons[i].control = false;
                }
            }
        }

        yy = curve.Update(Time.deltaTime);
        transform.localPosition = new Vector3(transform.localPosition.x,yy,transform.localPosition.z);
    }

    public void start_game()
    {
        Singleton.Instance.create_transition(TransitionMode.Scene,"DayOne");
    }

    public void end_game()
    {
        Singleton.Instance.create_transition(TransitionMode.Exit);
    }
}
