using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LevelManager : MonoBehaviour
{
    public GameObject future_points_ui;

    public GameObject canvas;
    public RoomCamera camera;

    public DialogueStruc next_question_dialogue;
    
    //People list
    [HideInInspector] public int person_index = 0;
    [HideInInspector] public Person current_person;


    //Interview stuff
    [Header("Interview")]
    [Space(10)]

    //Number of interviews
    public int number_of_interviews;
    public int hire_limit;

    //Question settings
    public int options_per_question;
    public int number_of_questions;
    
    [SerializeField] private List<Person> mandatory_people_list;

    //Final people list
    [HideInInspector] private Person[] people_list;
    [HideInInspector] public int people_in_list;

    //Prefabs
    [Header("Prefabs")]
    [Space(10)]

    public GameObject dialogue_prefab;
    public GameObject option_prefab, person_prefab, resume_prefab, document_prefab, hire_count_prefab;

    //Instances that are currently in screen
    [HideInInspector] public PersonInRoom current_person_obj = null;
    [HideInInspector] public Dialogue current_dialogue = null;
    [HideInInspector] public Option current_options = null;

    //State machine stuff
    [HideInInspector] public IState currentState;
    [HideInInspector] public IState pre_interview,
                                    person_intro, 
                                    initial_dialogue, 
                                    make_questions, 
                                    interview_conclusion, 
                                    post_interview;

    private void Awake()
    {
        var day_count = Instantiate(Singleton.Instance.day_count_prefab);
        day_count.GetComponent<DayCount>().day = Singleton.Instance.current_day;

        people_list = new Person[number_of_interviews];

        while (mandatory_people_list.Count > 0)
        {
            //generate random index from people_list
            int random_index = UnityEngine.Random.Range(0, people_list.Length);
            
            if (people_list[random_index] == null)
            {
                people_list[random_index] = mandatory_people_list[0];
                mandatory_people_list.RemoveAt(0);
            }
        }
        
        for (int i = 0; i < number_of_interviews; i++)
        {
            if (people_list[i] != null) continue;

            int random_index = UnityEngine.Random.Range(0, Singleton.Instance.random_people.Count);

            //Debug.Log(random_index);
            people_list[i] = Singleton.Instance.random_people[random_index].create_person();

            Singleton.Instance.random_people.RemoveAt(random_index);
        }

        people_in_list = people_list.Length;

        future_points_ui = Instantiate(Singleton.Instance.future_points_prefab);
        future_points_ui.transform.SetParent(canvas.transform, false);
        future_points_ui.transform.localPosition = Vector3.zero;
    }
    private void Start()
    {
        set_current_person();
        
        person_intro = new PersonIntroduction(this); initial_dialogue = new InitialDialogue(this); make_questions = new MakeQuestion(this); interview_conclusion = new InterviewConclusion(this); post_interview = new PostInterview(this);

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
        if (Singleton.Instance.game_paused) return;

        current_person = people_list[person_index];

        if (currentState != null)
        {
            currentState.UpdateState();
        }
    }

    public void set_current_person(int ind = -1)
    {
        current_person = people_list[(ind == -1) ? person_index : ind];
    }

    public Dialogue create_dialogue(DialogueStruc[] text)
    {
        var gameobj = Instantiate(dialogue_prefab);
        gameobj.transform.SetParent(canvas.transform, false);

        for (int i = 0; i < text.Length; i++)
        {
            if (text[i].character == null) 
            {
                text[i].character = current_person.dialogue_character;
            }
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

    public ResumeUILayout create_resume(GameObject prefab, TMPro.TMP_FontAsset font , string name, int age, string id, string[] competences)
    {
        var gameobj = Instantiate(prefab);
        gameobj.transform.SetParent(canvas.transform, false);

        var resume = gameobj.GetComponent<ResumeUILayout>();

        resume.name = name;
        resume.age = age;
        resume.id = id;
        resume.competence_descs = competences;

        resume.font = font;

        return resume;
    }

    public Document spawn_id(string name, string id)
    {
        var gameobj = Instantiate(document_prefab,Singleton.Instance.instantiate_document_pos,Quaternion.identity);
        var doc_layout = gameobj.GetComponent<IdDocumentLayout>();
        doc_layout.name = name; doc_layout.id = id;
        var doc = gameobj.GetComponent<Document>();
        doc.my_person = current_person;

        return doc;
    }

    public PersonInRoom create_person(Sprite sprite, Sprite[] animation)
    {
        var gameobj = Instantiate(person_prefab);

        current_person_obj = gameobj.GetComponent<PersonInRoom>();
        
        current_person_obj.idle_sprite = sprite;
        current_person_obj.talking_animation = animation;

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

    public void finish_day(float points_result)
    {
        var points = calculate_future_points(points_result);

        //Add points
        Singleton.Instance.future_points += points;

        //Spawn bonus points effect
        GameObject obj = Instantiate(Singleton.Instance.bonus_points_prefab);
        obj.transform.SetParent(future_points_ui.transform, false);
        obj.transform.localPosition = Vector3.up * 60;

        obj.GetComponent<BonusPoints>().points = points;

        Singleton.Instance.go_to_next_day();
    }

    public float calculate_future_points(float raw_points)
    {
        float points = raw_points;

        if (points < 0) points = 0;

        points *= 100;

        return points;
    }
}

public interface IState //State Interface
{
    void EnterState();
    void UpdateState();
    void ExitState();
}
