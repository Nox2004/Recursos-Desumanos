using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragInTable : DragWithShadow
{
    //Table collider
    protected PolygonCollider2D barrier_collider;
    protected PolygonCollider2D pol_collider;

    protected void Start()
    {
        base.Start();
        pol_collider = (PolygonCollider2D) collider;
        //Sets up component variables
        barrier_collider = GameObject.Find("Table").GetComponent<PolygonCollider2D>();
    }
    
    protected void Update()
    {
        base.Update();

        collide(barrier_collider);

        //Applies offset
        transform.position = pos + Vector3.up*y_offset; 
        my_shadow.transform.position = transform.position + Vector3.down*y_offset;
    }

    protected void collide(PolygonCollider2D col)
    {
        foreach (var point in pol_collider.points)
        {
            Vector2 pointWorldPosition = (Vector2) my_shadow.transform.TransformPoint(point);
            Vector2 closestPoint = col.ClosestPoint(pointWorldPosition);

            if (!col.OverlapPoint(closestPoint))
            {
                Vector2 moveDirection = closestPoint - pointWorldPosition;
                my_shadow.transform.position += (Vector3) moveDirection;
                pos = my_shadow.transform.position;
            }
        }
    }
}
