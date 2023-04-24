using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LevelManager : MonoBehaviour
{
    public GameObject canvas;

    //People list
    public int person_index = 0;
    
    //Question stuff
    public int options_per_question;
    public int personal_options_per_question;
    public int number_of_questions;

    public DialogueStruc next_question_dialogue;

    public Person current_person;

    [SerializeField] private Person[] people_list;
    public int people_in_list;

    //Prefabs
    public GameObject dialogue_prefab, option_prefab, person_prefab, resume_prefab, document_prefab;

    //Instances that are currently in screen
    public PersonInRoom current_person_obj = null;
    public Dialogue current_dialogue = null;
    public Option current_options = null;

    //State machine stuff
    public IState currentState;
    public IState person_intro, initial_dialogue, make_questions, final_dialogue, interview_conclusion;// initial_dialogue = new InitialDialogue(this);

    private void Awake()
    {
        foreach (Person person in people_list)
        {
            person.dialogue_character = new DialogueCharacter(person.name, person.voice);
        } //Transformar isso em funcao de Person depois

        people_in_list = people_list.Length;
    }
    private void Start()
    {
        current_person = people_list[person_index];
        
        person_intro = new PersonIntroduction(this); initial_dialogue = new InitialDialogue(this); make_questions = new MakeQuestion(this); final_dialogue = new FinalDialogue(this); interview_conclusion = new InterviewConclusion(this);

        change_state(person_intro); //Enters the initial state
    }

    public void change_state(IState newState)
    {
        if (currentState != null)
        {
            currentState.ExitState();
        }
        currentState = newState;
        currentState.EnterState();
    }

    void Update()
    {
        current_person = people_list[person_index];

        if (currentState != null)
        {
            currentState.UpdateState();
        }
    }

    public Dialogue create_dialogue(DialogueStruc[] text)
    {
        var gameobj = Instantiate(dialogue_prefab);
        gameobj.transform.SetParent(canvas.transform, false);

        for (int i = 0; i < text.Length; i++)
        {
            if (text[i].character == null) text[i].character = current_person.dialogue_character;
        }

        current_dialogue = gameobj.GetComponent<Dialogue>();

        current_dialogue.text = text;
        current_dialogue.hide_at_end = false;
        current_dialogue.destroy_at_hide = false;

        return current_dialogue;
    }

    public void set_current_dialogue(DialogueStruc[] text)
    {
        for (int i = 0; i < text.Length; i++)
        {
            if (text[i].character == null) text[i].character = current_person.dialogue_character;
        }
        current_dialogue.set_text(text);
    }
    
    public Option create_options(OptionValues[] options)
    {
        var gameobj = Instantiate(option_prefab);
        gameobj.transform.SetParent(canvas.transform, false);

        current_options = gameobj.GetComponent<Option>();
        current_options.options = options;

        return current_options;
    }

    public ResumeUI create_resume(string name, int age, string id, string[] competences)
    {
        var gameobj = Instantiate(resume_prefab);
        gameobj.transform.SetParent(canvas.transform, false);

        ResumeUI resume = gameobj.GetComponent<ResumeUI>();

        resume.name = name;
        resume.age = age;
        resume.id = id;
        resume.competence_descs = competences;

        return resume;
    }

    public IdDocument spawn_id(string name, string id)
    {
        var gameobj = Instantiate(document_prefab,Singleton.Instance.instantiate_document_pos,Quaternion.identity);
        var doc = gameobj.GetComponent<IdDocument>();
        doc.name = name; doc.id = id;

        return doc;
    }

    public PersonInRoom create_person(Sprite sprite)
    {
        var gameobj = Instantiate(person_prefab);
        gameobj.GetComponent<SpriteRenderer>().sprite = sprite;

        current_person_obj = gameobj.GetComponent<PersonInRoom>();

        return current_person_obj;
    }

    public void destroy_option()
    {
        current_options.destroy = true;
        current_options = null;
    }

    public void destroy_dialogue()
    {
        current_dialogue.destroy_at_hide = true;
        current_dialogue = null;
    }

    public void destroy_person_obj()
    {
        Destroy(current_person_obj.gameObject);
        current_person_obj = null;
    }
    

    public void destroy_obj(GameObject obj)
    {
        Destroy(obj);
    }
}

public interface IState //State Interface
{
    void EnterState();
    void UpdateState();
    void ExitState();
}
