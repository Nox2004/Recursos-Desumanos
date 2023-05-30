using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuLogo : MonoBehaviour
{
    [SerializeField] private float angle_spd;
    [SerializeField] private float angle_variation;
    [SerializeField] private float y_variation;

    private float initial_y;
    private Vector3 initial_scale;

    // Start is called before the first frame update
    void Start()
    {
        initial_y = transform.position.y;
        initial_scale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        transform.localRotation = Quaternion.Euler(0,0,Mathf.Sin(Time.realtimeSinceStartup/(angle_spd/60)) * (angle_variation));

        var scale = 1 + (Mathf.Sin(Time.realtimeSinceStartup/((angle_spd/2)/60)) * (y_variation));
        transform.localScale = initial_scale * scale;
    }
}
