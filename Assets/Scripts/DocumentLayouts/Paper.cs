using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paper : DragInTable
{
    //Perspective
    protected Perspective persp_script;

    //Document layer
    public int order = 0;
    public bool selected = false;

    //Sound
    [SerializeField] protected AudioClip pick_sound;
    [SerializeField] protected AudioClip drop_sound;

    protected AudioSource audio_source;
    protected bool is_dragging_last_frame = false;

    protected override bool mouse_over()
    {
        if (selected)
        {
            return true;
        }
        return false;
    }

    protected override bool picking()
    {
        //If the mouse is over the document and the left mouse button is pressed
        if (mouse_over() && InGameCursor.get_button_down(1))
        {
            PaperManager.DocManager.get_document(this); //Docmanager selects this document

            is_dragging = true;
            return true;
        }
        return false;
    }

    protected override bool drop()
    {
        //If the mouse is over the document and the left mouse button is released
        if (is_dragging && InGameCursor.get_button_up(1))
        {
            PaperManager.DocManager.drop_document(); //Docmanager drops this document
            is_dragging = false;
            return true;
        }
        return false;
    }

    protected void Start()
    {
        base.Start();

        persp_script = gameObject.GetComponent<Perspective>();
        PaperManager.DocManager.document_list.Add(this);

        pol_collider = (PolygonCollider2D) collider;

        //Sets up component variables
        barrier_collider = GameObject.Find("Table").GetComponent<PolygonCollider2D>();

        //Apply perspective to the shadow
        if (persp_script != null) 
        {
            my_shadow.AddComponent<Perspective>();
            my_shadow.tag = "NoPerspective"; //So it wont be added to the "perspective child" list
        }

        //Sets up audio
        audio_source = gameObject.AddComponent<AudioSource>();
        audio_source.loop = false;
    }
    
    protected void Update()
    {
        //Sets up childs sorting order
        foreach (Transform child in transform)
        {
            if (child.GetComponent<SpriteRenderer>() != null)
            {
                child.gameObject.GetComponent<SpriteRenderer>().sortingOrder = order;
            }
            else
            {
                child.gameObject.GetComponent<MeshRenderer>().sortingOrder = order;
            }
        }

        sprite_renderer.sortingOrder = order - 1; //So the document is behind its childs
        shadow_rend.sortingOrder = order - 2; //So the shadow is behind the document

        base.Update();

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
    }

    protected void OnDestroy() 
    {
        PaperManager.DocManager.document_list.Remove(this);
    }
}
