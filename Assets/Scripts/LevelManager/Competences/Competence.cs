using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public struct TestCompetence
{
    public string option_text;
    public string question;
    public string[] true_answer;
    public string[] false_answer;

    public TestCompetence(string _option, string _question, string[] true_, string[] false_)
    {
        option_text = _option; question = _question; true_answer = true_; false_answer = false_;
    }
}

[Serializable]
public struct CompetencePoints
{
    public Jobs job;
    public int points;
}

[CreateAssetMenu(fileName = "New Competence", menuName = "Game/Competence")]
public class Competence : ScriptableObject
{   
    public string name;
    public string resume_description;

    public List<CompetencePoints> points = new List<CompetencePoints>();
    public List<TestCompetence> possible_tests = new List<TestCompetence>();

    /*
    public Competence (string _name, string desc, List<TestCompetence> tests)
    {
        name = _name;
        resume_description = desc;
        
        possible_tests = tests;
    }*/
}

/*
public class AllCompetences : MonoBehaviour
{
    //public static Singleton Instance { get; private set; }
    public static Competence[] competences;

    public static void init_competences()
    {
        competences = new Competence[20];

        competences[0] = new Competence(CompetenceEnum.Excel,
        "Curso de Excel 2006.",
        new TestCompetence[2] {
            new TestCompetence("Questionar a cor do excel","Qual a principal cor do tema do excel?", 
                new string[1] {"Verde"}, 
                new string[1] {"Azul"}),
            new TestCompetence("Como crio funções no excel?","Como adiciono uma funcao em uno excel?",
                new string[1] {"Verde"}, 
                new string[1] {"Azul"})});
    }

    public static Competence get_competence(CompetenceEnum find_id)
    {
        for (int i = 0; i < competences.Length; i++)
        {
            if (competences[i].id == find_id)
            {
                return competences[i];
            }
        }

        return null;
    }
}
*/