using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Document : Paper
{
    protected Person my_person; 
    protected PolygonCollider2D table_collider;
    protected bool locked = true; //if false, can go inside drawer

    protected void Start()
    {
        base.Start();

        table_collider = GameObject.Find("Table").GetComponent<PolygonCollider2D>();
        //change barrier/arena so the document can be dragged bellow the table
        barrier_collider = GameObject.Find("DocumentArena").GetComponent<PolygonCollider2D>();
    }
    
    protected void Update()
    {
        base.Update();

        //When exiting table:
        //deactivates shadow
        //deactivates perspective (or just change offset values in laterUpdate)
        

        //Checks if the document is inside the table
        if (!table_collider.OverlapPoint(transform.position))
        {
            //If it is, activates shadow
            //activates perspective (or just change offset values in laterUpdate)
            my_shadow.SetActive(false);
            persp_script.enabled = false;
            y_offset = 0;
            material.SetFloat("left_offset", 0);
            material.SetFloat("right_offset", 0);

            foreach (Perspective.SpriteChild child in persp_script.sprite_childs)
            {
                child.transform.localPosition = child.localpos;
            
                child.sprite_renderer.material.SetFloat("left_offset", 0);
                child.sprite_renderer.material.SetFloat("right_offset", 0);
                
            }

            foreach (Perspective.TextChild child in persp_script.text_childs)
            {
                child.transform.localPosition = child.localpos;
            
                child.material.SetFloat("left_offset", 0);
                child.material.SetFloat("right_offset", 0);
                
            }
        }
        else
        {
            my_shadow.SetActive(true);
            persp_script.enabled = true;
        }
    }

    //When exiting table collider
    protected void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log("AAAAAAAAA");
        if (other == (Collider2D) table_collider)
        {
            //deactivates shadow
            //deactivates perspective (or just change offset values in laterUpdate)
            my_shadow.SetActive(false);
            persp_script.enabled = false;
            y_offset = 0;
            material.SetFloat("left_offset", 0);
            material.SetFloat("right_offset", 0);
        }
    }
}
