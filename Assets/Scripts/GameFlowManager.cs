using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFlowManager : MonoBehaviour
{
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private PlayerJoin playerManager;

    public GameState state;

    public static GameFlowManager instance;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        levelManager.GenerateLevel();
    }
}

public enum GameState
{
    WAITING_FOR_PLAYERS,
    PLAYING
}