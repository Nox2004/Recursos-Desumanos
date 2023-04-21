using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeQuestion : IState
{
    private LevelManager me;

    public MakeQuestion(LevelManager level_manager)
    {
        this.me = level_manager;
    }

    public void EnterState()
    {  
        instantiate_options();
    }

    public void UpdateState()
    {
        if (me.current_options == null) return;

        if (me.current_options.choosen_option!=-1)
        {
            set_text_from_option(me.current_options.choosen_option_struc);   
            me.destroy_option();
        }
    }

    public void ExitState()
    {
        
    }

    public Option instantiate_options()
    {
        //create 3 option struts
        OptionStruc[] current_options = new OptionStruc[me.number_of_options];
        //competence blacklist 
        List<string> selected_list = new List<string>();

        for (int i = 0; i < me.number_of_options; i++)
        {
            while (true)
            {
                //Choosea radom individual competence
                var list = me.current_person.competences;
                int comp_index = Random.Range(0, list.Length);
                var comp = list[comp_index];

                if (comp.tested) { continue; } //Loops again if competence was alread tested

                if (selected_list.IndexOf(comp.competence.name) != -1) { continue; } //Loop again if competence is alread selected

                var test_list = comp.competence.possible_tests; //List of possible tests for that competence

                //Choose random test
                int test_index = Random.Range(0, test_list.Count);
                var test = test_list[test_index];
                
                current_options[i] = new OptionStruc(test.option_text, comp_index, test);
                selected_list.Add(comp.competence.name); //add competence name to black list (thus, why name atribute must be unique)
                break;
            }
        }

        return me.create_options(current_options);
    }

    private void set_text_from_option(OptionStruc selected_struct)
    {
        //Gets current competence and test from option
        int comp_index = selected_struct.competence_index;
        TestCompetence test = selected_struct.test_made;

        Person.individualCompetence comp = me.current_person.competences[comp_index];
        
        //Gets answer from test struct, depending if the person is lying or not
        string[] text = comp.is_true ? test.true_answer : test.false_answer;

        DialogueStruc[] dialogue = new DialogueStruc[text.Length+1]; //Initiate an array
        dialogue[0] = new DialogueStruc(test.question,"NOME JOGADOR"); //Makes question
        
        //Defines dialogue structs to persons answer
        for (int t = 1; t < text.Length+1; t++)
        {
            Debug.Log(t);
            dialogue[t] = new DialogueStruc(text[t-1], me.current_person.name);
        }

        me.current_dialogue.set_text(dialogue);

        //Marks competence as alread tested
        me.current_person.competences[comp_index].tested = true;
    }
}