using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysxSheet : MonoBehaviour
{
    [SerializeField] public float mass;
    [Range(0,1)] 
    [SerializeField] public float eRestituion;

    [Range(0,1)] 
    [SerializeField] public float uFriction;

    public float offsetImpulseCorrection = 5000;    

    [Range(2,9)]
    public int cushionMassMultiplier =2; //the cushion's mass would be relative to the mass of the ball but one thing is for certain, it's always gonna be more than the ball


    /* 
    [Range(0,10)]
    [SerializeField] public float gravity = 9.8F; //[N]
    */    
/* 
    public Vector3 CalculateImpulse(Transform A, Transform B){
        
    return new V
    }
    */

    public Vector3 CalculateImpulseVectorBall(Ball A, Ball B){

        Vector3 AB = B.getCenter() - A.getCenter();
        Vector3 n = AB.normalized;
        float Impulse = (-Vector3.Dot((1+eRestituion)*AB,n))/Vector3.Dot(n,(2/mass)*n); 
        Vector3 impulse = offsetImpulseCorrection*Impulse*n;

        //Removing the z component
        impulse.z = 0;
        return impulse;
    }
    
     public Vector3 CalculateImpulseVectorBallCushion(Ball A, Cushion B){

        Vector3 AB = B.center - A.getCenter();
        Vector3 n = AB.normalized;
        float Impulse = (-Vector3.Dot((1+eRestituion)*AB,n))/Vector3.Dot(n,(1/mass + 1/(mass*cushionMassMultiplier))*n); 
        
        Vector3 impulse = (offsetImpulseCorrection/3F)*Impulse*n;

        //Removing the z component
        impulse.z = 0;
        return impulse;
    }

}
