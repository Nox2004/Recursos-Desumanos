using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Document : MonoBehaviour
{
    public Camera cam;

    //[SerializeField] private GameObject table;
    private PolygonCollider2D tablecollider;
    private Vector3 start_pos;
    private Vector3 mouse_pos;
    private Vector3 pos;

    private float y_offset = 0;
    [SerializeField] private float y_offtarget = 1; private float y_offsmooth = 4.0f / 60;
    
    private SpriteRenderer sprite_renderer;
    private Material material;
    private PolygonCollider2D collider;
    private Perspective persp_script;

    private GameObject my_shadow;
    SpriteRenderer shadow_rend;

    private bool isDragging = false;

    void Start()
    {
        pos = transform.localPosition;
        
        sprite_renderer = gameObject.GetComponent<SpriteRenderer>();
        material = sprite_renderer.material;
        collider = gameObject.GetComponent<PolygonCollider2D>();
        persp_script = gameObject.GetComponent<Perspective>();

        my_shadow = new GameObject("Shadow");
        my_shadow.transform.parent = transform;

        shadow_rend = my_shadow.AddComponent<SpriteRenderer>();
        shadow_rend.sortingLayerName = sprite_renderer.sortingLayerName;
        shadow_rend.sortingOrder = -1;

        shadow_rend.sprite = sprite_renderer.sprite;
        shadow_rend.material = material;
        //tablecollider = table.GetComponent<PolygonCollider2D>();
    }

    private void LateUpdate()
    {
        shadow_rend.material.SetFloat("left_offset", persp_script.xoffleft);
        shadow_rend.material.SetFloat("right_offset", persp_script.xoffright);
        shadow_rend.material.SetInt("color_overlay_on", 1);
        shadow_rend.material.SetColor("color_overlay", new Color(0,0,0,0.3f));
    }

    private void Update()
    {
        mouse_pos = Input.mousePosition;
        mouse_pos = cam.ScreenToWorldPoint(mouse_pos);

        material.SetColor("blend_color",new Color(0.8f,0.8f,0.8f,1f));
        if (collider.OverlapPoint(new Vector2(mouse_pos.x, mouse_pos.y)))
        {
            material.SetColor("blend_color",new Color(1f,1f,1f,1f));
        }

        //if (!tablecollider.OverlapPoint(new Vector2(mouse_pos.x, mouse_pos.y)))
        //{
        //    isDragging = false;
        //}

        if (isDragging)
        {
            y_offset += ((y_offtarget-y_offset)/y_offsmooth) * Time.deltaTime;
            DragObject();
        }
        else 
        {
            y_offset = 0;
        }
        Debug.Log(y_offset);
        transform.localPosition = new Vector3(pos.x,pos.y+y_offset,pos.z);

        my_shadow.transform.localPosition = pos;
    }

    private void OnMouseDown()
    {
        start_pos = mouse_pos - pos;

        isDragging = true;
    }

    private void OnMouseUp()
    {
        isDragging = false;
    }

    public void DragObject()
    {
        Vector3 mouse_pos = Input.mousePosition;

        mouse_pos = cam.ScreenToWorldPoint(mouse_pos);
        pos = new Vector3(mouse_pos.x - start_pos.x, mouse_pos.y - start_pos.y, pos.z);
    }
}
