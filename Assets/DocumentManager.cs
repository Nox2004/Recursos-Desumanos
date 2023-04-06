using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DocumentManager : MonoBehaviour
{
    public static DocumentManager DocManager { get; private set; }

    public List<Document> document_list = new List<Document>();

    public Camera cam;

    private int holding = -1;

    private void put_on_top(Document doc)
    {
        var index = document_list.IndexOf(doc);
        for (int i = index; i > 0; i--)
        {
            document_list[i] = document_list[i - 1];
        }
        document_list[0] = doc;
    }

    public void get_document(Document doc)
    {
        var index = document_list.IndexOf(doc);

        if (index == null) return;

        put_on_top(doc);
        holding = index;
    }

    public void drop_document()
    {
        holding = -1;
    }

    // Start is called before the first frame update
    void Awake()
    {
        DocManager = this;
    }

    // Update is called once per frame
    void Update()
    {
        var mouse_pos = Input.mousePosition;
        mouse_pos = cam.ScreenToWorldPoint(mouse_pos);

        Document selected = null;

        for (int i = 0; i < document_list.Count; i++)
        {
            document_list[i].order = -i*3;

            document_list[i].selected = false;
            if ((document_list[i].my_collider.OverlapPoint(new Vector2(mouse_pos.x, mouse_pos.y))) && (selected == null) && (holding == -1))
            {
                selected = document_list[i];
            }
        }

        if (selected != null) selected.selected = true;
    }
}
