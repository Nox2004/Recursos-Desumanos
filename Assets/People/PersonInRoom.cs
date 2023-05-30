using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonInRoom : MonoBehaviour
{
    private SpriteRenderer sprite_renderer;

    public bool exiting = false;

    public float start_x;
    private float xx;

    private float initial_y;
    [SerializeField] private AnimCurveValue xx_enter_curve;
    [SerializeField] private AnimCurveValue xx_exit_curve;

    
    [SerializeField] private float y_offset_animation_ratio;
    
    [SerializeField] private Color initial_color;

    [SerializeField] private float y_initial_offset;
    private float y_offset_start, y_offset_end, y_offset;

    public bool talking = false;
    public Sprite idle_sprite;
    public Sprite[] talking_animation;
    [SerializeField] private float frame_duration;

    //Sound
    [SerializeField] protected AudioClip moving_sound;
    protected AudioSource audio_source;
    protected bool is_exiting_last_frame = false;

    void Awake()
    {
        audio_source = gameObject.AddComponent<AudioSource>();
        audio_source.pitch = 1.2f;
        audio_source.PlayOneShot(moving_sound);

        xx_exit_curve.val_start = xx_enter_curve.val_end;
        xx_exit_curve.val_end = xx_enter_curve.val_start;

        y_offset_end = 0;
        y_offset_start = y_initial_offset;
        y_offset = y_offset_start;
        
        initial_y = transform.position.y;
        xx = xx_enter_curve.val_start;
        transform.position = new Vector3(xx,initial_y+y_offset,transform.position.z);
    }

    // Start is called before the first frame update
    void Start()
    {
        sprite_renderer = GetComponent<SpriteRenderer>();
        sprite_renderer.sprite = idle_sprite;

        sprite_renderer.material.SetInt("color_mix_on",1);
        sprite_renderer.material.SetColor("color_mix",initial_color);

        frame_duration/=60;
    }

    // Update is called once per frame
    void Update()
    {
        if (Singleton.Instance.game_paused) return;

        //Adds a wavey effect to the person
        //height - 0.04  duration 7/pi per second
        float yy = Mathf.Sin(Time.realtimeSinceStartup*7)*0.04f;

        if (exiting && !is_exiting_last_frame)
        {
            audio_source.pitch = 0.8f;
            audio_source.PlayOneShot(moving_sound);
        }

        is_exiting_last_frame = exiting;

        if (exiting)
        {
            Exit();
        }
        else 
        {
            Enter();
        }

        transform.position = new Vector3(xx,initial_y+yy+y_offset,transform.position.z);
    }

    private void LateUpdate()
    {
        if (Singleton.Instance.game_paused) return;

        if (talking)
        {
            int frame = Mathf.FloorToInt((Time.realtimeSinceStartup/frame_duration) % talking_animation.Length);
            sprite_renderer.sprite = talking_animation[frame];
            //Debug.Log(frame);
        }
        else
        {
            sprite_renderer.sprite = idle_sprite;
        }

        talking = false;
    }

    void Enter()
    {
        xx = xx_enter_curve.Update(Time.deltaTime);
        
        var ratio = xx_enter_curve.GetRawValue();
        if (ratio < y_offset_animation_ratio) ratio = ratio / y_offset_animation_ratio;
        else ratio = 1;

        sprite_renderer.material.SetFloat("color_mix_strength",1-ratio);
        y_offset = Mathf.Lerp(y_offset_start,y_offset_end,ratio);
    }

    void Exit()
    {
        xx = xx_exit_curve.Update(Time.deltaTime);
        
        var ratio = (1-xx_exit_curve.GetRawValue());
        if (ratio < y_offset_animation_ratio) ratio = ratio / y_offset_animation_ratio;
        else ratio = 1;

        sprite_renderer.material.SetFloat("color_mix_strength",1-ratio);
        y_offset = Mathf.Lerp(y_offset_start,y_offset_end,ratio);
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
