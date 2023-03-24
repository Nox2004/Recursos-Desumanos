using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aula03 : MonoBehaviour
{
    private class Ball 
    { 
        protected Vector2 position;
        protected Vector2 velocity;
        protected float mass;

        public Ball(Vector2 pos, Vector2 vel, float m)
        {
            position = pos; velocity = vel; mass = m;
        }

        public Vector2 collide(Ball other_ball, bool rec = false)
        {
            //https://en.wikipedia.org/wiki/Elastic_collision
            var vel1 = velocity; var pos1 = position; var mass1 = mass;
            var vel2 = other_ball.velocity; var pos2 = other_ball.position; var mass2 = other_ball.mass;

            float norm(Vector2 foo)
            {
                return Mathf.Sqrt(Mathf.Pow(foo.x, 2f) + Mathf.Pow(foo.y, 2f));
            }

            var output = vel1;

            output -= ((2 * mass2) / (mass1 + mass2)) * (Vector2.Dot(vel1 - vel2, pos1 - pos2) / norm(pos1 - pos2)) * (pos1 - pos2);
            if (!rec) other_ball.collide(this,true);
            velocity = output;

            return output;
        }
        public void debug() { Debug.Log("pos: " + position.x + "," + position.y + "\nvel: " + velocity.x + "," + velocity.y + "\nmass: " + mass); }

        public void move() { position += velocity; }
    }
    void Start()
    {
        Ball ball1 = new Ball(new Vector2(0,0), new Vector2(1, 0), 1);
        Ball ball2 = new Ball(new Vector2(1,0), new Vector2(-1, 0.2f), 1);

        ball1.collide(ball2);

        ball1.move(); ball2.move();
        ball1.debug(); ball2.debug();
    }
    void Update()
    {
        
    }

}
