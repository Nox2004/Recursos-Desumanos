using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.Events;
//using TMPro.TextMeshPro;

[Serializable]
public struct SceneStruc
{
    [SerializeField] public Sprite sprite;
    [SerializeField] public string text;
}

public class Cutscene : MonoBehaviour
{
    [SerializeField] private UnityEvent ending_event;
    private bool ending = false;

    //Box and Text Objects
    private GameObject image_obj;
    private GameObject text_obj;

    //Image and text components
    private Image image;
    private TMPro.TextMeshProUGUI text;

    //Text border
    [SerializeField] private float text_border;
    [SerializeField] private TMPro.TMP_FontAsset font;
    [SerializeField] private int font_size;
    
    //Typewriter effect properties
    [SerializeField] private float letters_spd_min = 0.1f;
    [SerializeField] private float letters_spd_max = 0.5f;
    private float letters_spd;
    private float current_letters = 0;

    //Dialogue and index
    private int index = 0;
    public SceneStruc[] scenes;

    //Audio
    private float previous_letters = 0;
    [SerializeField] private AudioClip[] audio_clips;
    private List<AudioSource> audio_sources;// = new List<AudioSource>();

    private void Start()
    {

        //Multiplies time stuff by 60, so the speed represents the letters per frame (at 60fps)
        letters_spd_min*=60;
        letters_spd_max*=60;
        
        //text speed stuff
        letters_spd = letters_spd_min;

        #region //Creates UI objects

        RectTransform canvasRectTransform = GetComponentInParent<Canvas>().GetComponent<RectTransform>();

        //Creates UI image_obj object and adds Image and RectTransform components to it
        image_obj = new GameObject("Image");
        image_obj.transform.SetParent(transform);
        
        image = image_obj.AddComponent<Image>();
        Sprite sprite = scenes[0].sprite;
        image.sprite = sprite;

        var image_rt = image_obj.GetComponent<RectTransform>();

        //set image_rt position at the top of canvas
        image_rt.localScale = Vector3.one;
        image_rt.anchorMin = new Vector2(0.5f, 0.5f); image_rt.anchorMax = new Vector2(0.5f, 0.5f);
        image_rt.pivot = new Vector2(0.5f, 1f);
        
        image_rt.sizeDelta = new Vector2(sprite.texture.width,sprite.texture.height);
        image_rt.localPosition = new Vector3(0,canvasRectTransform.sizeDelta.y/2,0); //set image position at middle of canvas

        #endregion
        //create a image
        
        //Creates text
        
        text_obj = new GameObject("Text");
        text_obj.transform.SetParent(transform);//(image_obj.transform);

        text = text_obj.AddComponent<TMPro.TextMeshProUGUI>();
        text.font = font;
        text.fontSize = font_size;
        
        RectTransform text_rt = text_obj.GetComponent<RectTransform>();
        text_rt.anchorMin = new Vector2(0.5f, 0.5f); text_rt.anchorMax = new Vector2(0.5f, 0.5f);
        text_rt.pivot = new Vector2(0.5f,1f);
        text_rt.localScale = Vector3.one;
        text_rt.sizeDelta = new Vector2(canvasRectTransform.sizeDelta.x-text_border*2,(canvasRectTransform.sizeDelta.y - image.rectTransform.sizeDelta.y)-text_border*2);
        
        float text_y_position = (canvasRectTransform.sizeDelta.y/2)-image_rt.sizeDelta.y - text_border;
        text_rt.localPosition = new Vector3(0,text_y_position,0);

        text.alignment = TMPro.TextAlignmentOptions.Top;
        //set text position at the bottom of the canvas
        
        audio_sources = new List<AudioSource>();
    }

    private void Update()
    {
        //When button pressed
        if (InGameCursor.get_button_down(0))
        {
            //Changes spd if text is not over
            if (current_letters < scenes[index].text.Length)
            {
                letters_spd = letters_spd_max;
            }
            else 
            {
                //Changes text if dialogue is not over
                if (index < scenes.Length-1)
                {
                    index++;
                    letters_spd = letters_spd_min;
                    image.sprite = scenes[index].sprite;
                    current_letters = 0;
                }
                else //If is over then
                {
                    if (!ending)
                    {
                        ending_event.Invoke();
                        ending = true;
                    }
                }
            }
        }

        show_scene(scenes[index].sprite,scenes[index].text, letters_spd);
    }

    private void show_scene(Sprite sprite, string txt, float spd)
    {
        //Adds to the letter count
        current_letters += spd*Time.deltaTime;
        current_letters = Mathf.Min(current_letters,txt.Length); //Clamps value so it wont surpass the text size
        
        //Play sound if letter count changes
        if ((int) current_letters != (int) previous_letters)
        {
            var audio_index = (int) UnityEngine.Random.Range(0,audio_clips.Length);
            if (audio_clips[audio_index] != null)
            {
                AudioSource audio_source = gameObject.AddComponent<AudioSource>();
                audio_source.clip = audio_clips[audio_index];
                audio_source.Play();
                audio_source.pitch = UnityEngine.Random.Range(0.8f,1.1f);
                audio_sources.Add(audio_source);
            }
        }

        //Removes audio sources that are not playing anymore from list
        for (int i = 0; i < audio_sources.Count; i++)
        {
            if (!audio_sources[i].isPlaying)
            {
                Destroy(audio_sources[i]);
                audio_sources.RemoveAt(i);
                i--;
            }
        }

        previous_letters = current_letters;

        text.text = txt.Substring(0, (int) Mathf.Floor(current_letters)); //Gets current texts substring
    }

    public void go_to_menu()
    {
        Singleton.Instance.create_transition(TransitionMode.Scene, "Menu");
    }
}
