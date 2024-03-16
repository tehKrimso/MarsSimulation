using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using Infrastructure;
using Static;
using Unity.VisualScripting;
using UnityEngine;

public class EntryPoint : MonoBehaviour
{
    public List<Transform> SpawnPoints;
    public List<PointOfInterest> PointsOfInterest;

    public GameObject BotPrefab;

    [Space, Header("Settings")] public int BotCount;


    private GlobalPlanner _planner;
    private BotFactory _botFactory;


    private WorkingZoneDivider _workingZoneDivider;
    

    private void Awake()
    {
        _workingZoneDivider = GetComponent<WorkingZoneDivider>();
        
        InitializeInfrastructure();
        
        InitBots();
    }

    private void InitializeInfrastructure()
    {
        _planner = new GlobalPlanner(_workingZoneDivider.GetReferencePointsFromWorkingZone(),PointsOfInterest);
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
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        var points = _workingZoneDivider.GetReferencePointsFromWorkingZone();
        foreach (Vector3 point in points)
        {
            Gizmos.DrawSphere(point,0.2f);
        }
    }
    
    
}
