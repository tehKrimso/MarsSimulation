using System.Collections.Generic;
using Infrastructure.Services.Planner;
using Static;
using UnityEngine;

namespace Behaviour
{
    public class BotController : MonoBehaviour
    {
        public float MoveSpeed = 1f;

        public int Id => _id;

        private int _id;

        private PointOfInterest _currentDestination;
        private Rigidbody _rb;

        private GlobalPlanner _planner;

        private List<Vector3> _trajectoryPoints;

        private LineRenderer _lineRenderer;

        public void Init(int id, GlobalPlanner planner)
        {
            _id = id;
            _planner = planner;

            _rb = GetComponent<Rigidbody>();

            SetUpLineRenderer();
        
            //SetDestination();
        }

        public void SetNewPath(List<Vector3> trajectory, PointOfInterest destination)
        {
            _trajectoryPoints = trajectory;
            _currentDestination = destination;
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
        
            _rb.MovePosition(transform.position + direction * (MoveSpeed * Time.fixedDeltaTime));
        }

        private void DrawPath()
        {
            _lineRenderer.positionCount = _trajectoryPoints.Count;
            _lineRenderer.SetPositions(_trajectoryPoints.ToArray());
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

            foreach (Vector3 point in _trajectoryPoints)
            {
                Gizmos.DrawSphere(point, 0.1f);
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
