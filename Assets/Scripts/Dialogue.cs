using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System;
//using TMPro.TextMeshPro;

[Serializable]
public struct DialogueStruc
{
    [SerializeField] private string text;
    [SerializeField] private string name;
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

    //Box and text transform components
    private RectTransform box_rect;
    private RectTransform text_rect;
    
    //Image and text components
    private TMPro.TextMeshProUGUI dialogue_text;
    private Image dialogue_box;    

    //Text border
    [SerializeField] private float text_border;
    
    //Typewriter effect properties
    [SerializeField] private float letters_spd_min = 0.1f;
    [SerializeField] private float letters_spd_max = 0.5f;
    private float letters_spd;
    private float current_letters = 0;

    //Dialogue and index
    private int index = 0;
    public string[] text;
    //private string currentCharacterName;

    private void Start()
    {
        //Multiplies time stuff by 60, so the speed represents the letters per frame (at 60fps)
        letters_spd_min*=60;
        letters_spd_max*=60;

        letters_spd = letters_spd_min;

        //Sets up components
        box_rect = dialogue_box_obj.GetComponent<RectTransform>();
        text_rect = text_obj.GetComponent<RectTransform>();
        
        dialogue_text = text_obj.GetComponent<TMPro.TextMeshProUGUI>();
        dialogue_box = dialogue_box_obj.GetComponent<Image>();

        //Sets up procedural animation stuff
        box_width = 0;
        width_open_curve.val_start=box_width;               width_close_curve.val_end = width_open_curve.val_start;
        width_open_curve.val_end=box_rect.sizeDelta.x;      width_close_curve.val_start = width_open_curve.val_end;
        
        box_yy = box_rect.sizeDelta.y * 1.5f + 100;
        yy_enter_curve.val_start = box_yy;                                      yy_exit_curve.val_end = yy_enter_curve.val_start;
        yy_enter_curve.val_end = dialogue_box_obj.transform.localPosition.y;    yy_exit_curve.val_start = yy_enter_curve.val_end;
    }

    private void Update()
    {
        //When button pressed
        if (Input.GetMouseButtonDown(0))
        {
            //Changes spd if text is not over
            if (current_letters < text[index].Length)
            {
                letters_spd = letters_spd_max;
            }
            else 
            {
                //Changes text if dialogue is not over
                if (index < text.Length)
                {
                    index++;
                    letters_spd = letters_spd_min;
                    current_letters = 0;
                }
                else //If is over then
                {

                }
            }
        }

        ShowDialogue("aaaaa",text[index], letters_spd);
        

        //Changes box width
        box_rect.sizeDelta = new Vector2(box_width,box_rect.sizeDelta.y);
        text_rect.sizeDelta = new Vector2(box_width-text_border*2,box_rect.sizeDelta.y-text_border*2); //Applies text border

        //Changes box y
        dialogue_box_obj.transform.localPosition = new Vector3(dialogue_box_obj.transform.localPosition.x,box_yy,dialogue_box_obj.transform.localPosition.z);
    }

    private void ShowDialogue(string name, string txt, float spd)
    {
        //Adds to the letter count
        current_letters += spd*Time.deltaTime;
        current_letters = Mathf.Min(current_letters,txt.Length); //Clamps value so it wont surpass the text size

        dialogue_text.text = txt.Substring(0, (int) Mathf.Floor(current_letters)); //Gets current texts substring

        box_width = width_open_curve.Update(Time.deltaTime);
        box_yy = yy_enter_curve.Update(Time.deltaTime);

        /*
        Stuff chatGPT sugested me to do

        currentCharacterName = characterName;
        currentDialogueText = _dialogueText;
        characterNameText.text = currentCharacterName;
        dialogueText.text = "";
        dialogueBox.SetActive(true); // Show the dialogue box
        gameObject.SetActive(true); // Show the whole dialogue UI

        // Start coroutine to display dialogue text letter by letter
        if (displayCoroutine != null)
        {
            StopCoroutine(displayCoroutine);
        }
        displayCoroutine = StartCoroutine(DisplayDialogueText());*/
    }

    private void HideDialogue()
    {
        box_width = width_close_curve.Update(Time.deltaTime);
        box_yy = yy_exit_curve.Update(Time.deltaTime);
        // Stop coroutine if it's running
        //if (displayCoroutine != null)
        //{
        //    StopCoroutine(displayCoroutine);
        //}

        // Hide dialogue UI
        //dialogueBox.SetActive(false); // Hide the dialogue box
        //gameObject.SetActive(false); // Hide the whole dialogue UI
    }
}