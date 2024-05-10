using System;
using System.Collections.Generic;
using Behaviour;
using UnityEngine;

namespace Infrastructure.Services
{
    
    public class DebugProvider : MonoBehaviour, IService
    {
        public bool DrawDebug;
        
        private readonly Color _collisionColor = new Color(1,0,0,0.2f);
        
        //debug data lists
        public List<TrajectoryPoint> Intersections;

        public void Init()
        {
            Intersections = new List<TrajectoryPoint>();
        }

        private void OnDrawGizmos()
        {
            if(!DrawDebug)
                return;
            
            DrawIntersections();
        }

        private void DrawIntersections()
        {
            Gizmos.color = _collisionColor;
            foreach (TrajectoryPoint trajectoryPoint in Intersections)
            {
                Gizmos.DrawSphere(trajectoryPoint.transform.position, 1f);
            }
        }
    }
}