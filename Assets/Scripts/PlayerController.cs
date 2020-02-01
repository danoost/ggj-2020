using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerController : MonoBehaviour
{
    [SerializeField] private InputActionAsset asset;
    [SerializeField] private bool invertRotation = true;

    public Vector2 Movement { get; private set; }
    public bool Interacting { get; private set; }

    public void SetDevice(InputDevice device)
    {
        Debug.Log(device);
        asset.FindAction("InteractPressed").performed += context =>
        {
            if (context.control.device.deviceId != device.deviceId)
                return;
            Interacting = true;
        };
        asset.FindAction("InteractReleased").performed += context =>
        {
            if (context.control.device.deviceId != device.deviceId)
                return; 
            Interacting = false;
        };
        asset.FindAction("Move").performed += context =>
        {
            if (context.control.device.deviceId != device.deviceId)
                return;
            Movement = context.ReadValue<Vector2>() * new Vector2(invertRotation ? -1 : 1, 1);
        };
        asset.FindAction("Move").canceled += context =>
        {
            if (context.control.device.deviceId != device.deviceId)
                return;
            Movement = context.ReadValue<Vector2>() * new Vector2(invertRotation ? -1 : 1, 1);
        };

        foreach (var actionMap in asset.actionMaps)
            actionMap.Enable();
    }
}
