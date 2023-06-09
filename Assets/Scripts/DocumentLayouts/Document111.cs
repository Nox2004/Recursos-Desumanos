using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
public class Document : MonoBehaviour
{
    //Table collider
    private PolygonCollider2D tablecollider;

    //Mouse Stuff
    private Vector3 start_pos;
    private Vector3 mouse_pos;
    private Vector3 pos;
    private Camera cam; //Camera
    
    //Document movement variables
    [SerializeField] private float y_offset = 0;
    [SerializeField] private float y_offtarget = 1; private float y_offsmooth = 4.0f / 60;
    
    //Important components
    private SpriteRenderer sprite_renderer;
    private Material material;
    public PolygonCollider2D my_collider;
    private Perspective persp_script;

    //Shadow stuff
    private GameObject my_shadow;
    SpriteRenderer shadow_rend;

    //Is the document being dragged by the mouse?
    private bool isDragging = false;

    //Document layer
    public int order = 0;
    public bool selected = false;

    public void Start()
    {
        cam = Singleton.Instance.cam; //Sets up cam
        PaperManager.DocManager.document_list.Add(this); //Add the document to the docmanager list

        tablecollider = GameObject.FindGameObjectsWithTag("Table")[0].GetComponent<PolygonCollider2D>();
        composite_collider = gameObject.GetComponent<CompositeCollider2D>();

        pos = transform.position; //position
        
        //Sets up component variables
        sprite_renderer = gameObject.GetComponent<SpriteRenderer>();
        material = sprite_renderer.material;
        my_collider = gameObject.GetComponent<PolygonCollider2D>();
        persp_script = gameObject.GetComponent<Perspective>();

        //Creates a child shadow
        my_shadow = new GameObject("Shadow");
        my_shadow.transform.parent = transform;
        my_shadow.transform.localPosition = new Vector3(0,0,0);

        //Add a sprite renderer and sets its sprite and mat
        shadow_rend = my_shadow.AddComponent<SpriteRenderer>();
        shadow_rend.sprite = sprite_renderer.sprite;
        shadow_rend.material = material;

        //Applies a low opacity grey overlay to the shadow shader
        shadow_rend.material.SetInt("color_overlay_on", 1);
        shadow_rend.material.SetColor("color_overlay", new Color(0,0,0,0.3f));

        //Sets up shadow layer
        shadow_rend.sortingLayerName = sprite_renderer.sortingLayerName;
        shadow_rend.sortingOrder = 0;

        //Apply perspective to the shadow
        if (persp_script != null) my_shadow.AddComponent<Perspective>();
        my_shadow.tag = "NoPerspective"; //So it wont be added to the "perspective child" list
    }

    private void Update()
    {
        //Gets mouse position in the world view
        mouse_pos = InGameCursor.get_position_in_world();

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

        //Makes the document "greyish"
        material.SetInt("blend_color_on", 1);
        material.SetColor("blend_color",new Color(0.8f,0.8f,0.8f,1f));

        if (selected || isDragging)
        {
            //Highlights the document if being selected
            material.SetColor("blend_color",new Color(1f,1f,1f,1f));

            if (InGameCursor.get_button_down(1))
            {
                start_pos = mouse_pos - pos; //Sets up start position
                isDragging = true;

                DocumentManager.DocManager.get_document(this); //Docmanager selects this document
            }

            //Drop the document
            if (InGameCursor.get_button_up(1))
            {
                isDragging = false;
                DocumentManager.DocManager.drop_document();
            }
        }

        //When dragging
        if (isDragging)
        {
            //Changes mouse position to be at the middle of the document
            if (persp_script != null)
            {
                persp_script.set_mouse_at_middle();
            }
            else
            {
                //CursorScript.cursor_pos = transform.position;
            }

            //Adds a smooth y offset to the document
            y_offset += ((y_offtarget-y_offset)/y_offsmooth) * Time.deltaTime;
            DragObject();
        }
        else 
        {
            y_offset = 0; //Resets y offset
        }
        
        //Applies offset
        

        //Sets shadow position
        my_shadow.transform.position = pos;

         // Check if any of the object's collider points are outside the collider
        foreach (var point in my_collider.points)
        {
            Vector2 pointWorldPosition = (Vector2) my_shadow.transform.TransformPoint(point);
            Vector2 closestPoint = tablecollider.ClosestPoint(pointWorldPosition);

            if (!tablecollider.OverlapPoint(closestPoint))
            {
                Vector2 moveDirection = closestPoint - pointWorldPosition;
                my_shadow.transform.position += (Vector3) moveDirection;
                pos = my_shadow.transform.position;
            }
        }

        //Applies offset
        transform.position = pos + Vector3.up*y_offset; 
        my_shadow.transform.position = transform.position + Vector3.down*y_offset;
    }
    private void LateUpdate()
    {
        //shadow_rend.material.SetFloat("left_offset", persp_script.xoffleft);
        //shadow_rend.material.SetFloat("right_offset", persp_script.xoffright);
    }

    public void DragObject()
    {
        //Follows the mouse
        pos = new Vector3(mouse_pos.x - start_pos.x, mouse_pos.y - start_pos.y, pos.z);
        //transform.position = pos;
    }
}
*/