using System.Collections.Generic;
using Behaviour;
using Infrastructure.Services;
using Infrastructure.Services.Planner;
using Static;
using UnityEngine;

namespace Infrastructure
{
    public class BotFactory : IService
    {
        private const string ColorPropertyName = "_Color";
        private readonly GameObject _botPrefab;
        private readonly List<Transform> _spawnPoints;
        private readonly GlobalPlanner _globalPlanner;

        public BotFactory(GameObject botPrefab, List<Transform> spawnPoints, GlobalPlanner globalPlanner)
        {
            _botPrefab = botPrefab;
            _spawnPoints = spawnPoints;
            _globalPlanner = globalPlanner;
        }
        
        public void SpawnBots(int count)
        {
            for (int i = 0; i < count; i++)
            {
                GameObject bot = GameObject.Instantiate(_botPrefab, _spawnPoints[i].position, Quaternion.identity);
                MeshRenderer botRenderer = bot.GetComponent<MeshRenderer>();
                botRenderer.material.SetColor(ColorPropertyName, Colors.GetColor(i));
                
                BotController botController = bot.GetComponent<BotController>();
                _globalPlanner.RegisterBot(botController); //mb register bot inside Init()????
                botController.Init(i,_globalPlanner);
            }
        }
    }
}