using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeQuestion : IState
{
    private bool jump_to_conclusion = false;
    private LevelManager me;
    private Option my_option;
    private Person.Question current_question;

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
        if ((question_index < me.number_of_questions) && (!jump_to_conclusion))// (int i = 0; i < me.number_of_questions; i++)
        {
            switch (stage)
            {
                case 0:
                {
                    //Instantiate all options
                    my_option = instantiate_options();
                    if (my_option == null)
                    {
                        jump_to_conclusion = true;
                    }
                    stage++;
                }
                break;
                case 1:
                {
                    //Checks if option was choosen
                    if (my_option.choosen_option!=-1)
                    {
                        current_question = my_option.choosen_option_struc.question;

                        //if (question_index < me.number_of_questions)
                        //{
                            
                        //}
                        set_text_from_option(my_option.choosen_option_struc,false);

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
                        if (current_question.output_action.GetPersistentEventCount() > 0)
                        {
                            current_question.output_action.Invoke();
                        }

                        if (current_question.follow_up_questions.Length > 0)
                        {
                            //Adds follow up questions
                            my_option = create_follow_up_options(current_question.follow_up_questions);
                            
                            stage--;

                            return;
                        }

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
        
    }

    public Option instantiate_options()
    {
        //Gets number of questions
        int remaining_options = 0;

        foreach (var question in me.current_person.possible_questions)
        {
            if (!question.asked) { remaining_options++; }
        }

        if (remaining_options <= 0) return null;

        //Creates a question blacklist
        List<string> selected_list = new List<string>();

        //create maximum possible option struts
        int number_of_options = Mathf.Min(me.options_per_question,remaining_options);

        OptionValues[] current_options = new OptionValues[number_of_options];

        for (int i = 0; i < number_of_options; i++)
        {
            while (true)
            {
                var list = me.current_person.possible_questions;
                var index = Random.Range(0, list.Length);
                var question = list[index];

                if (question.asked) { continue; } //Loops again if question were alread asked

                if (selected_list.IndexOf(question.option_text) != -1) { continue; } //Loop again if question is alread in selection

                current_options[i] = new OptionValues(question.option_text, question, index);
                selected_list.Add(question.option_text); //adds question to blacklist
                break;
                
            }
        }

        return me.create_options(current_options);
    }

    private Option create_follow_up_options(Person.Question[]  follow_up_questions)
    {
        //Creates options from follow up questions
        OptionValues[] current_options = new OptionValues[follow_up_questions.Length];

        for (int i = 0; i < follow_up_questions.Length; i++)
        {
            current_options[i] = new OptionValues(follow_up_questions[i].option_text, follow_up_questions[i]);
        }

        return me.create_options(current_options);
    }

    private void set_text_from_option(OptionValues selected_option, bool add_next_dialogue = true)
    {
        //Gets current competence and test from option
        DialogueStruc[] dialogue;

        var comp_option = selected_option;
        int index = comp_option.question_index;

        if (index != -1) 
        {
            me.current_person.possible_questions[index].asked = true; //Marks question as askes
        }

        dialogue = current_question.output; //Sets question output

        //Adds "Next question!" dialogue
        if (add_next_dialogue) 
        {
            System.Array.Resize(ref dialogue,dialogue.Length+1);
            dialogue[dialogue.Length-1] = me.next_question_dialogue;
        }
        //Sets dialogue
        me.set_current_dialogue(dialogue);
    }
}