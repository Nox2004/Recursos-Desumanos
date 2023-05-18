using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinishButton : MonoBehaviour
{
    public bool activacted = false;

    private float scale;
    [SerializeField] private float smooth_scale;
    [SerializeField] private float scale_over;
    [SerializeField] private float scale_out;

    [SerializeField] private float yy_start;
    [SerializeField] private float yy_end;
    [SerializeField] private float smooth_y;

    private float yy;
    public Image image;

    private ImageButton button;
    
    // Start is called before the first frame update
    void Start()
    {
        //transform.localScale = Vector3.one * scale;
        image = GetComponent<Image>();
        button = new ImageButton();
        
        yy = yy_start;
        transform.localPosition = Vector3.up * yy;

        scale = scale_out;
    }

    // Update is called once per frame
    void Update()
    {
        yy += ((yy_end-yy) / (smooth_y/60)) * Time.deltaTime;

        transform.localPosition = Vector3.up * yy;

        float scale_target = scale_out;
        if (button.cursor_over(image))
        {
            scale_target = scale_over;
            if (InGameCursor.get_button_down(0))
            {
                activacted = true;
            }
        }
        
        scale += ((scale_target-scale) / (smooth_scale/60)) * Time.deltaTime;
    }
}