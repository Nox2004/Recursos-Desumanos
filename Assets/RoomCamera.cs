using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomCamera : MonoBehaviour
{
    public bool locked;
    [SerializeField] private float yy_start;

    [SerializeField] private float cursor_y_move_threshold;
    [SerializeField] private float yy_end;
    
    void Start()
    {
        
    }

    void Update()
    {
        //InGameCursor.get_position_in_screen().y - Screen.height*cursor_y_move_threshold;
    }
}
