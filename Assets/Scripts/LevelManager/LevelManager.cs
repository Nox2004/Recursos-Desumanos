using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LevelManager : MonoBehaviour
{
    public GameObject canvas;

    //People list
    public int person_index = 0;
    public Person current_person;

    [SerializeField] private Person[] people_list;

    //Prefabs
    public GameObject dialogue_prefab, option_prefab, person_prefab, resume_prefab, document_prefab;

    //Instances that are currently in screen
    public PersonInRoom current_person_obj = null;
    public Dialogue current_dialogue = null;
    public Option current_options = null;

    public int number_of_options;

    //State machine stuff
    public IState currentState;
    public IState person_entering, initial_dialogue, make_questions;// initial_dialogue = new InitialDialogue(this);

    private void Start()
    {
        current_person = people_list[person_index];
        
        person_entering = new PersonEntering(this); initial_dialogue = new InitialDialogue(this); make_questions = new MakeQuestion(this);

        change_state(person_entering); //Enters the initial state
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

        current_dialogue = gameobj.GetComponent<Dialogue>();

        current_dialogue.text = text;

        return current_dialogue;
    }
    
    public Option create_options(OptionStruc[] options)
    {
        var gameobj = Instantiate(option_prefab);
        gameobj.transform.SetParent(canvas.transform, false);

        current_options = gameobj.GetComponent<Option>();
        current_options.options = options;

        return current_options;
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
}

public interface IState //State Interface
{
    void EnterState();
    void UpdateState();
    void ExitState();
}