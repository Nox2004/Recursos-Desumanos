using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System;

[CreateAssetMenu(fileName = "New Character", menuName = "Game/Character")]
public class DialogueCharacter : ScriptableObject
{
    public string name;
    public Color speech_color;
    public AudioClip[] voice;

    public PersonInRoom person_in_room; // will be used by Dialogue to animate the person that is talking

    public DialogueCharacter(string char_name, AudioClip[] char_voice)
    {
        name = char_name;
        voice =  char_voice;
    }
}