using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CursorScript : MonoBehaviour
{
    private Camera cam; //Camera

    //Cursor position
    public static Vector3 cursor_pos;

    //Sprite to being applied when the mouse is idle, and when its clicking
    [SerializeField] private Sprite mouse;
    [SerializeField] private Sprite mouse_clicking;
    private Image render; //Renderer

    private void Start()
    {
        render = gameObject.GetComponent<Image>();

        //Makes the cursor invisible
        Cursor.visible = false;
    }

    private void Update()
    {
        //Sets position
        transform.position = cursor_pos;

        //Gets real mouse position
        var mouse_pos = Input.mousePosition;
        //mouse_pos = cam.ScreenToWorldPoint(mouse_pos);

        //Changes sprite if mouse is clicking
        if (Input.GetMouseButton(0))
        {
            render.sprite = mouse_clicking;
        }
        else
        {
            render.sprite = mouse;
        }

        cursor_pos = new Vector3(mouse_pos.x,mouse_pos.y,0);
    }

    private void Awake()
    {
        //Find the camera and setup cam variable
        cam = GameObject.FindGameObjectsWithTag("MainCamera")[0].GetComponent<Camera>();
        
        DontDestroyOnLoad(gameObject);
    }
}
