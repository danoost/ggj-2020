using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameFlowManager : MonoBehaviour
{
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private Canvas winCanvas;
    [SerializeField] private TextMeshProUGUI winText;
    [SerializeField] private TextMeshProUGUI restartText;

    public GameState state;
    public static GameFlowManager instance;

    private List<PlayerStats> players;

    public int PlayerCount
    {
        get { return players.Count; }
    }

    void Start()
    {
        instance = this;
        levelManager.GenerateLevel();
        players = new List<PlayerStats>();
        winCanvas.gameObject.SetActive(false);
    }

    public void RegisterPlayer(PlayerStats player)
    {
        players.Add(player);

        SoundManager.PlayJoinClip();
    }

    public void DeadPlayer(PlayerStats player)
    {
        players.Remove(player);
        if (players.Count == 1)
        {
            var winner = players[0];
            Debug.Log($"{winner.Index}");
            winCanvas.gameObject.SetActive(true);
            string hex = ColorUtility.ToHtmlStringRGB(winner.Color);
            winText.SetText($"<color=#{hex}>Player {PlayerJoin.instance.numbers[winner.Index]} Wins!</color>");
            StartCoroutine(Restart());

            SoundManager.PlayWinClip();
        }

        SoundManager.PlayDeathClip();
    }

    private IEnumerator Restart()
    {
        restartText.SetText("New Game in 5...");
        yield return new WaitForSeconds(1);
        restartText.SetText(restartText.text + "4...");
        yield return new WaitForSeconds(1);
        restartText.SetText(restartText.text + "3...");
        yield return new WaitForSeconds(1);
        restartText.SetText(restartText.text + "2...");
        yield return new WaitForSeconds(1);
        restartText.SetText(restartText.text + "1...");
        yield return new WaitForSeconds(1);
        restartText.SetText(restartText.text + "NOW");
        //SceneManager.LoadScene("Main");
        //System.Diagnostics.Process.Start(Application.dataPath.Replace("_Data", ".exe")); //new program
        //Application.Quit(); //kill current process
        Debug.Log("Out of scope");
        throw new NotImplementedException("Out of scope");
    }
}

public enum GameState
{
    WAITING_FOR_PLAYERS,
    PLAYING
}