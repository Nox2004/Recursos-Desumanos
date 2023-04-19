using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    
    public void change_state(IState newState)
    {
        if (currentState != null)
        {
            currentState.ExitState();
        }
        currentState = newState;
        currentState.EnterState();
    }

    private void Start()
    {
        person_entering = new PersonEntering(this); initial_dialogue = new InitialDialogue(this);

        change_state(person_entering); //Enters the initial state
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

public class PersonEntering : IState
{
    private LevelManager me;

    public PersonEntering(LevelManager level_manager)
    {
        this.me = level_manager;
    }

    public void EnterState()
    {
        me.create_person(me.current_person.sprite);
    }

    public void UpdateState()
    {
        if (me.current_person_obj.HasEntered())
        {
            me.change_state(me.initial_dialogue);
        }
    }

    public void ExitState()
    {
        
    }
}

public class InitialDialogue : IState
{
    private LevelManager me;

    public InitialDialogue(LevelManager level_manager)
    {
        this.me = level_manager;
    }

    public void EnterState()
    {
        me.create_dialogue(me.current_person.initial_dialogue);
    }

    public void UpdateState()
    {
        //if (me.current_person_obj.HasEntered())
        //{
        //    me.change_state(me.initial_dialogue);
        //}
    }

    public void ExitState()
    {
        
    }
}