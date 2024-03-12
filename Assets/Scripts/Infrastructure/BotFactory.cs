using System.Collections.Generic;
using DefaultNamespace;
using Static;
using UnityEngine;

namespace Infrastructure
{
    public class BotFactory
    {
        private const string ColorPropertyName = "_Color";
        private readonly GameObject _botPrefab;
        private readonly List<Transform> _spawnPoints;

        public BotFactory(GameObject botPrefab, List<Transform> spawnPoints)
        {
            _botPrefab = botPrefab;
            _spawnPoints = spawnPoints;
        }

        public void SpawnBots(int count, GlobalPlanner planner)
        {
            for (int i = 0; i < count; i++)
            {
                GameObject bot = GameObject.Instantiate(_botPrefab, _spawnPoints[i].position, Quaternion.identity);
                bot.GetComponent<BotController>().Init(i,planner);
                MeshRenderer botRenderer = bot.GetComponent<MeshRenderer>();
                botRenderer.material.SetColor(ColorPropertyName, Colors.GetColor(i));
            }
        }
    }
}