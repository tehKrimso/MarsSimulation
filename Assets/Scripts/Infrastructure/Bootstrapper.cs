using System;
using System.Collections.Generic;
using Behaviour;
using Infrastructure.Services;
using Infrastructure.Services.Map;
using Infrastructure.Services.Planner;
using UnityEngine;

namespace Infrastructure
{
    public class Bootstrapper : MonoBehaviour
    {
        // [Header("Map Settings")] 
        // public GameObject Floor;
        // public float QuadrantSize;

        [Range(1f,15f)]
        public float TimeScale;

        [Header("Bot settings")] 
        public GameObject BotPrefab;

        public GameObject TrajectoryPointPrefab;

        public List<Transform> BotSpawnPoints;
        public int BotCount;

        public List<PointOfInterest> PointsOfInterest;

        [Header("Debug")] 
        public DebugProvider DebugProvider;

        //private MapHandler _mapHandler;
        private GlobalPlanner _planner;
        private BotFactory _botFactory;

        private ServiceLocator _container;


        private void Awake()
        {
            _container = ServiceLocator.Container;
            
            RegisterServices();
            InitWorld();
        }

        private void RegisterServices()
        {
            _container.RegisterSingle<DebugProvider>(DebugProvider);
            
            _botFactory = new BotFactory(BotPrefab, TrajectoryPointPrefab, BotSpawnPoints);
            _container.RegisterSingle<BotFactory>(_botFactory);
            
            _planner = new GlobalPlanner(PointsOfInterest, _botFactory, DebugProvider);
            _container.RegisterSingle<GlobalPlanner>(_planner);
        }

        private void Update()
        {
            Time.timeScale = TimeScale;
        }

        private void InitWorld()
        {
            //_botFactory.SpawnBots(BotCount);

            for (int i = 0; i < BotCount; i++)
            {
                _planner.RegisterBot(_botFactory.SpawnBot(i));
            }
            
            _planner.CreateInitialTrajectories();
        }

        //debug
        // private void OnDrawGizmos()
        // {
        //     if (_planner?.Intersections == null)
        //     {
        //         return;
        //     }
        //
        //     var collisionColor = new Color(1,0,0,0.25f);
        //     foreach (TrajectoryPoint intersection in _planner.Intersections)
        //     {
        //         if (intersection.isCollisionDetected)
        //             Gizmos.color = collisionColor;
        //         else
        //         {
        //             Gizmos.color = Color.magenta;
        //         }
        //         Gizmos.DrawSphere(intersection.transform.position,1f);
        //     }
        // }
    }
}