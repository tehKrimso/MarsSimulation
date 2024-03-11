using System;
using System.Collections;
using System.Collections.Generic;
using Static;
using UnityEngine;
using Random = UnityEngine.Random;

public class BotController : MonoBehaviour
{
    public float MoveSpeed = 1f;
    
    private int _id;

    private Vector3 _currentDirection;
    private CharacterController _controller;
    
    public void Init(int id)
    {
        _id = id;
        _controller = GetComponent<CharacterController>();
        
        SetDestination();
    }

    private void FixedUpdate()
    {
        if (_currentDirection == Vector3.zero)
            return;
        
        Move();
    }

    private void Move()
    {
        _controller.Move(_currentDirection * (MoveSpeed * Time.fixedDeltaTime));
    }

    private void SetDestination()
    {
        _currentDirection = Random.insideUnitSphere.normalized;
        _currentDirection.y = 0;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Colors.GetColor(_id);
        Gizmos.DrawLine(transform.position,transform.position + _currentDirection*MoveSpeed);
    }
}
