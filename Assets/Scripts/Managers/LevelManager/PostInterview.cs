using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostInterview : IState
{
    private LevelManager me;

    private List<Drawer> drawers;
    private int count;

    private HireCount hire_count_ui;
    private bool day_finished;

    public PostInterview(LevelManager level_manager)
    {
        this.me = level_manager;
    }

    public void EnterState()
    {
        day_finished = false;

        //Sets drawers
        drawers = new List<Drawer>();
        
        GameObject[] drawers_obj = GameObject.FindGameObjectsWithTag("Drawer");
        
        foreach (GameObject drawer in drawers_obj)
        {
            drawers.Add(drawer.GetComponent<Drawer>());
        }

        //Unlock camera
        me.camera.locked = false;

        //Unlock drawers
        foreach (Drawer drawer in drawers)
        {
            drawer.locked = false;
        }

        //Create count UI
        GameObject obj = GameObject.Instantiate(me.hire_count_prefab);
        obj.transform.SetParent(me.canvas.transform);
        obj.transform.localPosition = Vector3.zero;

        hire_count_ui = obj.GetComponent<HireCount>();
    }

    public void UpdateState()
    {
        count = 0;
        //Update count to be the number of documents inside the drawers
        foreach (Drawer drawer in drawers)
        {
            count += drawer.document_number;
        }

        //Update count UI
        hire_count_ui.count = count;
        hire_count_ui.limit = me.hire_limit;

        //Check if count is complete
        bool hire_limit_reached = count >= me.hire_limit;
        
        //if so: locks drawers, evaluate people and finish day (???)
        if (hire_limit_reached)
        {
            float points = 0;
            foreach (Drawer drawer in drawers)
            {
                drawer.locked = true;
                foreach (Person person in drawer.people_hired)
                {
                    points += person.evaluate_in_job(drawer.job);
                }
            }
            
            if (!day_finished)
            {   
                me.finish_day(points);
                day_finished = true;
            }
        }
    }

    public void ExitState()
    {
        
    }
}