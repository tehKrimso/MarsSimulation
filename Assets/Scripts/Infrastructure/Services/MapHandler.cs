using UnityEngine;
using Vector2 = System.Numerics.Vector2;

namespace Infrastructure.Services
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
            
            for (int i = 0; i < mapSizeX; i++)
            {
                for (int j = 0; j < mapSizeY; j++)
                {
                    var start = new Vector3(_map[i, j].CenterPoint.X, 0, _map[i, j].CenterPoint.Y);
                    var end = new Vector3(_map[i, j].CenterPoint.X, 5, _map[i, j].CenterPoint.Y);
                    Debug.DrawLine(start,end,Color.cyan,10f);
                }
            }
            
            //debug
            // Vector3 leftTopCorner = floorCenter + new Vector3(-floorWidth / 2, 0, floorHeight / 2);
            // Vector3 rightTopCorner = floorCenter + new Vector3(floorWidth / 2, 0, floorHeight / 2);
            // Vector3 leftBottomCorner = floorCenter + new Vector3(-floorWidth / 2, 0, -floorHeight / 2);
            // Vector3 rightBottomCorner = floorCenter + new Vector3(floorWidth / 2, 0, -floorHeight / 2);
            //
            // Debug.DrawLine(leftTopCorner,new Vector3(leftTopCorner.x,5,leftTopCorner.z),Color.red,10f);
            // Debug.DrawLine(rightTopCorner,new Vector3(rightTopCorner.x,5,rightTopCorner.z),Color.blue,10f);
            // Debug.DrawLine(leftBottomCorner,new Vector3(leftBottomCorner.x,5,leftBottomCorner.z),Color.green,10f);
            // Debug.DrawLine(rightBottomCorner,new Vector3(rightBottomCorner.x,5,rightBottomCorner.z),Color.yellow,10f);
            //
            // for (int i = 0; i < mapSizeX; i++)
            // {
            //     for (int j = 0; j < mapSizeY; j++)
            //     {
            //         Vector3 startPoint = new Vector3(leftTopCorner.x + i * _quadrantSize, 0,
            //             leftTopCorner.z - j * +_quadrantSize);
            //         
            //         Vector3 finishPoint = new Vector3(leftTopCorner.x + i * _quadrantSize, 5,
            //             leftTopCorner.z - j * +_quadrantSize);
            //         Debug.DrawLine(startPoint,finishPoint,Color.magenta,10f);
            //     }
            // }
            
            
        }
    }
    

    public struct MapNode
    {
        public float Size;
        public Vector2 CenterPoint;
        public bool IsAccessible;
    }
}
