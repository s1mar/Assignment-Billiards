using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionManager : MonoBehaviour
{       
    Ball[] balls;
    Cushion[] cushions;
    
    [SerializeField]
    Renderer pocketPlaneRenderer;
    private Vector3 minExtents, maxExtents;

    [Range (0,1)]    
    public float radiusCorrectionOffset = 0;

    PhysxSheet sheet;
    void Awake(){
        balls = GameObject.FindObjectsOfType<Ball>();
        cushions = GameObject.FindObjectsOfType<Cushion>();
    }

    void Start(){

        sheet = GameObject.FindObjectOfType<PhysxSheet>();

        //pocket plane bounds
        minExtents =pocketPlaneRenderer.bounds.min;
        maxExtents = pocketPlaneRenderer.bounds.max;
    }


    //Since the number of objects in play are low in number and all are spheres, we're gonna check each against the other in every fixed update

    void FixedUpdate() {
         if(balls!=null && balls.Length>0){

                for (int i = 0; i < balls.Length; i++)
                {       

                     //Check for Cushion collision in here as well 
                    for(int k =0;k<cushions.Length;k++){
                                Ball ball = balls[i];
                                Cushion cushion = cushions[k];
                                //Vector3 possibleContactPoint = cushion.center - ball.getCenter();
                                //An early escape test, for if the ball isn't moving towards the cushion, there is no point in doing further collision checks between them
                                //if(Vector3.Dot(possibleContactPoint,ball.getVelocity())>0){
                                  bool collisionHappened =  CollisionResolutionBallCushion(ball,cushion);
                                 
                                 if(collisionHappened){
                                    //Calculate The Impulse between the cushion and the ball
                                    Vector3 impulseVector = sheet.CalculateImpulseVectorBallCushion(ball,cushion);

                                    //Apply the force to the ball
                                    ball.AddForce(impulseVector);
                                 }
                               // }

                    }   

                
                    for(int j=i+1;j<balls.Length;j++){

                        Ball A = balls[i];
                        Ball B = balls[j];
                        checkForCollisionBetweenBalls(A,B);
                    }
                        //Check for whether the ball is in on the plane or has fell in a pocket
                        if(hasBallFallenIntoAPocketOrGoneOutOfBounds(balls[i]))
                        {
                            balls[i].putOutOfAction();
                        }

                }

              


         }

    }

    float Distance(Vector3 A,Vector3 B){
        float distance = (A.x - B.x)*(A.x - B.x) + (A.y - B.y)*(A.y - B.y) + (A.z - B.z)*(A.z - B.z);
        return Mathf.Sqrt(distance);
    }    

    void checkForCollisionBetweenBalls(Ball A, Ball B){


        float radiusOfA = A.getRadius();
        float radiusOfB = B.getRadius();
        float sumOfRadii = radiusOfA+radiusOfB;

        sumOfRadii-= sumOfRadii * radiusCorrectionOffset;

        float distanceAB = Distance(A.getCenter(),B.getCenter());
        
        //Debug.Log("Sum of radii "+sumOfRadii+" distanceAB "+distanceAB);
        
        if(distanceAB<=sumOfRadii){
            //Debug.Log("Collision happened");
            collisionResolutionBall(A,B);
        }

    }

    void collisionResolutionBall(Ball A,Ball B){

        Ball faster = A.getVelocity().magnitude>B.getVelocity().magnitude?A:B;
        Ball slower = A.getVelocity().magnitude>B.getVelocity().magnitude?B:A;

        Vector3 impulseVector = sheet.CalculateImpulseVectorBall(faster,slower);

        //Assumption, it's going to be applied as it is to the faster ball and with it's direction inverted for the slower or at rest ball
        faster.AddForce(impulseVector);
        slower.AddForce(-impulseVector);
    }


    bool CollisionResolutionBallCushion(Ball ball, Cushion cushion){
           //Closest point to sphere center by clamping
           float x_closest =  Mathf.Max(cushion.MinX, Mathf.Min(ball.getCenter().x,cushion.MaxX));
           float y_closest =  Mathf.Max(cushion.MinY, Mathf.Min(ball.getCenter().y,cushion.MaxY));
           float z_closest =  Mathf.Max(cushion.MinZ, Mathf.Min(ball.getCenter().z,cushion.MaxZ));

           float distance = Mathf.Sqrt((x_closest - ball.getCenter().x)*(x_closest - ball.getCenter().x) +
                                       (y_closest - ball.getCenter().y)*(y_closest - ball.getCenter().y) +
                                       (z_closest - ball.getCenter().z)*(z_closest - ball.getCenter().z));

           return distance<ball.getRadius();                            
    }

    bool hasBallFallenIntoAPocketOrGoneOutOfBounds(Ball ball){

            Vector3 posOfBall = ball.transform.position;
            return posOfBall.x < minExtents.x || posOfBall.x > maxExtents.x;

    }

}
