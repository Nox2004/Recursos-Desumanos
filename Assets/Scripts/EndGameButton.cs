using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndGameButton : MonoBehaviour
{
    private ImageButton button;
    private Image image;

    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (button.cursor_over(image))
        {
            if (InGameCursor.get_button_down(0))
            {
                Singleton.Instance.reset();
                Singleton.Instance.go_to_menu();
            }
        }
    }
}
