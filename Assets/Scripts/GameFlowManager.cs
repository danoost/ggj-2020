using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFlowManager : MonoBehaviour
{
    [SerializeField] private LevelManager levelManager;

    public GameState state;
    public static GameFlowManager instance;

    private List<PlayerStats> players;

    void Start()
    {
        instance = this;
        levelManager.GenerateLevel();
        players = new List<PlayerStats>();
    }

    public void RegisterPlayer(PlayerStats player)
    {
        players.Add(player);
    }

    public void DeadPlayer(PlayerStats player)
    {
        players.Remove(player);
        if (players.Count == 1)
        {
            var winner = players[0];
            Debug.Log($"{winner.Index}");
        }
    }
}

public enum GameState
{
    WAITING_FOR_PLAYERS,
    PLAYING
}