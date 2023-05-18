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
    private FinishButton finish_button;

    public PostInterview(LevelManager level_manager)
    {
        this.me = level_manager;
    }

    public void EnterState()
    {
        day_finished = false;
        finish_button = null;

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
            //Creates "finish day" button and evaluate your future points
            if (finish_button == null)
            {
                //Creates button
                GameObject button_obj = GameObject.Instantiate(me.finish_button_prefab);
                button_obj.transform.SetParent(me.canvas.transform);
                finish_button = button_obj.GetComponent<FinishButton>();

                //Calculates future points
                int points = 0;

                foreach (Drawer drawer in drawers)
                {
                    drawer.locked = true;
                    foreach (Person person in drawer.people_hired)
                    {
                        points += person.evaluate_in_job(drawer.job);
                    }
                }

                //Add corporate points
                Singleton.Instance.corporate_points += (int) points;

                //Add Future points
                float fp = me.calculate_future_points(points);
                Singleton.Instance.future_points += fp;

                //Spawn bonus points effect
                GameObject points_obj = GameObject.Instantiate(Singleton.Instance.bonus_points_prefab);
                points_obj.transform.SetParent(me.future_points_ui.transform, false);
                points_obj.transform.localPosition = Vector3.up * 60;

                points_obj.GetComponent<BonusPoints>().points = fp;
            }
            else
            {
                //Finish day
                if (finish_button.activacted && !day_finished)
                {
                    //Calculates moral points
                    int points = 0;

                    foreach (Drawer drawer in drawers)
                    {
                        drawer.locked = true;
                        foreach (Person person in drawer.people_hired)
                        {
                            points += person.moral_points;
                        }
                    }
                    Singleton.Instance.moral_points += points;

                    me.finish_day();
                    day_finished = true;
                }
            }
            
        }
    }

    public void ExitState()
    {
        
    }
}