using System;
using System.Collections.Generic;
using Behaviour;
using Infrastructure.Services;
using Infrastructure.Services.Map;
using UnityEngine;

namespace Infrastructure
{
    public class Bootstrapper : MonoBehaviour
    {
        [Header("Map Settings")] 
        public GameObject Floor;
        public float QuadrantSize;

        [Header("Bot settings")] 
        public GameObject BotPrefab;

        public List<Transform> BotSpawnPoints;
        public int BotCount;

        public List<PointOfInterest> PointsOfInterest;


        private BotFactory _botFactory;
        private MapHandler _mapHandler;

        private ServiceLocator _container;


        private void Awake()
        {
            _container = ServiceLocator.Container;
            
            RegisterServices();
            InitWorld();
        }

        private void RegisterServices()
        {
            _botFactory = new BotFactory(BotPrefab, BotSpawnPoints);
            _container.RegisterSingle<BotFactory>(_botFactory);

            _mapHandler = new MapHandler(QuadrantSize, Floor);
            _container.RegisterSingle<MapHandler>(_mapHandler);
        }

        private void InitWorld()
        {
            _botFactory.SpawnBots(BotCount);
        }
    }
}