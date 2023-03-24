using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class document : MonoBehaviour
{
    public Camera cam;

    private float start_x;
    private float start_y;

    private Vector3 pos;

    private bool isDragging = false;

    //[SerializeField] Transform transform;

    void Start()
    {
        pos = transform.localPosition;
    }

    private void Update()
    {
        if (isDragging)
        {
            DragObject();
        }
        transform.localPosition = pos;
    }

    private void OnMouseDown()
    {
        Vector3 mouse_pos = Input.mousePosition;

        mouse_pos = cam.ScreenToWorldPoint(mouse_pos);

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
