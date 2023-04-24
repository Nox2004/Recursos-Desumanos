using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdDocument : MonoBehaviour
{
    [SerializeField] private GameObject name_obj;
    [SerializeField] private GameObject id_obj;

    [SerializeField] private TMPro.TMP_Text name_text;
    [SerializeField] private TMPro.TMP_Text id_text;

    public string name;
    public string id;

    // Start is called before the first frame update
    void Start()
    {
        name_text = name_obj.GetComponent<TMPro.TMP_Text>();
        id_text = id_obj.GetComponent<TMPro.TMP_Text>();

        name_text.text = name;
        id_text.text = id;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
