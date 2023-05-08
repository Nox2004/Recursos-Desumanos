using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaperManager : MonoBehaviour
{
    //Static stuff
    public static PaperManager DocManager { get; private set; }

    //List of all the documents in scene
    public List<Paper> document_list = new List<Paper>();

    public Camera cam; //Game camera, obviously
    private int holding = -1; //Index of the document being selected. -1 if null

    //Puts a document above the others
    private void put_on_top(Paper doc)
    {
        var index = document_list.IndexOf(doc); //Gets the document index

        if (index == -1) return; //Returns if the document is not in the list
        
        //Every document in the list goes one position up, and the document on top goes to index 0
        for (int i = index; i > 0; i--)
        {
            document_list[i] = document_list[i - 1];
        }
        document_list[0] = doc;
    }

    public void get_document(Paper doc)
    {
        var index = document_list.IndexOf(doc); //Gets the document index

        if (index == -1) return; //Returns if the document is not in the list

        //Puts the document above the others and sets up the "holding" property
        put_on_top(doc);
        holding = index;
    }

    public void drop_document()
    {
        //Resets "holding"
        holding = -1;
    }

    void Awake()
    {
        DocManager = this;
    }
    
    void Update()
    {
        //Sets up mouse position in the world view
        var mouse_pos = InGameCursor.get_position_in_world();

        //Sets up a "selected" variable
        Paper selected = null;

        for (int i = 0; i < document_list.Count; i++)
        {
            //Sets the document layer order
            document_list[i].order = -i*3; //*3 because the childs and shadow of the documents will have separated orders

            //Selects the first (on top) document being overlaped with the mouse
            document_list[i].selected = false;
            if ((document_list[i].GetComponent<Collider2D>().OverlapPoint(new Vector2(mouse_pos.x, mouse_pos.y))) && (selected == null) && (holding == -1))
            {
                selected = document_list[i];
            }
        }

        //Informs the selected document that he was selected
        if (selected != null) selected.selected = true;
    }
}
