using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using System.Linq;
using TMPro;

public class PlayerJoin : MonoBehaviour
{
    [SerializeField] private InputActionAsset asset;
    [SerializeField] private Camera previewCamera;
    [SerializeField] private Canvas canvas;
    [SerializeField] private TextMeshProUGUI joinedText;
    [SerializeField] private TextMeshProUGUI startPrompt;

    public Material[] playerMats;
    public Material[] playerBoosterMats;
    public Color[] playerColors;
    public string[] numbers;

    private InputAction interactAction;
    private List<NewPlayerInfo> playersJoined;
    public static PlayerJoin instance;

    protected void Awake()
    {
        instance = this;
        playersJoined = new List<NewPlayerInfo>();
        numbers = new string[] { "One", "Two", "Three", "Four" };

        interactAction = asset.FindAction("InteractPressed");
        interactAction.performed += JoinPlayer;
        interactAction.Enable();
    }

    protected void Start()
    {
        startPrompt.gameObject.SetActive(false);
    }

    private void JoinPlayer(InputAction.CallbackContext context)
    {
        if (GameFlowManager.instance.state != GameState.WAITING_FOR_PLAYERS)
            return;

        InputDevice device = context.control.device;
        if (playersJoined.TrueForAll(pi => pi.device != device) && playersJoined.Count < 4)
        {
            // New device! Create a player
            Debug.Log(playerColors.Length);
            Debug.Log(playersJoined.Count);
            int index = playersJoined.Count;
            playersJoined.Add(new NewPlayerInfo { device = device, color = playerColors[index] });
            string hex = ColorUtility.ToHtmlStringRGB(playerColors[index]);
            string space = index == 0 ? "" : "<space=2em>";
            joinedText.SetText(joinedText.text + $"{space}<color=#{hex}>Player {numbers[index]}</color>");

            if (playersJoined.Count > 1)
            {
                startPrompt.gameObject.SetActive(true);
            }

            SoundManager.PlayJoinClip();
        } 
        else
        {
            StartGame();
        }
    }

    private void StartGame()
    {
        GameFlowManager.instance.state = GameState.PLAYING;
        canvas.gameObject.SetActive(false);
        previewCamera.transform.rotation = Quaternion.Euler(180, 0, 0);
        previewCamera.clearFlags = CameraClearFlags.SolidColor;
        int playerIndex = 0;
        playersJoined.ForEach(pi =>
        {
            LevelManager.instance.SpawnPlayer(pi, playerIndex, playersJoined.Count);
            playerIndex++;
        });

        SoundManager.PlayStartClip();
    }
}

class NewPlayerInfo
{
    public InputDevice device;
    public Color color;
}
