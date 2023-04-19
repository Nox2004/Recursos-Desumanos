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
        //if (me.current_person_obj.HasEntered())
        //{
        //    me.change_state(me.initial_dialogue);
        //}
    }

    public void ExitState()
    {
        
    }
}