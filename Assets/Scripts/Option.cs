using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Option : MonoBehaviour
{
    //Image scale
    private float scale=1;

    //Serialized stuff (regarding the procedural animation)
    [SerializeField] private float scale_min, scale_max, smoothness, wave_speed, wave_size;
    private float time = 0;

    private Image uiImage; // Reference to the UI image

    private bool control = false;

    void Start()
    {
        //Get image component
        uiImage = gameObject.GetComponent<Image>();
        smoothness/=60;
    }

    void Update()
    {
        
    }
}
