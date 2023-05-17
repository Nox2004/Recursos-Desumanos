using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drag : MonoBehaviour
{
    //Mouse Stuff
    protected Vector3 start_pos;
    protected Vector3 mouse_pos;
    protected Vector3 pos;
    protected Camera cam; //Camera

    protected Collider2D collider;

    //Is the document being dragged by the mouse?
    protected bool is_dragging = false;

    protected void Start()
    {
        collider = gameObject.GetComponent<Collider2D>();

        cam = Singleton.Instance.cam; //Sets up cam
        
        pos = transform.position; //position
    }

    protected void Update()
    {
        if (Singleton.Instance.game_paused) return;
        
        //Gets mouse position in the world view
        mouse_pos = InGameCursor.get_position_in_world();

        if (picking())
        {
            start_pos = mouse_pos - pos; //Sets up start position
        }
        
        if (drop())
        {
            
        }
        
        //When dragging
        if (is_dragging)
        {
            pos = drag_object();
        }

        //Applies offset
        transform.position = pos; //Applies offset
    }
    
    protected virtual bool mouse_over()
    {
        if (collider.OverlapPoint(mouse_pos))
        {
            return true;
        }	
        return false;
    }

    protected virtual bool picking()
    {
        //If the mouse is over the document and the left mouse button is pressed
        if (mouse_over() && InGameCursor.get_button_down(1))
        {
            is_dragging = true;
            return true;
        }
        return false;
    }

    protected virtual bool drop()
    {
        //If the mouse is over the document and the left mouse button is released
        if (is_dragging && InGameCursor.get_button_up(1))
        {
            is_dragging = false;
            return true;
        }
        return false;
    }

    protected Vector3 drag_object()
    {
        //Follows the mouse
        return new Vector3(mouse_pos.x - start_pos.x, mouse_pos.y - start_pos.y, pos.z);
    }
}
