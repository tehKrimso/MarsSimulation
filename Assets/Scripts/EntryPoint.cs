using System;
using System.Collections;
using System.Collections.Generic;
using Static;
using UnityEngine;

public class EntryPoint : MonoBehaviour
{
    public List<Transform> SpawnPoints;

    public GameObject BotPrefab;

    [Space, Header("Settings")] public int BotCount;

    private void Awake()
    {
        if (BotCount > SpawnPoints.Count)
        {
            Debug.LogWarning("Not engough spawn points for current BotCount");

            SpawnBots(SpawnPoints.Count);
        }
        else
        {
            SpawnBots(BotCount);
        }
    }

    private void SpawnBots(int count)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject bot = GameObject.Instantiate(BotPrefab, SpawnPoints[i].position, Quaternion.identity);
            bot.GetComponent<BotController>().Init(i);
            MeshRenderer botRenderer = bot.GetComponent<MeshRenderer>();
            botRenderer.material.SetColor("_Color", Colors.GetColor(i));
        }
    }
}
