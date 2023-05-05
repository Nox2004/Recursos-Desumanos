using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragWithShadow : Drag
{
    //Document movement variables
    protected float y_offset = 0;
    [SerializeField] protected float y_offtarget = 1; protected float y_offsmooth = 4.0f / 60;
    
    //Important components
    protected SpriteRenderer sprite_renderer;
    protected Material material;
    
    //Shadow stuff
    protected GameObject my_shadow;
    protected SpriteRenderer shadow_rend;

    protected void Start()
    {
        base.Start();
        //Sets up component variables
        sprite_renderer = gameObject.GetComponent<SpriteRenderer>();
        material = sprite_renderer.material;

        create_shadow();

        material.SetInt("blend_color_on", 1);
    }

    protected void Update()
    {
        base.Update();

        //Highlights the document if being selected
        if (mouse_over() || is_dragging)
        {
            material.SetColor("blend_color",new Color(1f,1f,1f,1f));
        }
        else
        {
            material.SetColor("blend_color",new Color(0.8f,0.8f,0.8f,1f));
        }

        //When dragging
        if (is_dragging)
        {
            
            y_offset += ((y_offtarget-y_offset)/y_offsmooth) * Time.deltaTime;
        }
        else
        {
            y_offset += ((0-y_offset)/y_offsmooth) * Time.deltaTime;
        }

        //Applies offset
        transform.position = pos + Vector3.up*y_offset; //Applies offset

        //Applies shadow offset
        my_shadow.transform.position = pos;
    }

    protected void create_shadow()
    {
        //Creates a child shadow
        my_shadow = new GameObject("Shadow");
        my_shadow.transform.parent = transform;
        my_shadow.transform.localPosition = new Vector3(0,0,0);

        //Add a sprite renderer and sets its sprite and mat
        shadow_rend = my_shadow.AddComponent<SpriteRenderer>();
        shadow_rend.sprite = sprite_renderer.sprite;
        shadow_rend.material = sprite_renderer.material;

        //Applies a low opacity grey overlay to the shadow shader
        shadow_rend.material.SetInt("color_overlay_on", 1);
        shadow_rend.material.SetColor("color_overlay", new Color(0,0,0,0.3f));

        //Sets the shadow to be behind the sprite
        shadow_rend.sortingLayerName = sprite_renderer.sortingLayerName;
        shadow_rend.sortingOrder = sprite_renderer.sortingOrder - 1;
    }
}
