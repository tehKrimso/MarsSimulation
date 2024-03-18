using System.Collections.Generic;
using GeneticAlgorithm;
using Unity.VisualScripting;
using UnityEngine;

namespace DefaultNamespace
{
    public class GlobalPlanner
    {
        private List<Vector3> _referencePoints;
        
        private List<PointOfInterest> _pointsOfInterest;
        private readonly GeneticAlgorithmHandler _gaHandler;
        private Dictionary<int,BotController> _botControllers;

        public GlobalPlanner(List<Vector3> referencePoints, List<PointOfInterest> points, GeneticAlgorithmHandler gaHandler)
        {
            _referencePoints = referencePoints;
            
            _pointsOfInterest = points;
            _gaHandler = gaHandler;
            _botControllers = new Dictionary<int, BotController>();
        }

        public void RegisterBot(BotController bot) => _botControllers.Add(bot.Id, bot);

        public PointOfInterest GetNewDestinationPoint() =>
            _pointsOfInterest[Random.Range(0, _pointsOfInterest.Count - 1)];


        public List<Vector3> GetPathToDestination(BotController bot,PointOfInterest destinationPoint)
        {
            List<Vector3> trajectory = new List<Vector3>();

            Vector3 startPoint = bot.transform.position;
            Vector3 endPoint = destinationPoint.transform.position;


            return _gaHandler.HandleTrajectory(startPoint,endPoint);
        }
    }
}