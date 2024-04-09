using System.Collections.Generic;
using System.Linq;
using Behaviour;
using UnityEngine;

namespace Infrastructure.Services.Planner
{
    public class GlobalPlanner : IService
    {
        private readonly List<PointOfInterest> _freePointsOfInterest;
        private readonly List<PointOfInterest> _occupiedPointsOfInterest;

        private List<BotController> _bots;
        private Dictionary<BotController, List<Vector3>> _trajectoriesByBot;

        public GlobalPlanner(List<PointOfInterest> freePointsOfInterest)
        {
            _freePointsOfInterest = freePointsOfInterest;
            _occupiedPointsOfInterest = new List<PointOfInterest>();

            _bots = new List<BotController>();
            _trajectoriesByBot = new Dictionary<BotController, List<Vector3>>();
        }

        public void RegisterBot(BotController bot) => _bots.Add(bot); //TODO add check if already registered

        public void CreateInitialTrajectories()
        {
            //plan for bot id 0
            GetDestinationPoint(0);
            //build trajectory
           

            for (int i = 1; i < _bots.Count; i++)//plan for others
            {
                GetDestinationPoint(i);
                //build trajectory
            }

        }

        private void GetDestinationPoint(int botId)
        {
            PointOfInterest newDestinationPoint = _freePointsOfInterest[Random.Range(0, _freePointsOfInterest.Count)];
            
            //set destination point? not whole pass TODO
            _bots[botId].SetNewPath(
                new List<Vector3>()
                {
                    _bots[botId].transform.position,
                    newDestinationPoint.transform.position
                },
                newDestinationPoint
            );

            newDestinationPoint.IsOccupied = true;
            _freePointsOfInterest.Remove(newDestinationPoint);
            _occupiedPointsOfInterest.Add(newDestinationPoint);
            
            //build trajectrory???
        }
    }
}