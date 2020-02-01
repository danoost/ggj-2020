using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] private Transform environmentParent;
    public Vector2 Dimensions { get => dimensions; }

    [Header("Prefabs")]
    [SerializeField] private GameObject boosterPrefab;
    [SerializeField] private GameObject asteroidPrefab;
    [SerializeField] private GameObject wallPrefab;
    [SerializeField] private GameObject playerPrefab;

    [Header("Settings")]
    [SerializeField] private Vector2 dimensions;
    [SerializeField] private float randomness = 2;
    [SerializeField] private float spacing = 5;
    [SerializeField] private int seed = 69;
    [SerializeField] private Vector2 playerCornerOffset = new Vector2(10, 10);
    [SerializeField] private float freeRadius = 2;

    [Header("Spawn Weights")]
    [SerializeField] private float boosterWeight;
    [SerializeField] private float asteroidWeight;
    [SerializeField] private float noneWeight;

    private (float, GameObject)[] weightList;
    private Vector2[] playerPositions;
    public static LevelGenerator instance;

    public void Start()
    {
        instance = this;
    }

    public void GenerateLevel()
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

        playerPositions = new Vector2[] { 
            -offset + playerCornerOffset, 
            offset - playerCornerOffset, 
            new Vector2(offset.x - playerCornerOffset.x, -offset.y + playerCornerOffset.y), 
            new Vector2(-offset.x + playerCornerOffset.x, offset.y - playerCornerOffset.y) 
        };

        // Create all the objects
        Random.InitState(seed);
        float totalWeight = boosterWeight + asteroidWeight + noneWeight; // Weights are programmed weirdly but who cares lmao
        weightList = new (float, GameObject)[] { (noneWeight, null), (boosterWeight, boosterPrefab), (asteroidWeight, asteroidPrefab) };

        for (int i = 0; i < dimensions.x / spacing; i++)
        {
            for (int j = 0; j < dimensions.y / spacing; j++)
            {
                float x = i * spacing + Random.Range(-randomness, randomness) - offset.x;
                float y = j * spacing + Random.Range(-randomness, randomness) - offset.y;
                Vector2 position = new Vector2(x, y);
                // Distance to all player spawn locations has to be at least freeRadius
                if (playerPositions.All(pp => (pp - position).sqrMagnitude > freeRadius * freeRadius))
                    CreateObject(totalWeight, position);
            }
        }
    }

    internal void SpawnPlayer(NewPlayerInfo pi, int playerIndex)
    {
        GameObject newPlayer = Instantiate(playerPrefab, playerPositions[playerIndex], Quaternion.identity, null);
        newPlayer.GetComponent<PlayerVisual>().SetColor(pi.color);
        newPlayer.GetComponent<PlayerController>().SetDevice(pi.device);
    }

    private void CreateObject(float totalWeight, Vector2 position)
    {
        float randomWeight = Random.Range(0, totalWeight);
        foreach (var pair in weightList)
        {
            if (randomWeight < pair.Item1)
            {
                if (pair.Item2 != null)
                    Instantiate(pair.Item2, position, Quaternion.identity, environmentParent);
                return;
            }
            randomWeight -= pair.Item1;
        }
    }
}
