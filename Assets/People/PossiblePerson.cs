using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "PossiblePerson", menuName = "Game/PossiblePerson", order = 1)]
public class PossiblePerson : ScriptableObject 
{
    [Header("Pontos por vaga")]
    [Space(10)]
    public Person.JobPoints[] job_points;
    public int moral_points; //moral points gained by hiring this person

    [Header("Atributos randômicos da pessoa")]
    [Space(10)]
    public string[] possible_names;
    public int age_min;
    public int age_max;
    public string[] possible_ids;

    [Header("Clipes de voz")]
    [Space(10)]
    public AudioClip[] voice;
    
    [Header("Sprites")]
    [Space(10)]
    public Sprite in_room_sprite;
    public Sprite[] talking_animation;

    [HideInInspector] public Sprite document_sprite;

    [Header("curriculo")]
    [Space(10)]
    public GameObject resume_prefab;
    public TMPro.TMP_FontAsset resume_font;
    public string[] resume_competences;

    [Header("Cor do dialogo")]
    [Space(10)]
    public Color dialogue_color;

    [Header("Dialogos")]
    [Space(10)]
    public DialogueStruc[] initial_dialogue;
    public DialogueStruc[] conclusion_dialogue;
    
    [Header("Questões")]
    [Space(10)]
    public Person.Question[] possible_questions;
    
    //[HideInInspector] public DialogueCharacter dialogue_character;

    public Person create_person()
    {
        Person person = new Person();

        Debug.Log(possible_names[0]);
        person.name = possible_names[UnityEngine.Random.Range(0,possible_names.Length)];
        person.age = UnityEngine.Random.Range(age_min,age_max);
        person.id = possible_ids[UnityEngine.Random.Range(0,possible_ids.Length)];

        person.voice = voice;

        person.in_room_sprite = in_room_sprite;
        person.talking_animation = talking_animation;
        person.document_sprite = document_sprite;

        person.resume_prefab = resume_prefab;
        person.resume_competences = resume_competences;
        person.resume_font = resume_font;

        person.dialogue_color = dialogue_color;

        person.job_points = job_points;
        person.moral_points = moral_points;

        person.initial_dialogue = initial_dialogue;
        person.conclusion_dialogue = conclusion_dialogue;

        person.possible_questions = possible_questions;
        for (int i = 0; i < person.possible_questions.Length; i++)
        {
            person.possible_questions[i].asked = false;
        }

        person.create_dialogue_character();

        return person;
    }

    public void do_stuff()
    {
        Debug.Log("Doing stuff");
    }
} 