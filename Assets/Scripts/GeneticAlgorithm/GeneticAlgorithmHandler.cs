using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace GeneticAlgorithm
{
    public class GeneticAlgorithmHandler : MonoBehaviour
    {
        public int IterationsCount;
        public float MutationChance;
        public float MutationRange;
        
        private List<Vector3> _referencePoints;


        public void Init(List<Vector3> referencePoints)
        {
            _referencePoints = referencePoints;
        }

        public List<Vector3> HandleTrajectory(Vector3 start, Vector3 end)
        {
            List<Vector3> trajectory = new List<Vector3>();

            Vector3 nextPoint = start;

            while (nextPoint != end)
            {
                foreach (Vector3 referencePoint in _referencePoints)
                {
                    //TODO make points matrix or class refpoints with position and indexes
                }
            }
            
            trajectory.Add(nextPoint);
            
            return trajectory;
        }

    }
}