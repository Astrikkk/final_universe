using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJoiner : MonoBehaviour
{
    private PlayerInputManager manager;

    private void Awake()
    {
        manager = GetComponent<PlayerInputManager>();
    }

    private void Update()
    {
        foreach (Gamepad gamepad in Gamepad.all)
        {
            if (gamepad.startButton.wasPressedThisFrame)
            {
                TryJoinWithGamepad(gamepad);
            }
        }
    }

    private void TryJoinWithGamepad(Gamepad gamepad)
    {
        if (manager.playerCount >= 2) return;
        manager.JoinPlayer(controlScheme: "Gamepad", pairWithDevice: gamepad);
    }
}