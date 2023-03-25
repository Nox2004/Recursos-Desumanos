using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class document : MonoBehaviour
{
    public Camera cam;

    [SerializeField] private GameObject table;
    private float start_x;
    private float start_y;

    private Vector3 pos;

    private bool isDragging = false;
    private PolygonCollider2D tablecollider;
    private Vector3 mouse_pos;
    void Start()
    {
        pos = transform.localPosition;

        tablecollider = table.GetComponent<PolygonCollider2D>();
    }

    private void Update()
    {
        mouse_pos = Input.mousePosition;
        mouse_pos = cam.ScreenToWorldPoint(mouse_pos);
        
        if (!tablecollider.OverlapPoint(new Vector2(mouse_pos.x, mouse_pos.y)))
        {
            isDragging = false;
        }

        if (isDragging)
        {
            DragObject();
            
        }
        transform.localPosition = pos;
    }

    private void OnMouseDown()
    {
        start_x = mouse_pos.x - pos.x;
        start_y = mouse_pos.y - pos.y;

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
        pos = new Vector3(mouse_pos.x - start_x, mouse_pos.y - start_y, pos.z);
    }
}
