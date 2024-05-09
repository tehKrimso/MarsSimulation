using System;
using System.Collections.Generic;
using System.Linq;
using Infrastructure.Services;
using Infrastructure.Services.Planner;
using Static;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Serialization;

namespace Behaviour
{
    public class BotController : MonoBehaviour
    {
        public float LinearMoveSpeed = 1f;
        public float AngularMoveSpeed = 1f; //rad/s?

        public int Id => _id;

        public List<TrajectoryPoint> trajectoryPoints;
        
        private int _id;

        private PointOfInterest _currentDestination;
        private Rigidbody _rb;


        private GlobalPlanner _planner;
        private LineRenderer _lineRenderer;

        private int nextPointIndex = 1;

        public void Init(int id)
        {
            _id = id;
            //_planner = planner;

            _rb = GetComponent<Rigidbody>();

            SetUpLineRenderer();
        
            //SetDestination();
        }

        public List<TrajectoryPoint> GetTrajectory() => trajectoryPoints;

        public void SetNewPath(List<TrajectoryPoint> trajectory, PointOfInterest destination)
        {
            trajectoryPoints = trajectory;
            _currentDestination = destination;

            nextPointIndex = 1;
        }

        public float GetTimeToReachPoint(TrajectoryPoint point)
        {
            float time = 0f;

            int pointIndex = trajectoryPoints.IndexOf(point); //передавать?

            time += (trajectoryPoints[nextPointIndex].transform.position - transform.position).magnitude /
                    LinearMoveSpeed;
            
            time += Vector3.Angle(transform.forward,
                (trajectoryPoints[nextPointIndex].transform.position - transform.position)) * Mathf.Deg2Rad / AngularMoveSpeed; //mathf.deg2rad / angular == можно константой
            
            for(int i= nextPointIndex+1; i<pointIndex; i++)
            {
                var angle = Vector3.Angle(transform.forward,
                    (trajectoryPoints[i].transform.position - trajectoryPoints[i-1].transform.position));

                time += angle * Mathf.Deg2Rad / AngularMoveSpeed;

                time += (trajectoryPoints[i].transform.position - trajectoryPoints[i - 1].transform.position)
                    .magnitude / LinearMoveSpeed;
            }

            return time;
        }

        private void SetUpLineRenderer()
        {
            _lineRenderer = GetComponent<LineRenderer>();
        
            _lineRenderer.material.SetColor("_Color", Colors.GetColor(_id));
            _lineRenderer.startWidth = _lineRenderer.endWidth = 0.2f;
        }

        private void FixedUpdate()
        {
            if (_currentDestination == null)
                return;
        
            Move();
            DrawPath();
        }

        private void Move()
        {
            Vector3 direction = (_currentDestination.transform.position - transform.position).normalized;

            float angle = Vector3.SignedAngle(transform.forward, (_currentDestination.transform.position - transform.position), Vector3.up);
            
            //Debug.DrawLine(transform.position, transform.position + transform.forward * 2f, Color.red);
            //Debug.DrawLine(transform.position, _currentDestination.transform.position, Color.magenta);
            
            if (Mathf.Abs(angle) < 1.5f)
            {
                _rb.angularVelocity = Vector3.zero;
                transform.LookAt(_currentDestination.transform);
                _rb.MovePosition(transform.position + direction * (LinearMoveSpeed * Time.fixedDeltaTime));
                return;
            }
            
            if (angle > 0f)
            {
                Quaternion deltaRotation = Quaternion.Euler(0, AngularMoveSpeed, 0);
                _rb.MoveRotation(_rb.rotation * deltaRotation);
                return;
            }

            if(angle < 0f)
            {
                Quaternion deltaRotation = Quaternion.Euler(0, -AngularMoveSpeed, 0);
                _rb.MoveRotation(_rb.rotation * deltaRotation);
            }
            
        }

        private void DrawPath()
        {
            _lineRenderer.positionCount = trajectoryPoints.Count;
            _lineRenderer.SetPositions(trajectoryPoints.Select(p =>p.transform.position).ToArray());
        }

        private void SetDestination()
        {
            PointOfInterest newDestination;
        
            //debug
            Debug.Log($"Agent #{_id} set course to {_currentDestination.name}");
            
        }

        private void OnDrawGizmos()
        {
            if (_currentDestination == null)
                return;

            Gizmos.color = Color.black;

            foreach (TrajectoryPoint point in trajectoryPoints)
            {
                Gizmos.DrawSphere(point.transform.position, 0.1f);
            }

        }

        //TODO: move to point of interest script??
        // private void OnTriggerEnter(Collider other)
        // {
        //     PointOfInterest point = other.GetComponent<PointOfInterest>();
        //
        //     if (point == null)
        //         return;
        //
        //     // if(point == _currentDestination)
        //     //     SetDestination();
        // }

        private void OnCollisionEnter(Collision other)
        {
            // if (other.gameObject.layer == LayerMask.NameToLayer("Bot"))
            // {
            //     LinearMoveSpeed = 0f;
            //     Debug.LogWarning($"CollisionHappened at {gameObject.transform.position} bot {Id}");
            // }
        }
    }
}
