using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drawer : MonoBehaviour
{

    [SerializeField] private float yy_start;
    [SerializeField] private float yy_end;
    [SerializeField] private float yy;
    [SerializeField] private float smoothness;

    [SerializeField] private Vector3 initial_pos;
    [SerializeField] private Jobs job;

    private Collider2D collider;

    // Start is called before the first frame update
    void Start()
    {
        collider = gameObject.GetComponent<Collider2D>();
        yy = yy_start;
        initial_pos = transform.position;

        smoothness/=60;
    }

    // Update is called once per frame
    void Update()
    {
        float y_target;
        if (collider.OverlapPoint(InGameCursor.get_position_in_world()))
        {
            y_target = yy_end;

            //if ()
        }
        else
        {
            y_target = yy_start;
        }

        yy += ((y_target-yy) / smoothness) * Time.deltaTime;

        transform.position = initial_pos + Vector3.down * yy;
        collider.offset = Vector3.up * (yy - 2);
    }
}
