using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace DefaultNamespace
{
    public class GlobalPlanner
    {
        private List<Vector3> _referencePoints;
        
        private List<PointOfInterest> _pointsOfInterest;
        private Dictionary<int,BotController> _botControllers;

        public GlobalPlanner(List<Vector3> referencePoints, List<PointOfInterest> points)
        {
            _referencePoints = referencePoints;
            
            _pointsOfInterest = points;
            _botControllers = new Dictionary<int, BotController>();
        }

        public void RegisterBot(BotController bot) => _botControllers.Add(bot.Id, bot);

        public PointOfInterest GetNewDestinationPoint() =>
            _pointsOfInterest[Random.Range(0, _pointsOfInterest.Count - 1)];

        
    }
}