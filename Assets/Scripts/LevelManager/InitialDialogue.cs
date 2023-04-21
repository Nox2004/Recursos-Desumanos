public class InitialDialogue : IState
{
    private LevelManager me;

    public InitialDialogue(LevelManager level_manager)
    {
        this.me = level_manager;
    }

    public void EnterState()
    {
        me.create_dialogue(me.current_person.initial_dialogue);
    }

    public void UpdateState()
    {
        if (me.current_dialogue.finished_text())
        {
            me.change_state(me.make_questions);
        }
    }

    public void ExitState()
    {
        
    }
}