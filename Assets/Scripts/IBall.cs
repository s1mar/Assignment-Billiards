using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IBall : MonoBehaviour
{   

    public float radius;
    Renderer rend;
public float getRadius(){
       return radius;
    }

    public Vector3 getCenter(){
        return rend.bounds.center;
    }
    void Start(){
        rend = GetComponent<Renderer>();
        radius = rend.bounds.extents.magnitude;
        
    }

    
}
