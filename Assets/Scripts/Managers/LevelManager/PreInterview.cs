using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "PreInterview", menuName = "Game/Level Introduction", order = 1)]
public class PreInterview : ScriptableObject, IState
{
    private LevelManager me;
    public UnityEvent method; 

    private bool finish;

    public PreInterview(LevelManager level_manager)
    {
        this.me = level_manager;
    }

    public void EnterState()
    {
        
    }

    public void UpdateState()
    {
        method.Invoke();
        if (finish)
        {
            me.change_state(me.make_questions);
        }
    }

    public void ExitState()
    {
        
    }

    public void Level1()
    {
        if (Time.timeSinceLevelLoad < 5) return;
        finish = true;
    }
}