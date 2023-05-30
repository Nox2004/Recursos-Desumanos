using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialFolder : Paper
{
    [SerializeField] private GameObject ui_prefab;
    [SerializeField] private GameObject canvas;
    public GameObject current_ui = null;

    protected void Start()
    {
        base.Start();   
    }
    
    protected void Update()
    {
        base.Update();
        
        if (mouse_over() && InGameCursor.get_button_down(0) && current_ui==null)
        {
            //create UI form
            current_ui = Instantiate(ui_prefab, transform.parent);
            current_ui.GetComponent<TutorialFolderUI>().folder_in_room = this;
            //set ui parent to be the canvas
            current_ui.transform.SetParent(canvas.transform, false);
        }
    }
}
