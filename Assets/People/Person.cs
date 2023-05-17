using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

public enum Jobs
{
    [InspectorName("Vendas")] 
    Sales=0,
    [InspectorName("Contabilidade")] 
    Accounting=1,
    [InspectorName("Faxina")] 
    Cleaning=2,
    [InspectorName("Segurança")] 
    Security=3
}

[Serializable]
public class Person 
{
    [Serializable]
    public struct Question
    {
        [InspectorName("Texto da opção")] public string option_text;

        [InspectorName("Ação")] public UnityEvent output_action;

        [InspectorName("Dialogo")] public DialogueStruc[] output;

        [HideInInspector] public bool asked;

        [InspectorName("Questões de Follow-up")] public Question[] follow_up_questions;
    }
    [Serializable]
    public struct JobPoints 
    {
        [MinAttribute(0)] public Jobs job;
        public int points;
    }

    [Header("Pontos por vaga")]
    [Space(10)]
    public JobPoints[] job_points;
    public int moral_points; //moral points gained by hiring this person

    [Header("Atributos da pessoa")]
    [Space(10)]
    public string name;
    public int age;
    public string id;

    [Header("Sprites")]
    [Space(10)]
    public Sprite in_room_sprite;
    public Sprite[] talking_animation;
    public Sprite document_sprite;

    [Header("Clipes de voz")]
    [Space(10)]
    public AudioClip[] voice;

    [Header("Curriculo")]
    [Space(10)]
    public string[] resume_competences;
    public GameObject resume_prefab; 
    public TMPro.TMP_FontAsset resume_font;

    public Color dialogue_color;

    [Header("Dialogos")]
    [Space(10)]
    public DialogueStruc[] initial_dialogue;
    public DialogueStruc[] conclusion_dialogue;

    [Header("Perguntas")]
    [Space(10)]
    public Question[] possible_questions;

    [HideInInspector] public DialogueCharacter dialogue_character;

    public void create_dialogue_character()
    {
        dialogue_character = (DialogueCharacter) ScriptableObject.CreateInstance("DialogueCharacter");
        dialogue_character.name = name;
        dialogue_character.voice = voice;
        dialogue_character.speech_color = dialogue_color;
    }

    public int evaluate_in_job(Jobs job)
    {
        //return if has no points
        if (job_points == null || job_points.Length <= 0) return 0;

        //Gets the value of this person in the job
        int value = 0;
        foreach (JobPoints point in job_points)
        {
            if (point.job == job) value+=point.points;
        }

        //0: awful
        //1-3: bad
        //4-6: ok
        //7-9: good
        //10: great

        if (value <= 0) return -2;
        if (value <= 3) return -1;
        if (value <= 6) return 1;
        if (value <= 9) return 2;
        if (value <= 10) return 4;

        return value;
    }
} 

