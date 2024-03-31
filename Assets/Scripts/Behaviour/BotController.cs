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

        public void Init(int id)
        {
            _id = id;

            _rb = GetComponent<Rigidbody>();

            SetUpLineRenderer();
        
            SetDestination();
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
        }

        private void Move()
        {
            Vector3 direction = (_currentDestination.transform.position - transform.position).normalized;
        
            _rb.MovePosition(transform.position + direction * (MoveSpeed * Time.fixedDeltaTime));

            DrawPath();
        }

        private void DrawPath()
        {
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
}