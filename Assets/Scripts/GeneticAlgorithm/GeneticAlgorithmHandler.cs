using System.Collections.Generic;
using System.Linq;
using Behaviour;
using Infrastructure.Services;
using Unity.VisualScripting;
using UnityEngine;

namespace GeneticAlgorithm
{
    public class GeneticAlgorithmHandler : MonoBehaviour, IService
    {
        public int IterationsCount;
        public float MutationChance;
        public float MutationRange;

        public int CrossingoverRange = 1;

        public void Init()
        {
            
        }

        public void MutateTrajectory(List<TrajectoryPoint> trajectoryToChange)
        {
            List<int> intersectionsPointIndex = new List<int>();

            for (int i = 0; i < trajectoryToChange.Count; i++)
            {
                if(trajectoryToChange[i].isCollisionDetected)
                    intersectionsPointIndex.Add(i);
            }

            int firstIndToMutate = intersectionsPointIndex[0] - 1 < 0
                ? intersectionsPointIndex[0]
                : intersectionsPointIndex[0] - 1;
            int lastIndToMutate = intersectionsPointIndex.Last() + 1 > trajectoryToChange.Count
                ? intersectionsPointIndex.Last()
                : intersectionsPointIndex.Last() + 1;
            
            intersectionsPointIndex.Insert(0,firstIndToMutate);
            intersectionsPointIndex.Add(lastIndToMutate);

            var trajectoriesPopulation = new List<Vector3>[IterationsCount];
            var trajectoriesFitness = new float[IterationsCount];

            for (int i = 0; i < IterationsCount; i++)
            {
                var newTrajectory = new List<Vector3>();
                trajectoryToChange.Select(p=>p.transform.position).ToList().CopyTo(newTrajectory.ToArray());
                
                foreach (int index in intersectionsPointIndex)
                {
                    float chance = Random.Range(0, 100);
                    if (chance > MutationChance)
                    {
                        newTrajectory[index] = new Vector3(
                            newTrajectory[index].x + Random.Range(-MutationRange, MutationRange),
                            newTrajectory[index].y,
                            newTrajectory[index].z + Random.Range(-MutationRange, MutationRange));
                    }
                }

                trajectoriesPopulation[i] = newTrajectory;
            }

        }
    }
}