    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class text_perspective : MonoBehaviour
{
    private struct Edge
    {
        public Vector2 upleft;
        public Vector2 upright;
        public Vector2 downleft;
        public Vector2 downright;

        public Edge(Vector2 upl, Vector2 upr, Vector2 downl, Vector2 downr)
        {
            upleft = upl;
            upright = upr;
            downleft = downl;
            downright = downr;
        }
    }


    public Vector2 persp_c; //perspective center
    private SpriteRenderer sprite_renderer;
    private Material material;

    public float tan_left, tan_right, sprite_width, sprite_height, xoffleft, xoffright;
    private Vector3 localpos;
    // Start is called before the first frame update
    void Start()
    {
        sprite_renderer = gameObject.GetComponent<SpriteRenderer>();
        localpos = transform.localPosition;
    }

    //Gets sprite edge positions
    Edge SetupEdges(float width, float height)
    {
        return new Edge(
        new Vector2(transform.position.x - width / 2, transform.position.y + height / 2),
        new Vector2(transform.position.x + width / 2, transform.position.y + height / 2),
        new Vector2(transform.position.x - width / 2, transform.position.y - height / 2),
        new Vector2(transform.position.x + width / 2, transform.position.y - height / 2));
    }

    void CalculateTan(Edge edges)
    {
        tan_left = (edges.upleft.x - persp_c.x) / Mathf.Abs(edges.upleft.y - persp_c.y);
        tan_right = (edges.upright.x - persp_c.x) / Mathf.Abs(edges.upright.y - persp_c.y);
    }

    void CalculateOffset()
    {
        xoffleft = sprite_height * tan_left;
        xoffright = sprite_height * tan_right;
    }

    // Update is called once per frame
    void Update()
    {
        material = sprite_renderer.material;
        Sprite sprite = sprite_renderer.sprite;

        Vector2 pos = new Vector2(transform.position.x, transform.position.y);
        sprite_height = sprite_renderer.bounds.size.y * transform.lossyScale.y;
        sprite_width = sprite_renderer.bounds.size.x * transform.lossyScale.x;

        Vector2 up_left, up_right, down_left, down_right;

        Edge edges = SetupEdges(sprite_height, sprite_width);

        if (transform.parent == null)
        {
            CalculateTan(edges);
            Debug.Log("TAN" + tan_left);

            CalculateOffset();
        }
        else
        {
            var parent_script = transform.parent.gameObject.GetComponent<sprite_perspective>();
            tan_left = parent_script.tan_left;
            tan_right = parent_script.tan_right;

            CalculateOffset();

            float parent_height = transform.parent.gameObject.GetComponent<SpriteRenderer>().bounds.size.y;// * transform.parent.lossyScale.y;
            float parent_top = parent_height / 2;

            float mid = (tan_left + tan_right) / 2;

            float relative_x = localpos.x + (parent_height - (parent_top + localpos.y)) * mid;
            relative_x -= (xoffleft + xoffright) / 4;

            transform.localPosition = new Vector3(relative_x, localpos.y, localpos.z);
        }

        edges.downleft = new Vector2(edges.upleft.x + xoffleft, edges.downleft.y);
        material.SetFloat("left_offset", xoffleft); 
        edges.downright = new Vector2(edges.downright.x + xoffright, edges.downright.y); 
        material.SetFloat("right_offset", xoffright);

        if (Input.GetKeyDown(KeyCode.D))
        {
            Debug.DrawLine(edges.upleft, edges.upright, Color.red, 100.0f);
            Debug.DrawLine(edges.upright, edges.downright, Color.red, 100.0f);
            Debug.DrawLine(edges.downright, edges.downleft, Color.red, 100.0f);
            Debug.DrawLine(edges.downleft, edges.upleft, Color.red, 100.0f);
        }
    }
}
