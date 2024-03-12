using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    public class GlobalPlanner
    {
        private List<PointOfInterest> _pointsOfInterest;
        private Dictionary<int,BotController> _botControllers;

        public GlobalPlanner(List<PointOfInterest> points)
        {
            _pointsOfInterest = points;
            _botControllers = new Dictionary<int, BotController>();
        }

        public void RegisterBot(BotController bot) => _botControllers.Add(bot.Id, bot);

        public PointOfInterest GetNewDestinationPoint() =>
            _pointsOfInterest[Random.Range(0, _pointsOfInterest.Count - 1)];
    }
}