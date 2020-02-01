using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] private Transform environmentParent;

    [Header("Prefabs")]
    [SerializeField] private GameObject boosterPrefab;
    [SerializeField] private GameObject gunPrefab;
    [SerializeField] private GameObject asteroidPrefab;
    [SerializeField] private GameObject wallPrefab;

    [Header("Settings")]
    [SerializeField] private Vector2 dimensions;
    [SerializeField] private float randomness = 2;
    [SerializeField] private float spacing = 5;
    [SerializeField] private int seed = 69;

    [Header("Spawn Weights")]
    [SerializeField] private float boosterWeight;
    [SerializeField] private float gunWeight;
    [SerializeField] private float asteroidWeight;
    [SerializeField] private float noneWeight;

    private (float, GameObject)[] weightList;

    void Start()
    {
        Vector2 offset = dimensions / 2;
        float wallHeight = 5;
        // Create the boundary walls
        var top = Instantiate(wallPrefab, new Vector3(0, offset.y, 0), Quaternion.identity, environmentParent);
        top.transform.localScale = new Vector3(dimensions.x, 1, wallHeight);
        top.name = "Top";
        var bot = Instantiate(wallPrefab, new Vector3(0, -offset.y, 0), Quaternion.Euler(0, 0, 180), environmentParent);
        bot.transform.localScale = new Vector3(dimensions.x, 1, wallHeight);
        bot.name = "Bottom";
        var lef = Instantiate(wallPrefab, new Vector3(-offset.x, 0, 0), Quaternion.Euler(0, 0, 90), environmentParent);
        lef.transform.localScale = new Vector3(dimensions.y, 1, wallHeight);
        lef.name = "Left";
        var rig = Instantiate(wallPrefab, new Vector3(offset.x, 0, 0), Quaternion.Euler(0, 0, -90), environmentParent);
        rig.transform.localScale = new Vector3(dimensions.y, 1, wallHeight);
        rig.name = "Right";

        // Create all the objects
        UnityEngine.Random.InitState(seed);
        weightList = new (float, GameObject)[] { (noneWeight, null), (boosterWeight, boosterPrefab), (gunWeight, gunPrefab), (asteroidWeight, asteroidPrefab) };
        float totalWeight = weightList.Sum(thing => thing.Item1);

        for (int i = 0; i < dimensions.x / spacing; i++)
        {
            for (int j = 0; j < dimensions.y / spacing; j++)
            {
                float x = i * spacing + UnityEngine.Random.Range(-randomness, randomness) - offset.x;
                float y = j * spacing + UnityEngine.Random.Range(-randomness, randomness) - offset.y;
                CreateObject(totalWeight, x, y);
            }
        }
    }

    private void CreateObject(float totalWeight, float x, float y)
    {
        float randomWeight = UnityEngine.Random.Range(0, totalWeight);
        foreach (var pair in weightList)
        {
            if (randomWeight < pair.Item1)
            {
                if (pair.Item2 != null)
                    Instantiate(pair.Item2, new Vector3(x, y, 0), Quaternion.identity, environmentParent);
                return;
            }
            randomWeight -= pair.Item1;
        }
    }
}
