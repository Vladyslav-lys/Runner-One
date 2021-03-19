using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetCamera : MonoBehaviour
{
    public Transform target;
    public GameManager gm;
    public Vector3 offset;
    
    void LateUpdate()
    {
        if (gm.currentPow != gm.needPow)
        {
            transform.position = new Vector3(transform.position.x, (target.position + offset).y, (target.position + offset).z);
        }
        if (gm.currentPow == gm.needPow)
        {
            transform.RotateAround(target.position, Vector3.up, 5f*Time.deltaTime);
        }
    }
}