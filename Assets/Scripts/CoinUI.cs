using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinUI : MonoBehaviour
{
    public Transform targetTransform;
    
    private void FixedUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetTransform.position, 1f*Time.deltaTime);
        if (transform.position == targetTransform.position)
        {   
            Destroy(gameObject);
        }
    }
}
