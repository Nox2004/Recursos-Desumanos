using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton : MonoBehaviour
{
    
    public static Singleton Instance { get; private set; }

    //Some global stuff
    public GameObject table;
    public Vector2 perspective_point;
    public Camera cam;
    public DialogueCharacter player_character;

    public Vector3 instantiate_document_pos;
    

    private void Awake()
    {
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
}
