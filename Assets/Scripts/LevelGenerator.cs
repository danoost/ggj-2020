using UnityEngine;
using System.Collections.Generic;
using System;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] private Transform environmentParent;
    [SerializeField] private Vector2 dimensions;

    [SerializeField] private float randomness = 2;
    [SerializeField] private float spacing = 5;

    [SerializeField] private GameObject boosterPrefab;
    [SerializeField] private float boosterWeight;

    [SerializeField] private GameObject asteroidPrefab;
    [SerializeField] private float asteroidWeight;

    [SerializeField] private float noneWeight;

    [SerializeField] private int seed = 69;

    private (float, GameObject)[] weightList;

    void Start()
    {
        UnityEngine.Random.InitState(seed);
        Vector2 offset = dimensions / 2;
        float totalWeight = boosterWeight + asteroidWeight + noneWeight; // Weights are programmed weirdly but who cares lmao
        weightList = new (float, GameObject)[] { (noneWeight, null), (boosterWeight, boosterPrefab), (asteroidWeight, asteroidPrefab) };

        for (int i = 0; i < dimensions.x / spacing; i++)
        {
            for (int j = 0; j < dimensions.y / spacing; j++)
            {
                float x = i * spacing + UnityEngine.Random.Range(-randomness, randomness) - offset.x;
                float z = j * spacing + UnityEngine.Random.Range(-randomness, randomness) - offset.y;
                CreateObject(totalWeight, x, z);
            }
        }
    }

    private void CreateObject(float totalWeight, float x, float z)
    {
        float randomWeight = UnityEngine.Random.Range(0, totalWeight);
        foreach (var pair in weightList)
        {
            if (randomWeight < pair.Item1)
            {
                if (pair.Item2 != null)
                    Instantiate(pair.Item2, new Vector3(x, 0, z), Quaternion.identity, environmentParent);
                return;
            }
            randomWeight -= pair.Item1;
        }
    }
}
