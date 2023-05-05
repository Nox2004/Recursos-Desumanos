using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostInterview : IState
{
    private LevelManager me;

    public PostInterview(LevelManager level_manager)
    {
        this.me = level_manager;
    }

    public void EnterState()
    {
        me.camera.locked = false;
    }

    public void UpdateState()
    {
        
    }

    public void ExitState()
    {
        
    }
}