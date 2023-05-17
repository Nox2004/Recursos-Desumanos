using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragObject : DragInTable
{
    //Perspective
    protected Perspective persp_script;

    //Sound
    [SerializeField] protected AudioClip pick_sound;
    [SerializeField] protected AudioClip drop_sound;

    protected AudioSource audio_source;
    protected bool is_dragging_last_frame = false;
    protected float angle = 0;
    protected PaperManager paper_manager;

    protected void Start()
    {
        base.Start();
   
        //Sets up component variables
        barrier_collider = GameObject.Find("Table").GetComponent<PolygonCollider2D>();
        pol_collider = GetComponent<PolygonCollider2D>();
        collider = GetComponent<BoxCollider2D>();
        paper_manager = PaperManager.DocManager;

        //Apply perspective to the shadow
        if (persp_script != null) 
        {
            my_shadow.AddComponent<Perspective>();
            my_shadow.tag = "NoPerspective"; //So it wont be added to the "perspective child" list
        }

        //Sets up audio
        audio_source = gameObject.AddComponent<AudioSource>();
        audio_source.loop = false;

        Material material = sprite_renderer.material;
        
        //Debug.Log("out " + material.GetValue("outline_on"));
        //Debug.Log("blend " + material.GetValue("blend_color_on"));
        //Debug.Log("color_overlay_on " + material.GetValue("color_overlay_on"));
        //Debug.Log("color_mix_on " + material.GetValue("color_mix_on"));

        material.SetInt("blend_color_on", 0);
    }
    
    protected void Update()
    {
        Vector3 last_pos = pos;

        shadow_rend.sortingOrder = sprite_renderer.sortingOrder-1; //So the shadow is behind the object

        base.Update();
        if (mouse_over() || is_dragging)
        {
            paper_manager.selecting_obj = true;
        }

        if (is_dragging && !is_dragging_last_frame)
        {
            audio_source.clip = pick_sound;
            audio_source.Play();
        }
        else if (!is_dragging && is_dragging_last_frame)
        {
            audio_source.clip = drop_sound;
            audio_source.Play();
        }

        is_dragging_last_frame = is_dragging;

        if (is_dragging) angle += (last_pos.x - pos.x) * 14;

        transform.localRotation = Quaternion.Euler(0,0,angle);
        my_shadow.transform.rotation = Quaternion.identity;
        angle /= 1.07f;
    }
}
