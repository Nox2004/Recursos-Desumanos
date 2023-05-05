using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System;

[CreateAssetMenu(fileName = "New Character", menuName = "Game/Character")]
public class DialogueCharacter : ScriptableObject
{
    public string name;
    public AudioClip[] voice;

    public DialogueCharacter(string char_name, AudioClip[] char_voice)
    {
        name = char_name;
        voice =  char_voice;
    }
}