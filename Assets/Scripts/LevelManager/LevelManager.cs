using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LevelManager : MonoBehaviour
{
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

    //State machine stuff
    public IState currentState;
    public IState person_entering, initial_dialogue;// initial_dialogue = new InitialDialogue(this);

    private void Start()
    {
        person_entering = new PersonEntering(this); initial_dialogue = new InitialDialogue(this);

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
        current_dialogue = gameobj.GetComponent<Dialogue>();

        current_dialogue.text = text;

        return current_dialogue;
    }
    
    public Option create_options(OptionStruc[] options)
    {
        var gameobj = Instantiate(option_prefab);
        current_options = gameobj.GetComponent<Option>();

        current_options.options = options;

        return current_options;
    }

    public PersonInRoom create_person(Sprite sprite)
    {
        var gameobj = Instantiate(person_prefab);
        current_person_obj = gameobj.GetComponent<PersonInRoom>();

        return current_person_obj;
    }
}

public interface IState //State Interface
{
    void EnterState();
    void UpdateState();
    void ExitState();
}