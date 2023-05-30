using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndTextPlaceholder : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI fp_text;
    [SerializeField] private TMPro.TextMeshProUGUI corp_text;
    [SerializeField] private TMPro.TextMeshProUGUI moral_text;
    [SerializeField] private TMPro.TextMeshProUGUI result_text;

    public bool control = true;
    [SerializeField] private MenuButtons[] other_buttons;

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
        fp_text.text = Singleton.Instance.future_points.ToString();
        corp_text.text = Singleton.Instance.corporate_points.ToString();
        moral_text.text = Singleton.Instance.moral_points.ToString();

        if (Singleton.Instance.corporate_points > 5)
        {
            result_text.text = "contratado!!!";
            result_text.color = Color.green;
        }
        else
        {
            result_text.text = "não efetivado";
            result_text.color = Color.red;
        }
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
                Singleton.Instance.go_to_menu();
                Singleton.Instance.reset();
                for (int i = 0; i < other_buttons.Length; i++)
                {
                    other_buttons[i].control = false;
                }
            }
        }
    }
}
