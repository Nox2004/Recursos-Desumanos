using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton : MonoBehaviour
{
    public static Singleton Instance { get; private set; }

    //Some global stuff
    public bool debug_game;
    public Vector2 perspective_point;
    public Camera cam;
    public DialogueCharacter player_character;
    public DialogueCharacter interviewed_character;
    public GameObject prefab_transition;

    public Vector3 instantiate_document_pos;

    public AudioClip transit_sound;

    private void Awake()
    {
        //Add a transition to the first scene
        if (debug_game)
        {
            create_transition(TransitionMode.Scene, "DayOne");
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
    }

    public void create_transition(TransitionMode mode, string target = null)
    {
        GameObject transition = Instantiate(prefab_transition);
        transition.GetComponent<Transition>().mode = mode;
        transition.GetComponent<Transition>().target = target;
        transition.GetComponent<Transition>().transit_sound = transit_sound;
    }
}