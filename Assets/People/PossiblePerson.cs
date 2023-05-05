using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "PossiblePerson", menuName = "Game/PossiblePerson", order = 1)]
public class PossiblePerson : ScriptableObject 
{
    public string[] possible_names;
    public int age_min;
    public int age_max;
    public string[] possible_ids;

    public AudioClip[] voice;
    
    public Sprite sprite;
    public Sprite document_sprite;

    public Sprite resume_sprite;
    public TMPro.TMP_FontAsset resume_font;
    public Color dialogue_color;
    public string[] resume_competences;

    public Person.JobPoints[] job_points;

    public DialogueStruc[] initial_dialogue;
    public DialogueStruc[] conclusion_dialogue;
    
    public Person.Question[] possible_questions;

    [HideInInspector] public DialogueCharacter dialogue_character;

    public Person create_person()
    {
        Person person = new Person();

        person.name = possible_names[UnityEngine.Random.Range(0,possible_names.Length)];
        person.age = UnityEngine.Random.Range(age_min,age_max);
        person.id = possible_ids[UnityEngine.Random.Range(0,possible_ids.Length)];

        person.voice = voice;

        person.sprite = sprite;
        person.document_sprite = document_sprite;

        person.resume_competences = resume_competences;

        person.job_points = job_points;

        person.initial_dialogue = initial_dialogue;
        person.conclusion_dialogue = conclusion_dialogue;

        person.possible_questions = possible_questions;

        person.create_dialogue_character();

        return person;
    }

    public void do_stuff()
    {
        Debug.Log("Doing stuff");
    }
} 