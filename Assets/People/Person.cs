using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

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
    public struct Question
    {
        public string option_text;
        public UnityEvent output_action;
        public DialogueStruc[] output;

        [HideInInspector] public bool asked;

        public Question[] follow_up_questions;
    }
    public struct JobPoints 
    {
        public Jobs job;
        public int points;
    }

    public string name;
    public int age;
    public string id;

    public AudioClip[] voice;

    public Sprite sprite;
    public Sprite document_sprite;

    public string[] resume_competences; 

    public JobPoints[] job_points;

    public DialogueStruc[] initial_dialogue;
    public DialogueStruc[] conclusion_dialogue;

    public Question[] possible_questions;

    [HideInInspector] public DialogueCharacter dialogue_character;

    public void create_dialogue_character()
    {
        dialogue_character = (DialogueCharacter) ScriptableObject.CreateInstance("DialogueCharacter");
        dialogue_character.name = name;
        dialogue_character.voice = voice;
    }

    public float evaluate_in_job(Jobs job)
    {
        float value = 0;
        foreach (JobPoints point in job_points)
        {
            if (point.job == job) value+=point.points;
        }

        return value;
    }
} 

