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

        PlayerInput p1Input = player1.GetComponent<PlayerInput>();

        // Проверяем: тот же геймпад у Игрока 2, что у Игрока 1?
        bool sameDevice = false;
        foreach (var d1 in p1Input.devices)
        {
            foreach (var d2 in player.devices)
            {
                if (d1 == d2) sameDevice = true;
            }
        }

        if (sameDevice)
        {
            // Друг забрал твой геймпад — садим тебя на клавиатуру
            p1Input.SwitchCurrentControlScheme("Keyboard", Keyboard.current);
        }
        // Иначе — ничего не делаем. Ты остаёшься на своём геймпаде.
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