using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeQuestion : IState
{
    private LevelManager me;
    private Option my_option;

    private int question_index = 0;
    private int stage;  //0 - instantiate questions
                        //1 - checks if option were chosen and sets text
                        //2 - waits for dialogue to end and says "Next Question..."
    
    public MakeQuestion(LevelManager level_manager)
    {
        this.me = level_manager;
    }

    public void EnterState()
    {  
        question_index = 0;
        stage = 0;
    }

    public void UpdateState()
    {
        if (question_index < me.number_of_questions)// (int i = 0; i < me.number_of_questions; i++)
        {
            switch (stage)
            {
                case 0:
                {
                    //Instantiate all options
                    my_option = instantiate_options();
                    stage++;
                }
                break;
                case 1:
                {
                    //Checks if option was choosen
                    if (my_option.choosen_option!=-1)
                    {
                        set_text_from_option(my_option.choosen_option_struc);
                        me.destroy_option();

                        stage++;
                    }
                }
                break;
                case 2:
                {
                    //Waiting dialogue to end
                    if (me.current_dialogue.finished_text())
                    {
                        //Goes to next question and reset stage
                        stage = 0;
                        question_index++;
                    } 
                }
                break;
            }
        }
        else
        {
            if (InGameCursor.get_button_down(0)) me.change_state(me.interview_conclusion);
        }
    }

    public void ExitState()
    {
        me.set_current_dialogue(me.current_person.conclusion_dialogue);
        me.current_dialogue.hide_at_end = true;
    }

    public Option instantiate_options()
    {
        //create 3 option struts
        OptionValues[] current_options = new OptionValues[me.options_per_question];
        //competence blacklist 
        List<string> selected_list = new List<string>();

        for (int i = 0; i < me.options_per_question; i++)
        {
            while (true)
            {
                //Personal questions
                if (i < me.personal_options_per_question)
                {
                    var list = me.current_person.possible_personal_questions;
                    var question_index = Random.Range(0, list.Length);
                    var question = list[question_index];

                    if (question.asked) { continue; } //Loops again if question were alread asked

                    if (selected_list.IndexOf(question.option_text) != -1) { continue; } //Loop again if question is alread in selection

                    current_options[i] = new PersonalQuestionOption(question.option_text, question_index);
                    selected_list.Add(question.option_text); //adds question to blacklist
                    break;
                }
                else 
                {
                    //Choose a random individual competence
                    var list = me.current_person.competences;
                    int comp_index = Random.Range(0, list.Length);
                    var comp = list[comp_index];

                    if (comp.tested) { continue; } //Loops again if competence was alread tested

                    if (selected_list.IndexOf(comp.competence.name) != -1) { continue; } //Loop again if competence is alread selected

                    var test_list = comp.competence.possible_tests; //List of possible tests for that competence

                    //Choose random test
                    int test_index = Random.Range(0, test_list.Count);
                    var test = test_list[test_index];
                    
                    current_options[i] = new TestCompetenceOption(test.option_text, comp_index, test);
                    selected_list.Add(comp.competence.name); //add competence name to black list (thus, why name atribute must be unique)
                    break;
                }
            }
        }

        return me.create_options(current_options);
    }

    private void set_text_from_option(OptionValues selected_option)
    {
        //Gets current competence and test from option
        int index;
        DialogueStruc[] dialogue;

        if (selected_option is TestCompetenceOption)
        {
            var test_option = (TestCompetenceOption) selected_option;
            index = test_option.competence_index;

            me.current_person.competences[index].tested = true; //Marks competence as alread tested
            
            TestCompetence test = test_option.test;
            Person.individualCompetence comp = me.current_person.competences[index];
            //gets dialogue from test checking if person is telling the truth or not
            dialogue = comp.is_true ? test.true_output : test.false_output;
        }
        else //if (selected_option is PersonalQuestionOption)
        {
            var comp_option = (PersonalQuestionOption) selected_option;
            index = comp_option.question_index;

            me.current_person.possible_personal_questions[index].asked = true; //Marks question as askes
            dialogue = me.current_person.possible_personal_questions[index].output; //ets question output
        }

        //Adds "Next question!" dialogue
        if (question_index < me.number_of_questions-1) 
        {
            System.Array.Resize(ref dialogue,dialogue.Length+1);
            dialogue[dialogue.Length-1] = me.next_question_dialogue;
        }
        //Sets dialogue
        me.set_current_dialogue(dialogue);
    }
}