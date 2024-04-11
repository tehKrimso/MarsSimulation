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

        private const float TrajectoryStepLength = 1f;

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

            BotController bot = _bots[botId];
            Vector3 botPos = bot.transform.position;
            Vector3 destinationPos = newDestinationPoint.transform.position;

            newDestinationPoint.IsOccupied = true;
            _freePointsOfInterest.Remove(newDestinationPoint);
            _occupiedPointsOfInterest.Add(newDestinationPoint);
            
            //build straight line trajectory with points
            Vector3 destinationDir = (destinationPos - botPos).normalized;
            
            Vector3 currentPoint = botPos;
            List<Vector3> trajectory = new List<Vector3>();

            while (currentPoint != destinationPos)
            {
                if (Vector3.Distance(currentPoint, destinationPos) < TrajectoryStepLength)
                {
                    currentPoint = destinationPos;
                }
                else
                {
                    currentPoint += destinationDir * TrajectoryStepLength;
                }
                
                trajectory.Add(currentPoint);
            }
            
            bot.SetNewPath(trajectory,newDestinationPoint);
            _trajectoriesByBot[bot] = trajectory;

            //todo check trajectories cross
            // for (int i = 0; i < botId; i++)
            // {
            //     //check if trajectories cross
            // }

        }
    }
}