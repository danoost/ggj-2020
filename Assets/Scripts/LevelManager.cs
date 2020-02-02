using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private Transform environmentParent;
    public Vector2 Dimensions { get => dimensions; }

    [Header("Prefabs")]
    [SerializeField] private GameObject boosterPrefab;
    [SerializeField] private GameObject gunPrefab;
    [SerializeField] private GameObject asteroidPrefab;
    [SerializeField] private GameObject wallPrefab;
    [SerializeField] private GameObject playerPrefab;

    [Header("Settings")]
    [SerializeField] private Vector2 dimensions;
    [SerializeField] private int borderIterations = 5;
    [SerializeField] private float randomness = 2;
    [SerializeField] private float spacing = 5;
    [SerializeField] private bool randomiseSeed = true;
    [SerializeField] private int seed = 69;
    [SerializeField] private Vector2 playerCornerOffset = new Vector2(10, 10);
    [SerializeField] private Vector2 fullyInsideLevelBuffer = new Vector2(1, 1);
    [SerializeField] private float freeRadius = 2;

    [Header("Play-Space Shrinking")]
    [SerializeField] private float shrinkStartTime;
    [SerializeField] private float shrinkRate;
    [SerializeField] private Vector2[] minimumDimensionsByPlayers =
    {
        new Vector2(15, 15),
        new Vector2(20, 20),
        new Vector2(25, 25),
        new Vector2(30, 30)
    };

    [Header("Spawn Weights")]
    [SerializeField] private float boosterWeight;
    [SerializeField] private float gunWeight;
    [SerializeField] private float asteroidWeight;
    [SerializeField] private float noneWeight;

    [Header("Continual Spawn Parameters")]
    [SerializeField] private float minimumInterval;
    [SerializeField] private float maximumInterval;
    [SerializeField] private float minimumDistance;
    [SerializeField] private float maximumDistance;
    [SerializeField] private float initialVelocity;

    private (float, GameObject)[] weightList;
    private Vector2[] playerPositions;
    public static LevelManager instance;

    private GameObject top, bot, lef, rig;

    public void Start()
    {
        instance = this;
    }

    public void GenerateLevel()
    {
        Vector2 offset = dimensions / 2;
        float wallHeight = 5;
        // Create the boundary walls
        top = Instantiate(wallPrefab, new Vector3(0, offset.y, 0), Quaternion.identity, environmentParent);
        top.transform.localScale = new Vector3(dimensions.x, 1, wallHeight);
        top.name = "Top";
        bot = Instantiate(wallPrefab, new Vector3(0, -offset.y, 0), Quaternion.Euler(0, 0, 180), environmentParent);
        bot.transform.localScale = new Vector3(dimensions.x, 1, wallHeight);
        bot.name = "Bottom";
        lef = Instantiate(wallPrefab, new Vector3(-offset.x, 0, 0), Quaternion.Euler(0, 0, 90), environmentParent);
        lef.transform.localScale = new Vector3(dimensions.y, 1, wallHeight);
        lef.name = "Left";
        rig = Instantiate(wallPrefab, new Vector3(offset.x, 0, 0), Quaternion.Euler(0, 0, -90), environmentParent);
        rig.transform.localScale = new Vector3(dimensions.y, 1, wallHeight);
        rig.name = "Right";

        playerPositions = new Vector2[] 
        { 
            -offset + playerCornerOffset, 
            offset - playerCornerOffset, 
            new Vector2(offset.x - playerCornerOffset.x, -offset.y + playerCornerOffset.y), 
            new Vector2(-offset.x + playerCornerOffset.x, offset.y - playerCornerOffset.y) 
        };

        // Create all the objects
        if (!randomiseSeed)
        {
            Random.InitState(seed);
        }
        weightList = new (float, GameObject)[] { (noneWeight, null), (boosterWeight, boosterPrefab), (gunWeight, gunPrefab), (asteroidWeight, asteroidPrefab) };
        float totalWeight = weightList.Sum(thing => thing.Item1);

        for (int i = -borderIterations; i < (dimensions.x / spacing) + borderIterations; i++)
        {
            for (int j = -borderIterations; j < (dimensions.y / spacing) + borderIterations; j++)
            {
                float x = i * spacing + Random.Range(-randomness, randomness) - offset.x;
                float y = j * spacing + Random.Range(-randomness, randomness) - offset.y;
                Vector2 position = new Vector2(x, y);
                // Distance to all player spawn locations has to be at least freeRadius
                if (playerPositions.All(pp => (pp - position).sqrMagnitude > freeRadius * freeRadius))
                {
                    CreateObject(totalWeight, position);
                }
            }
        }

        // Start spawning stuff over time and shrink the level after a while 
        StartCoroutine(ContinuallySpawnObjects());
        StartCoroutine(ShrinkLevel());
    }

    internal void SpawnPlayer(NewPlayerInfo pi, int playerIndex, int totalPlayers)
    {
        GameObject newPlayer = Instantiate(playerPrefab, playerPositions[playerIndex], Quaternion.identity, null);
        newPlayer.GetComponent<PlayerVisual>().SetPlayerIndex(playerIndex);
        newPlayer.GetComponent<PlayerController>().SetDevice(pi.device);
        newPlayer.GetComponent<PlayerCamera>().SetConfig(totalPlayers, playerIndex);
        newPlayer.GetComponent<PlayerStats>().SetStats(pi.color, playerIndex);
    }

    private GameObject CreateObject(float totalWeight, Vector2 position)
    {
        float randomWeight = Random.Range(0, totalWeight);
        foreach (var pair in weightList)
        {
            if (randomWeight < pair.Item1)
            {
                if (pair.Item2 != null)
                {
                    GameObject newObject = Instantiate(pair.Item2, position, Quaternion.identity, environmentParent);
                    newObject.transform.localRotation *= Quaternion.Euler(0, Random.Range(0, 360), 0);
                    return newObject;
                }
                else
                {
                    return null;
                }
                
            }
            randomWeight -= pair.Item1;
        }
        return null;
    }

    private Vector2 GetPositionBeyondWall(int wallId)
    {
        Vector2 offset = dimensions / 2;
        switch (wallId)
        {
            // Left wall
            case 0:
                return new Vector2(
                    lef.transform.position.x - Random.Range(minimumDistance, maximumDistance), 
                    Random.Range(-offset.y, offset.y)
                );
            // Top wall
            case 1:
                return new Vector2(
                    Random.Range(-offset.x, offset.x),
                    top.transform.position.y + Random.Range(minimumDistance, maximumDistance)
                );
            // Right wall
            case 2:
                return new Vector2(
                    rig.transform.position.x + Random.Range(minimumDistance, maximumDistance),
                    Random.Range(-offset.y, offset.y)
                );
            // Bottom wall
            case 3:
                return new Vector2(
                    Random.Range(-offset.x, offset.x),
                    bot.transform.position.y - Random.Range(minimumDistance, maximumDistance)
                );
            default:
                return Vector2.zero;
        }
    }

    private IEnumerator ContinuallySpawnObjects()
    {
        yield return new WaitUntil(() => GameFlowManager.instance.state == GameState.PLAYING);
        float totalWeight = weightList.Sum(thing => thing.Item1);
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minimumInterval, maximumInterval));

            // Choose a wall to start from and target. Can't be the same one.
            int startWallId = Random.Range(0, 4);
            int targetWallId;
            while ((targetWallId = Random.Range(0, 4)) == startWallId) ;

            Vector2 startPosition = GetPositionBeyondWall(startWallId);
            Vector2 targetPosition = GetPositionBeyondWall(targetWallId);

            GameObject newObj = CreateObject(totalWeight, startPosition);
            if (newObj != null) 
            {
                if (newObj.TryGetComponent(out Rigidbody2D rb))
                {
                    rb.velocity = (targetPosition - startPosition).normalized * initialVelocity;
                }
            }
        }
    }

    private IEnumerator ShrinkLevel()
    {
        yield return new WaitUntil(() => GameFlowManager.instance.state == GameState.PLAYING);
        yield return new WaitForSeconds(shrinkStartTime);

        float aspectRatio = dimensions.x / dimensions.y;
        float wallHeight = 5;

        while (true)
        {
            yield return new WaitForFixedUpdate();

            if (dimensions.x > minimumDimensionsByPlayers[GameFlowManager.instance.PlayerCount - 1].x)
            {
                lef.transform.position -= shrinkRate * lef.transform.up * aspectRatio * Time.deltaTime;
                rig.transform.position -= shrinkRate * rig.transform.up * aspectRatio * Time.deltaTime;
            }
            if (dimensions.y > minimumDimensionsByPlayers[GameFlowManager.instance.PlayerCount - 1].y)
            {
                top.transform.position -= shrinkRate * top.transform.up * Time.deltaTime;
                bot.transform.position -= shrinkRate * bot.transform.up * Time.deltaTime;
            }

            dimensions = new Vector2(
                rig.transform.position.x - lef.transform.position.x,
                top.transform.position.y - bot.transform.position.y
            );

            lef.transform.localScale = new Vector3(dimensions.y, 1, wallHeight);
            rig.transform.localScale = new Vector3(dimensions.y, 1, wallHeight);
            top.transform.localScale = new Vector3(dimensions.x, 1, wallHeight);
            bot.transform.localScale = new Vector3(dimensions.x, 1, wallHeight);
        }
    }

    public bool IsFullyInsideLevel(Vector2 position)
    {
        Vector2 offset = dimensions / 2;

        if (position.x < -offset.x + fullyInsideLevelBuffer.x)
        {
            return false;
        }
        if (position.x > offset.x - fullyInsideLevelBuffer.x)
        {
            return false;
        }
        if (position.y < -offset.y + fullyInsideLevelBuffer.y)
        {
            return false;
        }
        if (position.y > offset.y - fullyInsideLevelBuffer.y)
        {
            return false;
        }
        return true;
    }
}
