using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.Serialization;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody _rb;
    public float forwardSpeed;
    public float maxSpeed;
    public float collisionForce;
    public GameManager gm;
    public Transform boxTransform;
    private bool _isJump;
    private bool _isReturn;
    private bool _isObstacle;
    
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (!IsMaxSpeed(maxSpeed) && gm.isStarted)
            _rb.AddForce(0, 0, forwardSpeed * Time.deltaTime);

        if (_isJump)
        {
            transform.position = Vector3.MoveTowards(transform.position,
                new Vector3(transform.position.x, 2f, transform.position.z), 2 * Time.fixedDeltaTime);
            if (transform.position.y == 2f)
            {
                _isJump = false;
                _isReturn = true;
            }

            if (_isObstacle)
                return;
            
            transform.rotation = Quaternion.RotateTowards(transform.rotation, 
                Quaternion.Euler(-180f, 0f ,0f), 250 * Time.fixedDeltaTime);
        }
        if (_isReturn)
        {
            transform.position = Vector3.MoveTowards(transform.position,
                new Vector3(transform.position.x, 1f, transform.position.z), 4 * Time.fixedDeltaTime);
            if (transform.position.y == 1f)
            {
                transform.rotation = Quaternion.Euler(Vector3.zero);
                _isObstacle = false;
                _isReturn = false;
            }
            
            if (_isObstacle)
                return;
            
            transform.Rotate(4f, 0f, 0f);
        }
    }

    public void StopMovement()
    {
        _rb.velocity = Vector3.zero;
        enabled = false;
    }

    public void DisableScript() => enabled = false;

    public void NullifyVelocity() => _rb.velocity = Vector3.zero;

    public void LowVelocity() => _rb.velocity = new Vector3(0f,0f,2f);

    public void SetFalling() => _rb.useGravity = true;

    public void Jump() => _isJump = true;

    public bool IsMaxSpeed(float maxSpeed) => _rb.velocity.magnitude >= maxSpeed;

    public void Obstacle()
    {
        _rb.AddForce(0f, 0f, collisionForce * Time.deltaTime, ForceMode.Impulse);
        Jump();
        _isObstacle = true;
    }

    public void SetStandartTransform()
    {
        transform.rotation = Quaternion.Euler(Vector3.zero);
        transform.position = new Vector3(transform.position.x, 1f, transform.position.z);
    }
}
