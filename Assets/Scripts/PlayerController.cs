using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerController : MonoBehaviour
{
    [SerializeField] private InputActionAsset asset;
    [SerializeField] private bool invertRotation = true;

    private InputDevice device;

    public Vector2 Movement { get; private set; }
    public bool Interacting { get; private set; }

    private void Update()
    {
        if (device == null) return;
    }

    public void SetDevice(InputDevice device)
    {
        this.device = device;
        Debug.Log(device);

        var actionMap = asset.actionMaps[0].Clone();
        actionMap.devices = new UnityEngine.InputSystem.Utilities.ReadOnlyArray<InputDevice>(new InputDevice[] { device });
        actionMap.FindAction("InteractPressed").performed += context =>
        {
            Interacting = true;
        };
        actionMap.FindAction("InteractReleased").performed += context =>
        {
            Interacting = false;
        };
        actionMap.FindAction("Move").performed += context =>
        {
            Movement = context.ReadValue<Vector2>() * new Vector2(invertRotation ? -1 : 1, 1);
        };
        actionMap.FindAction("Move").canceled += context =>
        {
            Movement = context.ReadValue<Vector2>() * new Vector2(invertRotation ? -1 : 1, 1);
        };
        actionMap.Enable();
    }
}
