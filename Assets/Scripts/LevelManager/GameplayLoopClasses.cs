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
    public string name;
    public int age;
    public string id;

    public AudioClip[] voice;

    public DialogueStruc[] initial_dialogue;
    public Competence[] competences;
}

[Serializable]
public class Competence
{
    [Serializable]
    private struct Test 
    {
        public DialogueStruc[] true_answer;
        public DialogueStruc[] false_answer;
    }
    
    public string resume_description;
    public string test_question;

    public int[] points = new int[(int) Jobs.Security+1]; 
}