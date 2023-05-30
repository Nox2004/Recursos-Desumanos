using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuBackground : MonoBehaviour
{
    SpriteRenderer sprite_renderer;
    Material material;

    // Start is called before the first frame update
    void Start()
    {
        sprite_renderer = GetComponent<SpriteRenderer>();
        material = sprite_renderer.material;
    }

    // Update is called once per frame
    void Update()
    {
        material.SetFloat("left_offset",-0.3f);
        material.SetFloat("right_offset",0.3f);
    }
}
