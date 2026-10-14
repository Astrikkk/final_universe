using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class GamepadIcon : MonoBehaviour
{
    [SerializeField] private Image iconImage;
    [SerializeField] private Sprite onSprite;
    [SerializeField] private Sprite offSprite;

    private void Start()
    {
        UpdateIcon(Gamepad.all.Count > 0);
    }

    private void OnEnable()
    {
        InputSystem.onDeviceChange += OnDeviceChange;
    }

    private void OnDisable()
    {
        InputSystem.onDeviceChange -= OnDeviceChange;
    }

    private void OnDeviceChange(InputDevice device, InputDeviceChange change)
    {
        if (!(device is Gamepad)) return;

        switch (change)
        {
            case InputDeviceChange.Added:
                UpdateIcon(true);
                break;
            case InputDeviceChange.Removed:
            case InputDeviceChange.Disconnected:
                UpdateIcon(false);
                break;
        }
    }

    private void UpdateIcon(bool isConnected)
    {
        iconImage.sprite = isConnected ? onSprite : offSprite;
    }
}