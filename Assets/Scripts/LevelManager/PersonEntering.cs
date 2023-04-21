
public class PersonEntering : IState
{
    private LevelManager me;

    public PersonEntering(LevelManager level_manager)
    {
        this.me = level_manager;
    }
    
    public void EnterState()
    {
        me.create_person(me.current_person.sprite);
    }

    public void UpdateState()
    {
        if (me.current_person_obj.HasEntered())
        {
            me.change_state(me.initial_dialogue);
        }
    }

    public void ExitState()
    {
        
    }
}