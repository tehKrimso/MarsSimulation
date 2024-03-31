using System.Collections.Generic;
using Infrastructure.Services;
using Static;
using UnityEngine;

namespace Infrastructure
{
    public class BotFactory : IService
    {
        private const string ColorPropertyName = "_Color";
        private readonly GameObject _botPrefab;
        private readonly List<Transform> _spawnPoints;

        public BotFactory(GameObject botPrefab, List<Transform> spawnPoints)
        {
            _botPrefab = botPrefab;
            _spawnPoints = spawnPoints;
        }
        
        public void SpawnBots(int count)
        {
            for (int i = 0; i < count; i++)
            {
                GameObject bot = GameObject.Instantiate(_botPrefab, _spawnPoints[i].position, Quaternion.identity);
                MeshRenderer botRenderer = bot.GetComponent<MeshRenderer>();
                botRenderer.material.SetColor(ColorPropertyName, Colors.GetColor(i));
            }
        }
    }
}