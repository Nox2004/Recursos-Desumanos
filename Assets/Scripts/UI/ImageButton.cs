using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageButton
{
    public bool cursor_over(Image image)
    {
        Vector2 localMousePos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(image.rectTransform, InGameCursor.get_position_in_screen(), null, out localMousePos);
        if (image.rectTransform.rect.Contains(localMousePos)) return true;

        return false;
    }
    //Discovers mouse position in canvas
    
    public ImageButton()
    {

    }
}