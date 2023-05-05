using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionImage : MonoBehaviour
{
    public Canvas my_canvas;
    //Image scale
    private float scale=1;

    //Serialized stuff (regarding the procedural animation)
    [SerializeField] private float scale_min, scale_max, smoothness, wave_speed, wave_size;
    private float time = 0;

    private Image uiImage; // Reference to the UI image
    public string text;
    private TMPro.TextMeshProUGUI text_obj;

    public bool control = true;
    public bool selected = false;

    private ImageButton button;

    void Start()
    {
        //Get image component
        uiImage = gameObject.GetComponent<Image>();
        smoothness/=60;

        text_obj = transform.GetChild(0).gameObject.GetComponent<TMPro.TextMeshProUGUI>();
        text_obj.text = text;

        button = new ImageButton();
    }

    void Update()
    {
        //Time variable (in 60fps frames)
        time+=Time.deltaTime*60;

        float starget = scale_min; //Scale target

        //Changes color and target scale
        if ((button.cursor_over(uiImage)) && (control==true)) //Checks if mouse is inside image's bounds
        {
            uiImage.color = new Color(1,1,1,1);
            starget = scale_max;

            if (InGameCursor.get_button_down(0))
            {
                selected = true;
            }
        }
        else
        {
            uiImage.color = new Color(0.9f,0.9f,0.9f,1);
            starget = scale_min;
        }

        //Smooth transition from current scale to target scale
        scale += ((starget-scale)/smoothness) * Time.deltaTime;

        //Debug.Log("max - " + scale_max + "     target - " + starget + "     scale -" + scale);

        var real_scale = scale + ((Mathf.Sin(time*wave_speed*Mathf.Deg2Rad)+1)/2) * wave_size; //Applies wavey scaling
        transform.localScale = new Vector3(real_scale,real_scale,1);
    }
}
