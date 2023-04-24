
public class PersonIntroduction : IState
{
    public int stage = 0; //0 - create documents
                          //1 - instantiate person 
                          //2 - change state
    private LevelManager me;
    private ResumeUI resume;

    public PersonIntroduction(LevelManager level_manager)
    {
        this.me = level_manager;
    }
    
    public void EnterState()
    {
        var comps = new string[me.current_person.competences.Length];
        
        for (int i = 0; i < me.current_person.competences.Length; i++)
        {
            comps[i] = me.current_person.competences[i].competence.resume_description;
        }

        resume = me.create_resume(me.current_person.name,me.current_person.age,me.current_person.id,comps);
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

                    me.create_person(me.current_person.sprite);
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