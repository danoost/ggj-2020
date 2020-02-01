using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using System.Linq;

public class PlayerJoin : MonoBehaviour
{
    [SerializeField] private InputActionAsset asset;
    [SerializeField] private Camera previewCamera;

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
        previewCamera.transform.rotation = Quaternion.Euler(180, 0, 0);
        previewCamera.clearFlags = CameraClearFlags.SolidColor;
        int playerIndex = 0;
        playersJoined.ForEach(pi =>
        {
            LevelManager.instance.SpawnPlayer(pi, playerIndex, playersJoined.Count);
            playerIndex++;
        });
    }
}

class NewPlayerInfo
{
    public InputDevice device;
    public Color color;
}
