using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using Static;
using UnityEngine;
using Random = UnityEngine.Random;

public class BotController : MonoBehaviour
{
    public float MoveSpeed = 1f;

    public int Id => _id;

    private int _id;

    private PointOfInterest _currentDestination;
    
    private CharacterController _controller;

    private Rigidbody _rb;

    private GlobalPlanner _planner;

    public void Init(int id, GlobalPlanner planner)
    {
        _id = id;
        
        _planner = planner;
        _planner.RegisterBot(this);
        
        _controller = GetComponent<CharacterController>();

        _rb = GetComponent<Rigidbody>();
        
        SetDestination();
    }

    private void FixedUpdate()
    {
        if (_currentDestination == null)
            return;
        
        Move();
    }

    private void Move()
    {
        //TODO: remove character controller and rewrite?
        //_controller.Move(_currentDirection * (MoveSpeed * Time.fixedDeltaTime));

        Vector3 direction = (_currentDestination.transform.position - transform.position).normalized;
        
        _rb.MovePosition(transform.position + direction * (MoveSpeed * Time.fixedDeltaTime));
    }

    private void SetDestination()
    {
        PointOfInterest newDestination;

        do
        {
            newDestination = _planner.GetNewDestinationPoint();
        } while (newDestination == _currentDestination);

        _currentDestination = newDestination;
        
        //debug
       
        Debug.Log($"Agent #{_id} set course to {_currentDestination.name}");
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Colors.GetColor(_id);
        Gizmos.DrawLine(transform.position,_currentDestination.transform.position);
    }

    //TODO: move to point of interest script??
    private void OnTriggerEnter(Collider other)
    {
        PointOfInterest point = other.GetComponent<PointOfInterest>();

        if (point == null)
            return;
        
        if(point == _currentDestination)
            SetDestination();
    }
}
