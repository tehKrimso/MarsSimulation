using UnityEngine;
using Vector2 = System.Numerics.Vector2;

namespace Infrastructure.Services.Map
{
    public class MapHandler : IService
    {
        private float _quadrantSize; //make dynamic?
        private GameObject _floor; //make array and fo by rows for each segment?

        private MapNode[,] _map; //make not square MapNode[][]
        private int mapSizeX;
        private int mapSizeY;
        public MapHandler(float quadrantSize, GameObject floorSegment)
        {
            _quadrantSize = quadrantSize;
            _floor = floorSegment;
            
            CalculateMapSize();
        }
        
        public MapNode[,] InitMap()
        {
            _map = new MapNode[mapSizeX,mapSizeY];
            return _map;
        }

        private void CalculateMapSize()
        {
            Vector3 floorCenter = _floor.transform.position;
            float floorWidth = _floor.transform.localScale.x * 10;  //default plane with scale 1 is 10 meters
            float floorHeight = _floor.transform.localScale.z * 10;

            mapSizeX = Mathf.FloorToInt(floorWidth / _quadrantSize);
            mapSizeY = Mathf.FloorToInt(floorHeight / _quadrantSize);
            
            Debug.Log($"Grid x:{mapSizeX}, GRid y: {mapSizeY}");
            
            Vector3 leftTopCorner = floorCenter + new Vector3(-floorWidth / 2, 0, floorHeight / 2);
            
            _map = new MapNode[mapSizeX,mapSizeY];

            for (int i = 0; i < mapSizeX; i++)
            {
                for (int j = 0; j < mapSizeY; j++)
                {
                    _map[i, j] = new MapNode()
                    {
                        CenterPoint = new Vector2(leftTopCorner.x, leftTopCorner.z) +
                                      new Vector2(i * (_quadrantSize / 2), -j * (_quadrantSize / 2)),
                        IsAccessible = true,
                        Size = _quadrantSize
                    };
                }
            }
        }
    }
    

    public struct MapNode
    {
        public float Size;
        public Vector2 CenterPoint;
        public bool IsAccessible;
        public float Elevation;
    }
}
