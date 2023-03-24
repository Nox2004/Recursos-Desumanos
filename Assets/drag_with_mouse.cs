using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class drag_with_mouse : MonoBehaviour
{
    public Camera cam;

    private float start_x;
    private float start_y;

    private bool isDragging = false;

    void Start()
    {

    }

    private void Update()
    {
        if (isDragging)
        {
            DragObject();
        }
    }

    private void OnMouseDown()
    {
        Vector3 mouse_pos = Input.mousePosition;

        mouse_pos = cam.ScreenToWorldPoint(mouse_pos);

        start_x = mouse_pos.x - transform.localPosition.x;
        start_y = mouse_pos.y - transform.localPosition.y;

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
        transform.localPosition = new Vector3(mouse_pos.x - start_x, mouse_pos.y - start_y, transform.localPosition.z);
    }
}
