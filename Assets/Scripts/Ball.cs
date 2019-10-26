using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{

     float mass;  
     Vector3 velocity = Vector3.zero;
     List<Vector3> forces = new List<Vector3>(0);
     Vector3 netForce = Vector3.zero;
     float uFriction;   
     public float radius;
     Renderer rend;   

    public void AddForce(Vector3 force){
        Debug.Log("Force Vector Recieved:"+force);
        forces.Add(force);
    }

    void Start(){
        PhysxSheet sheet = GameObject.FindObjectOfType<PhysxSheet>();
        mass = sheet.mass;
        uFriction = sheet.uFriction;
        rend = GetComponent<Renderer>();
        radius = rend.bounds.extents.magnitude;
    }
    
    void FixedUpdate()
    {
        processTheForces();
    }

    void processTheForces(){

		netForce = Vector3.zero;
		foreach (Vector3 forceVector in forces) {
			netForce = netForce + forceVector;
		}
		forces.Clear();
        //Debug.Log("Net Force is: "+netForce);

        /* 
        Vector3 frictionForce = getFrictionToTheOpposingForce(netForce);
        netForce+=frictionForce;
        Debug.Log("Net Force after applying friction is: "+netForce);
        */
        Vector3 accelerationVector = netForce / mass;
		velocity += accelerationVector * Time.deltaTime;
		
        //Given the setup of our system, friction can not be applied in the tradtional sense. So, we're just gonna have an opposing velocity and when the co-eff is 1, they
        //cancel each other out and there is no change in position

        Vector3 velocityAsAResultOfFriction = -velocity * uFriction;
        velocity+=velocityAsAResultOfFriction;

        transform.position += velocity * Time.deltaTime;
    }

    /* 
    private Vector3 getFrictionToTheOpposingForce(Vector3 force){
            return -force * uFriction;
    }
*/
    public Vector3 getVelocity(){
        return velocity;
    }

    public Vector3 getNetForce(){
        return netForce;
    }

    public void setVelocity(Vector3 velocity){
        this.velocity = velocity;
    }

    public Vector3 getCenterPosition(){
        return transform.position;
    }



    
    public float getRadius(){
       return radius;
    }

    public Vector3 getCenter(){
        return rend.bounds.center;
    }
}
