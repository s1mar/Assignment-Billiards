using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CueStick : MonoBehaviour
{
    public Ball cueBall;
    public float forceMagnitude = 100.0F;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
          if (Input.GetButtonDown("Fire1")){
                    Vector3 pseudoImpulse = new Vector3(forceMagnitude,0,0);
                    cueBall.AddForce(pseudoImpulse);
          }
        
    }
}
