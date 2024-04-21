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

        [Header("Bot settings")] 
        public GameObject BotPrefab;

        public List<Transform> BotSpawnPoints;
        public int BotCount;

        public List<PointOfInterest> PointsOfInterest;


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
            _planner = new GlobalPlanner(PointsOfInterest);
            _container.RegisterSingle<GlobalPlanner>(_planner);
            
            _botFactory = new BotFactory(BotPrefab, BotSpawnPoints, _planner);
            _container.RegisterSingle<BotFactory>(_botFactory);
        }

        private void InitWorld()
        {
            _botFactory.SpawnBots(BotCount);
            _planner.CreateInitialTrajectories();
        }

        //debug
        private void OnDrawGizmos()
        {
            if (_planner.Intersections == null)
            {
                return;
            }

            Gizmos.color = Color.magenta;
            foreach (Vector3 intersection in _planner.Intersections)
            {
                Gizmos.DrawSphere(intersection,0.5f);
            }
        }
    }
}