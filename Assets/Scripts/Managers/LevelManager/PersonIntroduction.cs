
public class PersonIntroduction : IState
{
    public int stage = 0; //0 - create documents
                          //1 - instantiate person 
                          //2 - change state
    private LevelManager me;
    private ResumeUILayout resume;

    public PersonIntroduction(LevelManager level_manager)
    {
        this.me = level_manager;
    }
    
    public void EnterState()
    {
        resume = me.create_resume(me.current_person.resume_prefab,me.current_person.resume_font,me.current_person.name,me.current_person.age,me.current_person.id,me.current_person.resume_competences);
    }
    
    public void UpdateState()
    {
        switch (stage)
        {
            case 0: 
            {
                if (resume.finished_intro())
                {
                    me.spawn_id(me.current_person.name, me.current_person.id);
                    stage++;
                }
            }
            break;
            case 1:
            {
                if (resume.finished_outro())
                {
                    me.destroy_obj(resume.gameObject);

                    var in_room = me.create_person(me.current_person.in_room_sprite, me.current_person.talking_animation);
                    me.current_person.dialogue_character.person_in_room = in_room;
                    
                    stage++;
                }
            }
            break;
            case 2:
            {
                if (me.current_person_obj.HasEntered())
                {
                    me.change_state(me.initial_dialogue);
                }
            }
            break;
        }
    }

    public void ExitState()
    {
        stage = 0;
    }
}