public class InterviewConclusion : IState
{
    private LevelManager me;

    public InterviewConclusion(LevelManager level_manager)
    {
        this.me = level_manager;
    }

    public void EnterState()
    {
        
    }

    public void UpdateState()
    {
        if (me.current_dialogue != null)
        {
            if (me.current_dialogue.hiding_box())
            {
                me.destroy_dialogue();
                me.current_dialogue = null;
                me.current_person_obj.exiting = true;
            }
        }
        else 
        {
            if (me.current_person_obj.HasExited())
            {
                me.destroy_person_obj();

                if (me.person_index < me.people_in_list-1)
                {
                    me.person_index++;
                    me.change_state(me.person_intro);
                } 
                else 
                {
                    me.current_person = null;
                    //change to ending interview stage
                }
                
            }
        }
    }

    public void ExitState()
    {
        
    }
}