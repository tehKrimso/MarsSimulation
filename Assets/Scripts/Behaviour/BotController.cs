using System.Collections.Generic;
using System.Linq;
using Infrastructure.Services.Planner;
using Static;
using UnityEngine;
using UnityEngine.Serialization;

namespace Behaviour
{
    public class BotController : MonoBehaviour
    {
        public float LinearMoveSpeed = 1f;
        public float AngularMoveSpeed = 1f; //rad/s?

        public int Id => _id;

        private int _id;

        private PointOfInterest _currentDestination;
        private Rigidbody _rb;

        private GlobalPlanner _planner;

        private List<GameObject> _trajectoryPoints;

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

        public List<GameObject> GetTrajectory() => _trajectoryPoints;

        public void SetNewPath(List<GameObject> trajectory, PointOfInterest destination)
        {
            _trajectoryPoints = trajectory;
            _currentDestination = destination;

            nextPointIndex = 1;
        }

        public float GetTimeToReachPoint(GameObject point)
        {
            float time = 0f;

            int pointIndex = _trajectoryPoints.IndexOf(point); //передавать?

            time += (_trajectoryPoints[nextPointIndex].transform.position - transform.position).magnitude /
                    LinearMoveSpeed;
            
            time += Vector3.Angle(transform.forward,
                (_trajectoryPoints[nextPointIndex].transform.position - transform.position)) * Mathf.Deg2Rad / AngularMoveSpeed; //mathf.deg2rad / angular == можно константой
            
            for(int i= nextPointIndex+1; i<pointIndex; i++)
            {
                var angle = Vector3.Angle(transform.forward,
                    (_trajectoryPoints[i].transform.position - _trajectoryPoints[i-1].transform.position));

                time += angle * Mathf.Deg2Rad / AngularMoveSpeed;

                time += (_trajectoryPoints[i].transform.position - _trajectoryPoints[i - 1].transform.position)
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
        
            //Move();
            DrawPath();
        }

        private void Move()
        {
            Vector3 direction = (_currentDestination.transform.position - transform.position).normalized;

            float angle = Vector3.SignedAngle(transform.forward, (_currentDestination.transform.position - transform.position), Vector3.up);

            if (Mathf.Abs(angle) < 1)
            {
                transform.LookAt(_currentDestination.transform);
                _rb.MovePosition(transform.position + direction * (LinearMoveSpeed * Time.fixedDeltaTime));
                return;
            }
            
            if (angle > 1f)
            {
                transform.RotateAround(transform.position, Vector3.up, 1f);
                return;
            }

            if(angle < 1f)
            {
                transform.RotateAround(transform.position, Vector3.up, -1f);
            }
            
        }

        private void DrawPath()
        {
            _lineRenderer.positionCount = _trajectoryPoints.Count;
            _lineRenderer.SetPositions(_trajectoryPoints.Select(p =>p.transform.position).ToArray());
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

            foreach (GameObject point in _trajectoryPoints)
            {
                Gizmos.DrawSphere(point.transform.position, 0.1f);
            }

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
}
