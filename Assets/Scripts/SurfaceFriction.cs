using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurfaceFriction : MonoBehaviour
{
    Ball[] ballsInPlay;
    float uFriction;
    void Awake()
    {
        ballsInPlay = GameObject.FindObjectsOfType<Ball>();    
        uFriction   = GameObject.FindObjectOfType<PhysxSheet>().uFriction;  
    }

   
  
    void FixedUpdate()
    {
        
            foreach (Ball ball in ballsInPlay)
            {
                applyFriction(ball);
            }

    }

    void applyFriction(Ball ball){


            Vector3 velocity = ball.getNetForce();
            Vector3 directionOfMovement = velocity.normalized; // the ball's direction
            float speed = velocity.magnitude; // the ball's speed

            //The friction would be am opposing force to this movement, in an attempt to stop it
            if(speed>0){
                Vector3 friction = -uFriction * directionOfMovement; //if uFriction is one, the ball will stop in its step
                ball.AddForce(friction);
            }
    }
}
