using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomCamera : MonoBehaviour
{
    public bool locked = true;
    [SerializeField] private float yy_start;
    [SerializeField] private float yy_end;

    [SerializeField] private float cursor_y_move_threshold; // 0 -> cant move    1 -> whole scree
    [SerializeField] private float cursor_y_move_spd;

    void Start()
    {
        
    }

    void Update()
    {
        if (Singleton.Instance.game_paused) return;

        if (!locked)
        {
            float yy_increase = 0;
            yy_increase -= Mathf.Max(Screen.height*cursor_y_move_threshold-InGameCursor.get_position_in_screen().y,0);
            yy_increase += Mathf.Max(InGameCursor.get_position_in_screen().y-Screen.height*(1-cursor_y_move_threshold),0);

            yy_increase *= Time.deltaTime;
            yy_increase *= cursor_y_move_spd;

            transform.position += Vector3.up * yy_increase;

            transform.position = new Vector3(transform.position.x,Mathf.Clamp(transform.position.y,yy_end, yy_start),transform.position.z);
        }
    }
}
