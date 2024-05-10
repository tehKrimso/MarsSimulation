using System.Collections.Generic;
using System.Linq;
using Behaviour;
using UnityEngine;

namespace Infrastructure.Services.Planner
{
    public class GlobalPlanner : IService
    {
        private readonly List<PointOfInterest> _freePointsOfInterest;
        private readonly BotFactory _factory;
        private readonly DebugProvider _debugProvider;
        private readonly List<PointOfInterest> _occupiedPointsOfInterest;

        private List<BotController> _bots;
        private Dictionary<BotController, List<TrajectoryPoint>> _trajectoriesByBot;

        private const float TrajectoryStepLength = 1f;

        private Collider[] _colliderBuffer;
        private const int ColliderBufferSize = 15;

        private const float CollisionAvoidanceTime = 2f;


        private LayerMask _collisionLayerMask;

        public GlobalPlanner(List<PointOfInterest> freePointsOfInterest, BotFactory factory, DebugProvider debugProvider)
        {
            
            _collisionLayerMask = LayerMask.GetMask(new string[]
            {
                //"Bot",
                "Obstacle",
                "TrajectoryPoint"
            });
            
            _freePointsOfInterest = freePointsOfInterest;
            _factory = factory;
            _debugProvider = debugProvider;
            _occupiedPointsOfInterest = new List<PointOfInterest>();

            _bots = new List<BotController>();
            _trajectoriesByBot = new Dictionary<BotController, List<TrajectoryPoint>>();

            _colliderBuffer = new Collider[ColliderBufferSize];
        }

        public void RegisterBot(BotController bot) //bots ask or planner ask every bot?
        {
            _bots.Add(bot);
            //TODO add check if already registered
        }

        public void CreateInitialTrajectories()
        {
            //plan for bot id 0
            SetInitialPath(0);
            //build trajectory
           

            for (int i = 1; i < _bots.Count; i++)//plan for others
            {
                SetInitialPath(i);
                CheckIntersectionsForBot(i);
                //build trajectory
            }

            //debug
            foreach (BotController bot in _bots)
            {
                List<TrajectoryPoint> trajectory = bot.GetTrajectory();
                TrajectoryPoint lastPoint = trajectory.Last();
                
                //bot.transform.LookAt(lastPoint.transform.position);
                
                var time = bot.GetTimeToReachPoint(lastPoint);
                var lengthWhole = Vector3.Distance(trajectory[0].transform.position, lastPoint.transform.position);
                var length = 0f;
                for (int i = 1; i < trajectory.Count; i++)
                {
                    length += Vector3.Distance(trajectory[i].transform.position, trajectory[i - 1].transform.position);
                }
                
                Debug.Log($"Bot {bot.Id}, LengthByPoints: {length}, LengthFromStartToEnd: {lengthWhole}, Time: {time}");
            }

        }

        private void SetInitialPath(int botId)
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
            List<TrajectoryPoint> trajectory = new List<TrajectoryPoint>();
            trajectory.Add(_factory.SpawnTrajectoryPoint(currentPoint,botId)); //add 0 point

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
                
                
                
                trajectory.Add(_factory.SpawnTrajectoryPoint(currentPoint,botId));
            }

            if (botId == 0)
            {
                bot.SetNewPath(trajectory,newDestinationPoint);
                _trajectoriesByBot[bot] = trajectory;
                return;
            }
            
            bot.SetNewPath(trajectory,newDestinationPoint);
            _trajectoriesByBot[bot] = trajectory;
        }

        private void CheckIntersectionsForBot(int botId)
        {
            BotController bot = _bots[botId];
            var botTrajectory = bot.GetTrajectory();

            for (int i = 0; i < botTrajectory.Count - 1; i++)
            {
                var point = botTrajectory[i];
                var hits = Physics.OverlapSphereNonAlloc(point.transform.position, 1f, _colliderBuffer, _collisionLayerMask);
                foreach (Collider hit in _colliderBuffer)
                {
                    if(hit == null)
                        continue;

                    if (hit.TryGetComponent(out TrajectoryPoint trajectoryPoint))
                    {
                        var trajectoryPointParentBotId = trajectoryPoint.parentBotId;
                        if(trajectoryPointParentBotId == botId)
                            continue;
                        
                        //detect trajectory point of other robot
                        float botTime = bot.GetTimeToReachPoint(point);
                        float otherBotTime = _bots[trajectoryPointParentBotId]
                            .GetTimeToReachPoint(trajectoryPoint); //change game object to trajectory point?

                        if (Mathf.Abs(botTime - otherBotTime) < CollisionAvoidanceTime)
                        {
                            point.isCollisionDetected = true;
                            
                            _debugProvider.Intersections.Add(point);
                            Debug.Log(
                                $"Intersection bot{botId}, time {botTime} with bot{trajectoryPointParentBotId}, time {otherBotTime} at {point.name}");
                        }

                    }
                }

                _colliderBuffer = new Collider[ColliderBufferSize];
            }
        }


        // private bool CheckIntersectionPoint(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4, out Vector3 intersectionPoint)
        // {
        //     intersectionPoint = Vector3.zero;
        //     var zn = (p4.z - p3.z) * (p2.x - p1.x) - (p4.x - p3.x) * (p2.z - p1.z);
        //
        //     if (zn < Mathf.Epsilon) //==0
        //     {
        //         return false; //lines parallel
        //     }
        //     
        //     var u1 = (p4.x - p3.x) * (p1.z - p3.z) - (p4.z - p3.z) * (p1.x - p3.x);
        //     var u2 = (p2.x - p1.x) * (p1.z - p3.z) - (p2.z = p1.z) * (p1.x - p3.x);
        //
        //     var ua = u1 / zn;
        //     var ub = u2 / zn;
        //
        //     if (ua >= 0 && ua <= 1 && ub >= 0 && ub <= 1)
        //     {
        //         intersectionPoint = new Vector3(p1.x + ua * (p2.x - p1.x), 0,p1.z + ua * (p2.z - p1.z));
        //         Intersections.Add(intersectionPoint);
        //         Debug.Log("Intersection found");
        //         return true;
        //     }
        //
        //     
        //     return false;
        // }
    }
}