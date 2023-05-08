using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Document : Paper
{
    protected Person my_person; 
    protected PolygonCollider2D table_collider;
    protected bool locked = true; //if false, can go inside drawer

    protected bool last_in_table = true;
    protected bool out_of_table = false;

    protected GameObject[] drawers;

    protected void Start()
    {
        base.Start();

        table_collider = GameObject.FindGameObjectsWithTag("Table")[0].GetComponent<PolygonCollider2D>();
        //change barrier/arena so the document can be dragged bellow the table
        barrier_collider = GameObject.Find("DocumentArena").GetComponent<PolygonCollider2D>();

        drawers = GameObject.FindGameObjectsWithTag("Drawer");
    }
    
    protected void Update()
    {
        base.Update();
        
        bool in_table = table_collider.OverlapPoint(transform.position);
        //Checks if the document is inside the table
        if (!in_table && last_in_table)
        {
            exit_table();
            out_of_table = true;
        }
        if (in_table && !last_in_table)
        {
            enter_table();
            out_of_table = false;
        }
        last_in_table = in_table;

        if (out_of_table)
        {
            y_offset = 0;
            transform.position = pos;

            if (!is_dragging)
            {
                //Check if document is colliding with a drawer
                foreach (GameObject drawer in drawers)
                {
                    if (drawer.GetComponent<Collider2D>().OverlapPoint(InGameCursor.get_position_in_world()))
                    {
                        Drawer drawer_script = drawer.GetComponent<Drawer>();

                        transform.SetParent(drawer.transform); // Sets parent as drawer
                        transform.localPosition = drawer_script.documents_offset + (drawer_script.document_number*drawer_script.documents_spacing); // Sets position as the next document position
                        
                        SpriteRenderer drawer_sr = drawer.GetComponent<SpriteRenderer>();
                        var layer_name = drawer_sr.sortingLayerName;
                        int layer_order = drawer_sr.sortingOrder + 1 + drawer_script.document_number*2;

                        sprite_renderer.sortingLayerName = layer_name;
                        sprite_renderer.sortingOrder = layer_order;

                        foreach (Transform child in transform)
                        {
                            if (child.GetComponent<SpriteRenderer>() != null)
                            {
                                child.gameObject.GetComponent<SpriteRenderer>().sortingLayerName = layer_name;
                                child.gameObject.GetComponent<SpriteRenderer>().sortingOrder = layer_order+1;
                            }
                            else
                            {
                                child.gameObject.GetComponent<MeshRenderer>().sortingLayerName = layer_name;
                                child.gameObject.GetComponent<MeshRenderer>().sortingOrder = layer_order+1;
                            }
                        }
                        
                        drawer_script.document_number++;
                        drawer_script.people_hired.Add(my_person);

                        //Destroy document drag, shadow and perspective behaviours
                        Destroy(my_shadow);
                        Destroy(persp_script);
                        Destroy(this);

                        return;
                    }
                }

                
                //Vector2 pointWorldPosition = (Vector2) transform.TransformPoint(point);
                Vector2 closestPoint = table_collider.ClosestPoint((Vector2) transform.position + Vector2.up * sprite_renderer.bounds.extents.y);
                transform.position = (Vector3) closestPoint;
                pos = (Vector3) closestPoint;
            }
        }
    }

    protected void exit_table()
    {
        //When exiting table:
        //deactivates shadow
        //deactivates perspective (or just change offset values in laterUpdate)
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

    protected void enter_table()
    {
        //If it is, activates shadow
        //activates perspective (or just change offset values in laterUpdate)
        my_shadow.SetActive(true);
        persp_script.enabled = true;
    }
}
