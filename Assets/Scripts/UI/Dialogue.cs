using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
//using TMPro.TextMeshPro;

[Serializable]
public struct DialogueStruc
{
    [TextArea(2,20)]
    [SerializeField] public string text;
    [SerializeField] public DialogueCharacter character;

    public DialogueStruc(string txt, DialogueCharacter this_char)
    {
        text = txt;
        character = this_char;
    }
}

public class Dialogue : MonoBehaviour
{
    private float box_yy, box_width;
    //Open and close box curvesX
    [SerializeField] private AnimCurveValue width_open_curve;
    [SerializeField] private AnimCurveValue width_close_curve;
    //Y movement box curves
    [SerializeField] private AnimCurveValue yy_enter_curve;
    [SerializeField] private AnimCurveValue yy_exit_curve;

    //Box and Text Objects
    [SerializeField] private GameObject dialogue_box_obj;
    [SerializeField] private GameObject text_obj;
    [SerializeField] private GameObject name_obj;

    //Box and text transform components
    private RectTransform box_rect;
    private RectTransform text_rect;
    private RectTransform name_text_rect;
    
    //Image and text components
    private TMPro.TextMeshProUGUI dialogue_text;
    private TMPro.TextMeshProUGUI name_text;
    private Image dialogue_box;    

    //Text border
    [SerializeField] private float text_border;
    
    //Typewriter effect properties
    [SerializeField] private float letters_spd_min = 0.1f;
    [SerializeField] private float letters_spd_max = 0.5f;
    private float letters_spd;
    private float current_letters = 0;

    //Audio
    private float previous_letters = 0;
    [SerializeField] private AudioClip[] audio_clips;
    private List<AudioSource> audio_sources;

    //Dialogue and index
    private int index = 0;
    public DialogueStruc[] text;

    public bool hide_at_end = true;
    public bool destroy_at_hide = true;

    private bool entering = true;

    private void Start()
    {
        //Multiplies time stuff by 60, so the speed represents the letters per frame (at 60fps)
        letters_spd_min*=60;
        letters_spd_max*=60;

        letters_spd = letters_spd_min;

        //Sets up audio source list
        audio_sources = new List<AudioSource>();

        //Sets up components
        box_rect = dialogue_box_obj.GetComponent<RectTransform>();
        text_rect = text_obj.GetComponent<RectTransform>();
        name_text_rect = name_obj.GetComponent<RectTransform>();
        
        dialogue_text = text_obj.GetComponent<TMPro.TextMeshProUGUI>();
        name_text = name_obj.GetComponent<TMPro.TextMeshProUGUI>();
        dialogue_box = dialogue_box_obj.GetComponent<Image>();

        //Sets up procedural animation stuff
        box_width = 0;
        width_open_curve.val_start=box_width;               width_close_curve.val_end = width_open_curve.val_start;
        width_open_curve.val_end=box_rect.sizeDelta.x;      width_close_curve.val_start = width_open_curve.val_end;
        
        box_yy = box_rect.sizeDelta.y * 1.5f + 100;
        yy_enter_curve.val_start = box_yy;                                      yy_exit_curve.val_end = yy_enter_curve.val_start;
        yy_enter_curve.val_end = dialogue_box_obj.transform.localPosition.y;    yy_exit_curve.val_start = yy_enter_curve.val_end;
        
        dialogue_box_obj.transform.localPosition = new Vector3(dialogue_box_obj.transform.localPosition.x,box_yy,dialogue_box_obj.transform.localPosition.z);
    }

    private void Update()
    {
        if (Singleton.Instance.game_paused) return;
        
        //var entering = true;

        //When button pressed
        if (InGameCursor.get_button_down(0))
        {
            //Changes spd if text is not over
            if (current_letters < text[index].text.Length)
            {
                letters_spd = letters_spd_max;
            }
            else 
            {
                letters_spd = letters_spd_min;
                
                //Changes text if dialogue is not over
                if (index < text.Length-1)
                {
                    index++;
                    current_letters = 0;
                }
                else //If is over then
                {
                    if (hide_at_end) entering = false;
                }
            }
        }

        show_dialogue(text[index].character.name,text[index].text, text[index].character.speech_color, letters_spd);
        
        if (current_letters < text[index].text.Length) //If text is not over
        {
            if (text[index].character.person_in_room != null)
            {
                //animate character
                text[index].character.person_in_room.talking = true;
            }
            if ((text[index].character.voice != null) && (text[index].character.voice.Length > 0))
            {
                //If new letter is added, play a random voice clip
                if ((int) current_letters != (int) previous_letters)
                {
                    //choose random clip
                    var list_of_clips = text[index].character.voice;
                    var audio_index = (int) UnityEngine.Random.Range(0,list_of_clips.Length);

                    if (audio_clips[audio_index] != null)
                    {
                        //Adds audio source
                        AudioSource audio_source = gameObject.AddComponent<AudioSource>();
                        audio_source.clip = list_of_clips[audio_index];
                        audio_source.Play();
                        audio_source.pitch = UnityEngine.Random.Range(0.8f,1.1f);
                        audio_sources.Add(audio_source);
                    }
                }
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

        //Animation stuff
        if (entering) enter();
        else hide();

        //Changes box width
        box_rect.sizeDelta = new Vector2(box_width,box_rect.sizeDelta.y);
        name_text_rect.sizeDelta = new Vector2(box_width-text_border*2,name_text_rect.sizeDelta.y);
        text_rect.sizeDelta = new Vector2(box_width-text_border*2,text_rect.sizeDelta.y); //Applies text border

        //Changes box y
        dialogue_box_obj.transform.localPosition = new Vector3(dialogue_box_obj.transform.localPosition.x,box_yy,dialogue_box_obj.transform.localPosition.z);

        if (hiding_box() && destroy_at_hide)
        {
            Destroy(gameObject);
        }
    }

    public bool finished_text()
    {
        return (current_letters >= text[index].text.Length) && (index >= text.Length-1);
    }

    public bool hiding_box()
    {
        return width_close_curve.GetRawValue() >= 1;
    }

    public void set_text(DialogueStruc[] new_text)
    {
        index=0;
        current_letters=0;
        text = new_text;
    }

    private void show_dialogue(string name, string txt, Color color, float spd)
    {
        //Adds to the letter count
        current_letters += spd*Time.deltaTime;
        current_letters = Mathf.Min(current_letters,txt.Length); //Clamps value so it wont surpass the text size

        dialogue_text.text = txt.Substring(0, (int) Mathf.Floor(current_letters)); //Gets current texts substring
        name_text.color = color;
        name_text.text = name;
    }

    private void enter()
    {
        box_width = width_open_curve.Update(Time.deltaTime);
        box_yy = yy_enter_curve.Update(Time.deltaTime);
    }

    private void hide()
    {
        box_width = width_close_curve.Update(Time.deltaTime);
        box_yy = yy_exit_curve.Update(Time.deltaTime);
    }
}
