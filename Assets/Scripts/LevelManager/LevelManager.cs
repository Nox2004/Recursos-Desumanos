using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    //People list
    public int person_index = 0;
    public Person current_person;
    [SerializeField] private Person[] list;

    //Prefabs
    public GameObject dialogue_prefab, option_prefab, person_prefab, resume_prefab, document_prefab;

    //Instances that are currently in screen
    public PersonObject current_person_obj = null;
    public Dialogue current_dialogue = null;
    public Option[] current_options = null;

    //State machine stuff
    IState currentState;
    
    public void ChangeState(IState newState)
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
        //ChangeState(PersonEntering); //Enters the initial state
    }

    void Update()
    {
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
        //Creates person from prefab
        //Instantiate(me.)
    }

    public void UpdateState()
    {
        
    }

    public void ExitState()
    {
        
    }
}