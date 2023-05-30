using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drawer : MonoBehaviour
{
    public bool locked;

    //Sound
    [SerializeField] protected AudioClip open_sound;
    [SerializeField] protected AudioClip close_sound;
    protected AudioSource audio_source;
    protected bool is_opening_last_frame = false;

    [SerializeField] private float yy_start;
    [SerializeField] private float yy_end;
    [SerializeField] private float yy;
    [SerializeField] private float smoothness;

    public Vector3 documents_offset;
    public Vector3 documents_spacing;
    public int document_number = 0;

    public List<Person> people_hired;

    [SerializeField] private Vector3 initial_pos;
    public Jobs job;

    private Collider2D collider;

    // Start is called before the first frame update
    void Start()
    {
        is_opening_last_frame = false;
        
        audio_source = gameObject.AddComponent<AudioSource>();

        locked = true;
        people_hired = new List<Person>();

        collider = gameObject.GetComponent<Collider2D>();
        yy = yy_start;
        initial_pos = transform.position;

        smoothness/=60;
    }

    // Update is called once per frame
    void Update()
    {
        if (Singleton.Instance.game_paused) return;
        
        bool is_opening = collider.OverlapPoint(InGameCursor.get_position_in_world()) && !locked;

        //Plays sound
        if (is_opening && !is_opening_last_frame)
        {
            audio_source.pitch = Random.Range(0.9f, 1.1f);
            audio_source.PlayOneShot(open_sound);
        }
        else if (!is_opening && is_opening_last_frame)
        {
            audio_source.pitch = Random.Range(0.9f, 1.1f);
            audio_source.PlayOneShot(close_sound);
        }

        is_opening_last_frame = is_opening;
        
        //Changes yy
        float y_target;

        if (is_opening) y_target = yy_end;
        else y_target = yy_start;

        yy += ((y_target-yy) / smoothness) * Time.deltaTime;

        transform.position = initial_pos + Vector3.down * yy;
        collider.offset = Vector3.up * (yy - 2);
    }
}

