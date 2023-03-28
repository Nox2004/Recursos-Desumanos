using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Perspective : MonoBehaviour
{
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

    //Gets sprite edge positions
    Edge SetupEdges(float width, float height, Vector2 pos, Vector2 pivot)
    {
        return new Edge(
        new Vector2(pos.x - width * pivot.x, pos.y + height * (1-pivot.y)),
        new Vector2(pos.x + width * (1-pivot.x), pos.y + height * (1-pivot.y)),
        new Vector2(pos.x - width * pivot.x, pos.y - height * pivot.y),
        new Vector2(pos.x + width * (1-pivot.x), pos.y - height * pivot.y));
    }

    Vector2 CalculateTan(Edge edges)
    {
        Vector2 persp_c = Singleton.Instance.perspective_point;
        return new Vector2((edges.upleft.x - persp_c.x) / Mathf.Abs(edges.upleft.y - persp_c.y),
        (edges.upright.x - persp_c.x) / Mathf.Abs(edges.upright.y - persp_c.y));
    }

    Vector2 CalculateOffset(float height, float lefttan, float righttan)
    {
        return new Vector2(height * lefttan,
        height * righttan);
    }

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

    private SpriteRenderer sprite_renderer;
    private Material material;

    private List<SpriteChild> sprite_childs = new List<SpriteChild>();
    private List<TextChild> text_childs = new List<TextChild>();
    
    private float tan_left, tan_right, sprite_width, sprite_height, xoffleft, xoffright;
    private Vector3 localpos;

    // Start is called before the first frame update
    void Start()
    {
        sprite_renderer = gameObject.GetComponent<SpriteRenderer>();
        localpos = transform.localPosition;

        foreach (Transform child in transform)
        {
            if (child.GetComponent<SpriteRenderer>() != null)
            {
                sprite_childs.Add(new SpriteChild(child, child.localPosition, child.gameObject.GetComponent<SpriteRenderer>()));
            }
            else//if (child.GetComponent<TextMesh>() != null)
            {
                text_childs.Add(new TextChild(child, child.localPosition, child.gameObject.GetComponent<MeshRenderer>().materials[0], child.gameObject.GetComponent<RectTransform>()));
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        material = sprite_renderer.material;
        Sprite sprite = sprite_renderer.sprite;

        Vector2 pos = new Vector2(transform.position.x, transform.position.y);
        sprite_height = sprite_renderer.bounds.size.y * transform.lossyScale.y;
        sprite_width = sprite_renderer.bounds.size.x * transform.lossyScale.x;

        Edge edges = SetupEdges(sprite_height, sprite_width, new Vector2(transform.position.x, transform.position.y), new Vector2(0.5f,0.5f));
        
        tan_left = CalculateTan(edges).x;
        xoffleft = CalculateOffset(sprite_height,tan_left,tan_right).x;
        material.SetFloat("left_offset", xoffleft);

        tan_right = CalculateTan(edges).y;
        xoffright = CalculateOffset(sprite_height,tan_left,tan_right).y;
        material.SetFloat("right_offset", xoffright);
        
        foreach (TextChild child in text_childs)
        {
            float _h = child.rect.sizeDelta.y * child.rect.lossyScale.y;

            float xoffl = CalculateOffset(_h,tan_left,tan_right).x;
            float xoffr = CalculateOffset(_h,tan_left,tan_right).y;

            float parent_height = sprite_height; 
            float parent_top = parent_height / 2;

            float mid = (tan_left + tan_right) / 2;

            float relative_x = child.localpos.x + (parent_height - (parent_top + child.localpos.y)) * mid;
            relative_x -= (xoffl + xoffr) / 4;

            child.transform.localPosition = new Vector3(relative_x, child.localpos.y, child.localpos.z);

            //Debug.Log(_h);
            //Debug.Log("left: " + xoffl + "       right: " + xoffr);
            child.material.SetFloat("left_offset", xoffl);
            child.material.SetFloat("right_offset", xoffr);
        }
        
        foreach (SpriteChild child in sprite_childs)
        {
            float _h = child.sprite_renderer.bounds.size.y * transform.lossyScale.y;
            Debug.Log(_h);
            float xoffl = CalculateOffset(_h,tan_left,tan_right).x;
            float xoffr = CalculateOffset(_h,tan_left,tan_right).y;

            float parent_height = sprite_height; // transform.lossyScale.y;// * transform.parent.lossyScale.y;
            float parent_top = parent_height / 2;

            float mid = (tan_left + tan_right) / 2;

            float relative_x = child.localpos.x + (parent_height - (parent_top + child.localpos.y)) * mid;
            relative_x -= (xoffl + xoffr) / 4;

            child.transform.localPosition = new Vector3(relative_x, child.localpos.y, child.localpos.z);

            child.sprite_renderer.material.SetFloat("right_offset", xoffr);
            child.sprite_renderer.material.SetFloat("left_offset", xoffl);
        }

        /*
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
