using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum TransitionMode
{
    Scene,
    Exit
}

public class TransitPaper : MonoBehaviour
{
    public float spd;
}

public class Transition : MonoBehaviour
{
    //List of possible images and papers
    [SerializeField] private Sprite[] possible_images;
    private List<GameObject> papers;

    //spawn papers timer
    [SerializeField] private float spawn_time; //in frames
    private float spawn_timer = 0;

    //time until transition
    [SerializeField] private float transit_time; //in frames
    private float transit_timer = 0;

    //papers speed
    [SerializeField] private float min_papers_speed;
    [SerializeField] private float max_papers_speed;
    public AudioClip transit_sound;
    private AudioSource audio_source;
    
    private bool transit = false;

    public TransitionMode mode;
    public string target;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        //create audio source and play sound
        audio_source = gameObject.AddComponent<AudioSource>();
        audio_source.clip = transit_sound;
        audio_source.loop = false;
        audio_source.playOnAwake = false;
        audio_source.pitch = Random.Range(0.8f,1.1f);
        
        audio_source.Play();
        
        papers = new List<GameObject>();
        spawn_time/=60;
        transit_time/=60;
    }

    // Update is called once per frame
    void Update()
    {
        transit_timer += Time.deltaTime;

        if (transit_timer < transit_time)
        {
            //black_background.color = new Color(0,0,0,transit_timer / transit_time);

            spawn_timer += Time.deltaTime;
            
            if (spawn_timer > spawn_time)
            {
                spawn_timer = 0;

                var obj = new GameObject("Paper");
                obj.transform.SetParent(transform, false);

                RectTransform obj_rt = obj.AddComponent<RectTransform>();
                Image obj_img = obj.AddComponent<Image>();

                Sprite c_sprite = possible_images[(int)Random.Range(0,possible_images.Length)];
                obj_img.sprite = c_sprite;
                obj_rt.pivot = new Vector2(0.5f,1);
                obj_rt.sizeDelta = new Vector2(c_sprite.texture.width, c_sprite.texture.height); 

                var scale = Random.Range(0.9f,1.1f);
                obj_rt.localScale = new Vector3(scale, scale, scale);
                obj_rt.position = new Vector3(Random.Range(0,Screen.width),-Random.Range(0,500),0);
                obj.transform.SetSiblingIndex((int)Random.Range(0,transform.childCount)-1);

                //set obj_img color to random greyscale color with alpha 0.5
                var color = Random.Range(0.8f,1f);
                obj_img.color = new Color(color,color,color,1f);

                var obj_script = obj.AddComponent<TransitPaper>();
                obj_script.spd = Random.Range(min_papers_speed,max_papers_speed);

                papers.Add(obj);
                
                //spawn paper
            }
        }
        else 
        {
            if (!transit)
            {
                if (mode == TransitionMode.Scene) { SceneManager.LoadScene(target, LoadSceneMode.Single); }
                else { Application.Quit(); }
            }
            
            //transit and destroy
            transit = true;

            if (transform.childCount == 0)
            {
                Destroy(gameObject);
            }
        }

        
        for (int i = 0; i < papers.Count; i++)// (GameObject paper in papers)
        {
            GameObject paper = papers[i];

            paper.transform.position += Vector3.up * paper.GetComponent<TransitPaper>().spd * Time.deltaTime;
            if (paper.transform.position.y > Screen.height + 500)
            {
                Destroy(paper);
                papers.Remove(paper);
                i--;
            }
        }
    }
}
