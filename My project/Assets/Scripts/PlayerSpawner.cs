using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private Transform player1;
    [SerializeField] private Vector3 spawnOffset = new Vector3(1.5f, 0f, 0f);

    private PlayerInput player2;

    private void OnPlayerJoined(PlayerInput player)
    {
        if (player.transform == player1) return;
        if (player1 == null) return;

        player2 = player;
        player.transform.position = player1.position + spawnOffset;
    }

    private void Update()
    {
        if (Keyboard.current != null && Keyboard.current.nKey.wasPressedThisFrame)
        {
            if (player2 != null)
            {
                Destroy(player2.gameObject);
                player2 = null;
            }
        }
    }
}