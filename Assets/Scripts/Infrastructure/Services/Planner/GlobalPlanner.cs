using System.Collections.Generic;
using System.Linq;
using Behaviour;
using UnityEngine;

namespace Infrastructure.Services.Planner
{
    public class GlobalPlanner : IService
    {
        private readonly List<PointOfInterest> _pointsOfInterest;

        private List<BotController> _bots;
        
        public GlobalPlanner(List<PointOfInterest> pointsOfInterest)
        {
            _pointsOfInterest = pointsOfInterest;

            _bots = new List<BotController>();
        }

        public void RegisterBot(BotController bot) => _bots.Add(bot); //TODO add check if already registered

        public void CreateInitialTrajectories()
        {
            foreach (BotController bot in _bots)
            {
                List<PointOfInterest> freePoints = _pointsOfInterest.Where(p => !p.IsOccupied).ToList();
                PointOfInterest initialDestination = freePoints[Random.Range(0, freePoints.Count)];

                
                //ga handler

                Vector3 startPoint = bot.transform.position;
                bot.SetNewPath(
                    new List<Vector3>()
                    {
                        startPoint,
                        initialDestination.transform.position
                    },
                    initialDestination
                    );

                initialDestination.IsOccupied = true;
            }
        }
    }
}