using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private Transform player1;
    [SerializeField] private Vector3 spawnOffset = new Vector3(1.5f, 0f, 0f);

    private void OnPlayerJoined(PlayerInput player)
    {
        if (player.transform == player1) return;
        if (player1 == null) return;

        player.transform.position = player1.position + spawnOffset;
    }
}