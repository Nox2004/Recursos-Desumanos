using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameCursor : MonoBehaviour
{
    private Camera cam; //Camera
    [SerializeField] private float mouse_spd;
    [SerializeField] private float mouse_spd_arcade;

    //Cursor position
    public static Vector2 cursor_pos;

    //Sprite to being applied when the mouse is idle, and when its clicking
    [SerializeField] private Sprite cursor_normal;
    [SerializeField] private Sprite cursor_holding;
    [SerializeField] private Sprite cursor_over_ui;
    [SerializeField] private Sprite cursor_clicking;

    [SerializeField] private AudioClip in_sound;
    [SerializeField] private AudioClip out_sound;
    [SerializeField] private AudioClip click_sound;

    private AudioSource in_source, out_source, click_source;

    public static bool over_ui;
    private bool over_ui_last_frame;

    private Image render; //Renderer

    private RectTransform rect_transform;

    private void Start()
    {
        #region //Audio sources
        in_source = gameObject.AddComponent<AudioSource>();
        in_source.clip = in_sound;
        in_source.loop = false;

        out_source = gameObject.AddComponent<AudioSource>();
        out_source.clip = out_sound;
        out_source.loop = false;

        click_source = gameObject.AddComponent<AudioSource>();
        click_source.clip = click_sound;
        click_source.loop = false;
        #endregion

        over_ui = false;
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

        var mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        cursor_pos += mouseDelta * mouse_spd;
        cursor_pos += new Vector2(Input.GetAxis("HORIZONTAL0"), Input.GetAxis("VERTICAL0")) * Time.deltaTime * mouse_spd_arcade;
        cursor_pos = new Vector2(Mathf.Clamp(cursor_pos.x,0,Screen.width), Mathf.Clamp(cursor_pos.y,0,Screen.height));
    }

    void LateUpdate()
    {
        if (over_ui && !over_ui_last_frame)
        {
            in_source.Play();
        }
        if (!over_ui && over_ui_last_frame)
        {
            out_source.Play();
        }
        if (get_button_down(0))
        {
            click_source.Play();
        }

        over_ui_last_frame = over_ui;

        //Set sprites
        render.sprite = cursor_normal;

        if (over_ui)
        {
            render.sprite = cursor_over_ui;
        }
        if (Input.GetMouseButton(0))
        {
            render.sprite = cursor_clicking;
        }
        if (Input.GetMouseButton(1))
        {
            render.sprite = cursor_holding;
        }

        over_ui = false;
    }

    private void Awake()
    {
        //Find the camera and setup cam variable
        cam = GameObject.FindGameObjectsWithTag("MainCamera")[0].GetComponent<Camera>();
        
        DontDestroyOnLoad(transform.parent.gameObject);
    }

    public static bool get_button(int button) 
    { 
        return ((Input.GetMouseButton(button)) || ((button==0) ? Input.GetButton("VERDE0") : Input.GetButton("VERMELHO0")));
    }

    public static bool get_button_down(int button) 
    { 
        return ((Input.GetMouseButtonDown(button)) || ((button==0) ? Input.GetButtonDown("VERDE0") : Input.GetButtonDown("VERMELHO0")));
    }
    
    public static bool get_button_up(int button) 
    { 
        return ((Input.GetMouseButtonUp(button)) || ((button==0) ? Input.GetButtonUp("VERDE0") : Input.GetButtonUp("VERMELHO0")));
    }

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
