using System.Collections.Generic;
using UnityEngine;
using Vector2 = System.Numerics.Vector2;

namespace Infrastructure.Services.Map
{
    public class MapHandler : IService
    {
        private float _mapSize;
        
        public MapHandler(float mapSize)
        {
            _mapSize = mapSize;
        }
    }

    public class GlobalMap
    {
        public float Size;
        //public List<Obstacle> Obstacles;
    }

    public class Obstacle
    {
    }

    public struct MapNode
    {
        public float Size;
        public Vector2 CenterPoint;
        public bool IsAccessible;
        public float Elevation;
    }
}
