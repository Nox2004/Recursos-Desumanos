using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum Jobs
{
    Sales=0,
    Accounting=1,
    Cleaning=2,
    Security=3
}

[Serializable]
public class Person 
{
    [Serializable]
    public struct PersonalQuestion
    {
        public string option_text;
        public bool asked;
        public DialogueStruc[] output;
    }

    [Serializable]
    public struct individualCompetence
    {
        public Competence competence;
        public bool is_true;
        public bool tested;
    }

    public string name;
    public int age;
    public string id;

    public AudioClip[] voice;

    public Sprite sprite;
    public Sprite document_sprite;

    public DialogueStruc[] initial_dialogue;
    public DialogueStruc[] conclusion_dialogue;
    public individualCompetence[] competences;
    public PersonalQuestion[] possible_personal_questions;

    public DialogueCharacter dialogue_character;

} 