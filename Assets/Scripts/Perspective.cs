using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Perspective : MonoBehaviour
{
    //Defines a edge struct, so I won't have to create a whole lot of variables later on
    struct Edge
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

    //Gets sprite edge positions and returns a struct with all of them
    Edge SetupEdges(float width, float height, Vector2 pos, Vector2 pivot)
    {
        return new Edge(
        new Vector2(pos.x - width * pivot.x, pos.y + height * (1-pivot.y)),
        new Vector2(pos.x + width * (1-pivot.x), pos.y + height * (1-pivot.y)),
        new Vector2(pos.x - width * pivot.x, pos.y - height * pivot.y),
        new Vector2(pos.x + width * (1-pivot.x), pos.y - height * pivot.y));
    }

    //Calculates the tan of the upper corners and returns them as a Vector 2
    Vector2 CalculateTan(Edge edges)
    {
        Vector2 persp_c = Singleton.Instance.perspective_point;
        return new Vector2((edges.upleft.x - persp_c.x) / Mathf.Abs(edges.upleft.y - persp_c.y),
        (edges.upright.x - persp_c.x) / Mathf.Abs(edges.upright.y - persp_c.y));
    }

    //Uses the tan to calculate the expected x offset of the bottom corners
    Vector2 CalculateOffset(float height, float lefttan, float righttan)
    {
        return new Vector2(height * lefttan,
        height * righttan);
    }

    //Stuff I will need to use from the sprite childs
    struct SpriteChild 
    {
        public Transform transform;
        public Vector3 localpos;
        public SpriteRenderer sprite_renderer;

        public SpriteChild(Transform trans, Vector3 pos, SpriteRenderer sprite)
        {
            transform = trans; localpos = pos; sprite_renderer = sprite;
        }
    }
    
    //Stuff I will need to use from the text childs
    struct TextChild 
    {
        public Transform transform;
        public Vector3 localpos;
        public Material material;
        public RectTransform rect;
        
        public TextChild(Transform trans, Vector3 pos, Material mat, RectTransform rec)
        {
            transform = trans; localpos = pos; material = mat; rect = rec;
        }
    }
    
    //Change polygon collider paths
    void adapt_collider(PolygonCollider2D collider, Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4)
    {
        collider.pathCount = 1;
        collider.SetPath(0,new[] { p1, p2, p3, p4});
        //collider.points = new[] { p1, p2, p3, p4};
    }

    //Calculates the middle of the documents and change the cursor position
    public void set_mouse_at_middle()
    {
        //InGameCursor.cursor_pos = Singleton.Instance.cam.WorldToScreenPoint(transform.localPosition + new Vector3((xoffleft+xoffright)/2/2,0,0));
    }

    //Components
    private SpriteRenderer sprite_renderer;
    private Material material;
    private PolygonCollider2D polygon_col;

    //List of childs
    private List<SpriteChild> sprite_childs = new List<SpriteChild>();
    private List<TextChild> text_childs = new List<TextChild>();
    
    //Tan and sprite size
    private float tan_left, tan_right, sprite_width, sprite_height;

    //Offset and initial position
    public float xoffleft, xoffright; 
    private Vector3 localpos;

    //offset for the collision
    public Vector2 collider_offset = Vector3.zero;

    void Start()
    {
        //Sets up initial position and components
        polygon_col = gameObject.GetComponent<PolygonCollider2D>();
        sprite_renderer = gameObject.GetComponent<SpriteRenderer>();
        localpos = transform.localPosition;

        //Store information about the childs in 2 lists (one for sprites, another one for texts)
        foreach (Transform child in transform)
        {
            if (child.gameObject.tag == "NoPerspective") continue; //Ignores childs with the No Perspective tag

            if (child.GetComponent<SpriteRenderer>() != null)
            {
                sprite_childs.Add(new SpriteChild(child, child.localPosition, child.gameObject.GetComponent<SpriteRenderer>()));
            }
            else //if (child.GetComponent<TextMesh>() != null)
            {
                text_childs.Add(new TextChild(child, child.localPosition, child.gameObject.GetComponent<MeshRenderer>().materials[0], child.gameObject.GetComponent<RectTransform>()));
            }
        }
    }

    void Update()
    {
        //Sets up material and sprite variables
        material = sprite_renderer.material;
        Sprite sprite = sprite_renderer.sprite;

        //Gets position and sprite size
        Vector2 pos = new Vector2(transform.position.x, transform.position.y);
        sprite_height = sprite_renderer.bounds.size.y * transform.lossyScale.y;
        sprite_width = sprite_renderer.bounds.size.x * transform.lossyScale.x;
        
        //Store sprites edges position
        Edge edges = SetupEdges(sprite_width, sprite_height, new Vector2(transform.position.x, transform.position.y), new Vector2(0.5f,0.5f));
        
        //Calculates tan
        tan_left = CalculateTan(edges).x;
        tan_right = CalculateTan(edges).y;

        //Calculates offset
        xoffleft = CalculateOffset(sprite_height,tan_left,tan_right).x;
        xoffright = CalculateOffset(sprite_height,tan_left,tan_right).y;

        //Applies offset to the perspective shader
        material.SetFloat("left_offset", xoffleft);
        material.SetFloat("right_offset", xoffright);
        
        //Applies perspective to the object text childs
        foreach (TextChild child in text_childs)
        {
            float _h = child.rect.sizeDelta.y * child.rect.lossyScale.y; //Child height

            //Uses parent tangent to calculate offset
            float xoffl = CalculateOffset(_h,tan_left,tan_right).x;
            float xoffr = CalculateOffset(_h,tan_left,tan_right).y;

            //Calculates parents top and the middle between left and right corners
            float parent_height = sprite_height; 
            float parent_top = parent_height / 2;
            float mid = (tan_left + tan_right) / 2;

            //Calculates relative x so the further the object is from the top, the more it moves horizontally
            float distance_from_top = parent_height - (parent_top + child.localpos.y);
            float relative_x = child.localpos.x + distance_from_top * mid;
            relative_x -= (xoffl + xoffr) / 4;

            //Apllies offset
            child.transform.localPosition = new Vector3(relative_x, child.localpos.y, child.localpos.z); 
            child.material.SetFloat("left_offset", xoffl);
            child.material.SetFloat("right_offset", xoffr);
            child.material.SetFloat("my_height", _h);
        }

        //Applies perspective to the object sprite childs
        foreach (SpriteChild child in sprite_childs)
        {
            float _h = child.sprite_renderer.bounds.size.y * transform.lossyScale.y; //Child height

            //Uses parent tangent to calculate offset
            float xoffl = CalculateOffset(_h,tan_left,tan_right).x;
            float xoffr = CalculateOffset(_h,tan_left,tan_right).y;

            //Calculates parents top and the middle between left and right corners
            float parent_height = sprite_height; // transform.lossyScale.y;// * transform.parent.lossyScale.y;
            float parent_top = parent_height / 2;
            float mid = (tan_left + tan_right) / 2;

            //Calculates relative x so the further the object is from the top, the more it moves horizontally
            float distance_from_top = parent_height - (parent_top + child.localpos.y);
            float relative_x = child.localpos.x + distance_from_top * mid;
            relative_x -= (xoffl + xoffr) / 4;

            //Apllies offset
            child.transform.localPosition = new Vector3(relative_x, child.localpos.y, child.localpos.z);
            child.sprite_renderer.material.SetFloat("right_offset", xoffr);
            child.sprite_renderer.material.SetFloat("left_offset", xoffl);
        }

        //Changes edge positions in the edge data structure
        edges.downleft.x += xoffleft;
        edges.downright.x += xoffright;

        //Adapt the collyder to fit the documents sprite
        if (polygon_col != null)
        {
            Vector2 _pos = new Vector2(transform.position.x, transform.position.y);

            adapt_collider(polygon_col, 
            edges.upleft - _pos + collider_offset,
            edges.upright - _pos + collider_offset,
            edges.downright - _pos + collider_offset,
            edges.downleft - _pos + collider_offset);
        }

        /*
        Rascunho aleatorio que eu vou deixar aqui pro precaucao
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
        }*/

        /*edges.downleft = new Vector2(edges.upleft.x + xoffleft, edges.downleft.y);
        edges.downright = new Vector2(edges.downright.x + xoffright, edges.downright.y); 

        if (Input.GetKeyDown(KeyCode.D))
        {
            Debug.DrawLine(edges.upleft, edges.upright, Color.red, 100.0f);
            Debug.DrawLine(edges.upright, edges.downright, Color.red, 100.0f);
            Debug.DrawLine(edges.downright, edges.downleft, Color.red, 100.0f);
            Debug.DrawLine(edges.downleft, edges.upleft, Color.red, 100.0f);
        }*/
    }
}
