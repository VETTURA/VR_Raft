using System;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;

public class ItemsGenerator : MonoBehaviour
{
    private System.Random rnd = new();

    private RaftController raft;
    private Water water;

    private struct ItemPrefab
    {
        public GameObject gameObject { get; set; }
        public float probability { get; set; }

        public ItemPrefab(GameObject gameObject, float probability)
        {
            this.gameObject = gameObject;
            this.probability = probability;
        }
    }

    private List<ItemPrefab> itemsPrefab;

    //Генерит ошибки
    //[SerializeField]
    private float startTime = 0.0f;

    //Генерит ошибки
    //[SerializeField]
    private float repeatRate = 2.0f;

    private const string NAMEREPEATFUNCTION = "SpawnItems";

    //Генерит ошибки
    //[SerializeField]
    private float minXSpawnPosition = -10.0f;

    //Генерит ошибки
    //[SerializeField]
    private float maxXSpawnPosition = 10.0f;

    //Генерит ошибки
    //[SerializeField]
    private float distanceSpawn = 30.0f;

    void Start()
    {
        raft = FindFirstObjectByType<RaftController>();
        water = FindFirstObjectByType<Water>();
        
        GetPrefabs();

        InvokeRepeating(NAMEREPEATFUNCTION, startTime, repeatRate);
    }

    private void GetPrefabs()
    {
        itemsPrefab = new()
        {
            new ItemPrefab((GameObject)Resources.Load("Prefabs/Level/InteractiveItems/Paddle"), 0.1f),
            new ItemPrefab((GameObject)Resources.Load("Prefabs/Level/InteractiveItems/Net"), 0.2f),
            new ItemPrefab((GameObject)Resources.Load("Prefabs/Level/InteractiveItems/Stick"), 0.8f),
            new ItemPrefab((GameObject)Resources.Load("Prefabs/Level/InteractiveItems/Fishes/Fish_1"), 0.7f),
            new ItemPrefab((GameObject)Resources.Load("Prefabs/Level/InteractiveItems/Fishes/Fish_2"), 0.7f),
        };
    }

    private void SpawnItems()
    {
        var zPosition = raft.transform.position.z - distanceSpawn;
        var yPosition = water.transform.position.y;
        var xPosition = (float)(rnd.NextDouble() * (maxXSpawnPosition - minXSpawnPosition) + minXSpawnPosition);

        var probability = (float)rnd.NextDouble();

        try
        {
            GameObject spawnItem = Instantiate(
                GetProbPrefab(probability),
                new(xPosition, yPosition, zPosition),
                Quaternion.identity,
                gameObject.transform
            );
        }
        catch { }
    }

    private GameObject GetProbPrefab(float probability)
    {
        List<GameObject> result = new();

        foreach (var item in itemsPrefab)
        {
            if (item.probability > probability)
            {
                result.Add(item.gameObject);
            }
        }

        if (result.Count != 0)
        {
            var element = rnd.Next(0, result.Count);

            return result[element];
        }
        else
        {
            throw new Exception("List is empty");
        }
    }
}
