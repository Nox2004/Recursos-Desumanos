using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DocumentManager : MonoBehaviour
{
    public static DocumentManager DocManager { get; private set; }

    public List<Document> document_list = new List<Document>();

    public void put_on_top(Document doc)
    {
        var index = document_list.IndexOf(doc);
        for (int i = index; i > 0; i--)
        {
            document_list[i] = document_list[i - 1];
        }
        document_list[0] = doc;
    }

    // Start is called before the first frame update
    void Awake()
    {
        DocManager = this;
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < document_list.Count; i++)
        {
            document_list[i].order = -i*3;
        }
    }
}
