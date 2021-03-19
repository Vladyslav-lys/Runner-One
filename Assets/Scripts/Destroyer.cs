using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyer : MonoBehaviour
{
    public Transform targetTransform;
    public Vector3 offset;
    
    private void OnTriggerEnter(Collider other)
    {
        Destroy(other.gameObject);
    }

    private void LateUpdate()
    {
        transform.position = targetTransform.position + offset;
    }
}
