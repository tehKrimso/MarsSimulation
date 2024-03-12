using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using Infrastructure;
using Static;
using UnityEngine;

public class EntryPoint : MonoBehaviour
{
    public List<Transform> SpawnPoints;
    public List<PointOfInterest> PointsOfInterest;

    public GameObject BotPrefab;

    [Space, Header("Settings")] public int BotCount;


    private GlobalPlanner _planner;
    private BotFactory _botFactory;
    

    private void Awake()
    {
        InitializeInfrastructure();
        
        InitBots();
    }

    private void InitializeInfrastructure()
    {
        _planner = new GlobalPlanner(PointsOfInterest);
        _botFactory = new BotFactory(BotPrefab, SpawnPoints);
    }

    private void InitBots()
    {
        if (BotCount > SpawnPoints.Count)
        {
            Debug.LogWarning("Not engough spawn points for current BotCount");

            _botFactory.SpawnBots(SpawnPoints.Count, _planner);
        }
        else
        {
            _botFactory.SpawnBots(BotCount,_planner);
        }
    }
}
