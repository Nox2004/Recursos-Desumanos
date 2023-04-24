using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameCursor : MonoBehaviour
{
    private Camera cam; //Camera
    [SerializeField] private float mouse_spd;

    //Cursor position
    public static Vector2 cursor_pos;

    //Sprite to being applied when the mouse is idle, and when its clicking
    [SerializeField] private Sprite mouse;
    [SerializeField] private Sprite mouse_clicking;
    private Image render; //Renderer

    private RectTransform rect_transform;

    private void Start()
    {
        render = gameObject.GetComponent<Image>();
        rect_transform = gameObject.GetComponent<RectTransform>();

        //Makes the cursor invisible
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        cursor_pos = new Vector2(Screen.width / 2f, Screen.height / 2f);
    }

    private void Update()
    {
        //Sets position
        transform.position = (Vector3) cursor_pos;

        //Gets real mouse position
        var mouse_pos = Input.mousePosition;
        //mouse_pos = cam.ScreenToWorldPoint(mouse_pos);

        //Changes sprite if mouse is clicking
        if (get_button(1))
        {
            render.sprite = mouse_clicking;
        }
        else
        {
            render.sprite = mouse;
        }

        var mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        cursor_pos += mouseDelta * mouse_spd;
        cursor_pos = new Vector2(Mathf.Clamp(cursor_pos.x,0,Screen.width), Mathf.Clamp(cursor_pos.y,0,Screen.height));
    }

    private void Awake()
    {
        //Find the camera and setup cam variable
        cam = GameObject.FindGameObjectsWithTag("MainCamera")[0].GetComponent<Camera>();
        
        DontDestroyOnLoad(gameObject);
        DontDestroyOnLoad(transform.parent.gameObject);
    }

    public static bool get_button(int button) { return Input.GetMouseButton(button); }

    public static bool get_button_down(int button) { return Input.GetMouseButtonDown(button); }
    
    public static bool get_button_up(int button) { return Input.GetMouseButtonUp(button); }

    // Get the position of the image in world space
    public static Vector2 get_position_in_world()
    {
        // Get the position of the image in screen space
        Vector3 screen_pos = (Vector3) cursor_pos;

        // Convert the position to world space
        Vector3 world_pos = Camera.main.ScreenToWorldPoint(screen_pos);

        // Return the position as a Vector2
        return new Vector2(world_pos.x, world_pos.y);
    }

    // Get the position of the image in world space
    public static Vector2 get_position_in_screen()
    {
        // Return the position as a Vector2
        return cursor_pos;
    }

    // Get the position of the image in a canvas (for different resolution canvas)
    public static Vector2 get_position_in_canvas(Canvas canvas)
    {
        // Get the position of the image in screen space
        Vector3 screen_pos = (Vector3) cursor_pos;

        // Convert the position to canvas space
        Vector2 canvas_pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, screen_pos, canvas.worldCamera, out canvas_pos);

        // Return the position as a Vector2
        return canvas_pos;
    }
}
