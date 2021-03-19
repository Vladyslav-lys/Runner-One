using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public GameManager gm;
    public Transform targetTransform;
    private float _coinSpeed;

    private void OnEnable()
    {
        _coinSpeed = gm.playerController.playerMovement.maxSpeed + 5f;
    }

    private void Update()
    {
        if (gm && targetTransform && gm.isMagnetting && Math.Abs(targetTransform.position.z - transform.position.z) < 6f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetTransform.position, _coinSpeed * Time.deltaTime);
            if (TimeSpan.FromTicks(DateTime.Now.Ticks - gm.magnettingTimeTicks).Seconds > gm.needMagnettingTime)
            {
                gm.isMagnetting = false;
                gm.playerController.magnetEffect?.SetActive(false);
            }
        }
    }
}
