using System;
using System.Collections.Generic;
using Infrastructure.Services;
using UnityEngine;

namespace Infrastructure
{
    public class Bootstrapper : MonoBehaviour
    {
        [Header("Map Settings")] 
        public GameObject Floor;
        public float QuadrantSize;
        
        
        private MapHandler _mapHandler;


        private void Awake()
        {
            RegisterServices();
            InitWorld();
        }

        private void RegisterServices()
        {
            ServiceLocator.Container.RegisterSingle<MapHandler>(new MapHandler(QuadrantSize,Floor));
        }

        private void InitWorld()
        {
            _mapHandler = ServiceLocator.Container.Single<MapHandler>();
        }
    }
}