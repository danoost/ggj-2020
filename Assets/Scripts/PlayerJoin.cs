using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using System.Linq;

public class PlayerJoin : MonoBehaviour
{
    [SerializeField] private InputActionAsset asset;

    private InputAction interactAction;
    private List<NewPlayerInfo> playersJoined;

    protected void Awake()
    {
        playersJoined = new List<NewPlayerInfo>();
        interactAction = asset.FindAction("InteractPressed");
        interactAction.performed += JoinPlayer;
        interactAction.Enable();
    }

    private void JoinPlayer(InputAction.CallbackContext context)
    {
        if (GameFlowManager.instance.state != GameState.WAITING_FOR_PLAYERS)
            return;

        InputDevice device = context.control.device;
        if (playersJoined.TrueForAll(pi => pi.device != device))
        {
            // New device! Create a player
            playersJoined.Add(new NewPlayerInfo { device = device, color = DynamicMaterialColor.CreateRandomHue() });
        } else
        {
            StartGame();
        }
    }

    private void StartGame()
    {
        GameFlowManager.instance.state = GameState.PLAYING;
        int playerIndex = 0;
        playersJoined.ForEach(pi =>
        {
            LevelGenerator.instance.SpawnPlayer(pi, playerIndex);
            playerIndex++;
        });
    }
}

class NewPlayerInfo
{
    public InputDevice device;
    public Color color;
}
