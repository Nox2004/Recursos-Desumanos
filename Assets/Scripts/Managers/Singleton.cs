using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Singleton : MonoBehaviour
{
    public static Singleton Instance { get; private set; }

    //Some important global stuff
    public string player_name;
    public bool debug_game;
    public bool game_paused = false;
    public Vector2 perspective_point;
    public Camera cam;

    //Player Dialogue Character
    public DialogueCharacter player_character;

    public DialogueCharacter manager_character;
    public Sprite manager_sprite;
    public Sprite[] manager_animation;

    //Where people will instantiate documents
    public Vector3 instantiate_document_pos;

    //Transition sound
    public AudioClip transit_sound;

    //Gameplay stuff
    public float future_points = 0; //Money
    public int corporate_points = 0; //Points with the corporation
    public int moral_points = 0; //Points gained by doing good things

    [HideInInspector] public int current_day = 1; //current_day
    public int first_day;
    public int last_day;


    //Possible people to be interviewed
    [Header("Possible People")]
    [Space(10)]

    [SerializeField] private List<PossiblePerson> initial_random_people; //List of all randomized people to be possibly interviewed
    [HideInInspector] public List<PossiblePerson> random_people;

    //Important prefabs    
    [Header("Prefabs importantes")]
    [Space(10)]

    public GameObject prefab_transition;
    public GameObject day_count_prefab;
    public GameObject future_points_prefab;
    public GameObject bonus_points_prefab;
    public GameObject player_name_form_prefab;
    public GameObject tutorial_folder_prefab;

    public void reset()
    {
        future_points = 0;
        corporate_points = 0;
        moral_points = 0;

        current_day = first_day;
        Debug.Log(initial_random_people[0].possible_names[0]);
        random_people = new List<PossiblePerson>(initial_random_people);
    } 

    private void Awake()
    {
        //Add a transition to the first scene
        if (debug_game)
        {
            go_to_first_day();
            //create_transition(TransitionMode.Scene, "Day1");
        }

        //Finds the camera every room and sets up the static var "cam"
        cam = GameObject.FindGameObjectsWithTag("MainCamera")[0].GetComponent<Camera>();

        //Garantees there will be only one singleton in scene
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
        
        DontDestroyOnLoad(gameObject);
        
        reset();
    }

    public void create_transition(TransitionMode mode, string target = null)
    {
        GameObject transition = Instantiate(prefab_transition);
        transition.GetComponent<Transition>().mode = mode;
        transition.GetComponent<Transition>().target = target;
        transition.GetComponent<Transition>().transit_sound = transit_sound;
    }

    public void go_to_first_day()
    {
        current_day = first_day;
        go_to_day(current_day);
    }

    public void go_to_next_day()
    {
        current_day++;

        if (current_day > last_day)
        {
            create_transition(TransitionMode.Scene, "End"); return;
        }

        go_to_day(current_day);
    }

    public void go_to_menu()
    {
        create_transition(TransitionMode.Scene, "Menu");
    }

    public void go_to_day(int d)
    {
        string scene_name = "Day" + d.ToString();
        create_transition(TransitionMode.Scene, scene_name);
    }
}