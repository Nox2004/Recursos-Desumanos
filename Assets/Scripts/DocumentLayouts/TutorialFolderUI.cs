using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System;
//using TMPro.TextMeshPro;

public class TutorialFolderUI : MonoBehaviour
{
    public TutorialFolder folder_in_room;
    public int stage = 0;
    [SerializeField] private float back_alpha;
    private float yy;
    //Y movement box curves
    [SerializeField] private float start_yy;
    [SerializeField] private AnimCurveValue yy_enter_curve;
    [SerializeField] private AnimCurveValue yy_exit_curve;

    private int current_page = 0;
    [SerializeField] private Sprite[] pages;

    private Image black_background; 
    private Image image;
    private ImageButton button;


    private void Start()
    {
        black_background = GameObject.FindGameObjectsWithTag("BlackCanvas")[0].GetComponent<Image>();

        image = GetComponent<Image>();
        button = new ImageButton();

        image.sprite = pages[current_page];

        //Sets up procedural animation stuff
        yy = start_yy;
        yy_enter_curve.val_start = start_yy;                   yy_exit_curve.val_end = yy_enter_curve.val_start;
        yy_enter_curve.val_end = transform.localPosition.y;    yy_exit_curve.val_start = yy_enter_curve.val_end;
        
        transform.localPosition = new Vector3(transform.localPosition.x,yy,transform.localPosition.z);
    }

    private void Update()
    {
        if (Singleton.Instance.game_paused) return;
        
        switch (stage)
        {
            case 0: {
                enter();
                //Debug.Log(yy_enter_curve.GetRawValue());
                if (yy_enter_curve.GetRawValue()>=1) stage++;
                black_background.color = new Color(0,0,0,yy_enter_curve.GetRawValue()*back_alpha);
            }
            break;
            case 1: {
                if (InGameCursor.get_button_down(0)) 
                {
                    if (button.cursor_over(image))
                    {
                        
                            current_page++;
                        
                        
                    }
                    else
                    {
                        stage++;
                    }
                }
            }
            break;
            case 2: {
                hide();
                
                black_background.color = new Color(0,0,0,(1-yy_exit_curve.GetRawValue())*back_alpha);

                if (yy_exit_curve.GetRawValue()>=1) 
                {
                    folder_in_room.current_ui = null;
                    Destroy(gameObject);
                }
            }
            break;
        }

        current_page = Mathf.Clamp(current_page,0,pages.Length-1);
        image.sprite = pages[current_page];

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