using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJoiner : MonoBehaviour
{
    [SerializeField] private Transform player1;
    [SerializeField] private GameObject joinPromptPanel;
    [SerializeField] private float confirmTimeout = 5f;

    private PlayerInputManager manager;
    private bool waitingForConfirm = false;
    private Gamepad pendingGamepad;
    private float currentTimer;

    private void Awake()
    {
        manager = GetComponent<PlayerInputManager>();
    }

    private void Update()
    {
        if (manager.playerCount >= 2)
        {
            if (waitingForConfirm) CancelJoin();
            return;
        }

        if (!waitingForConfirm)
        {
            foreach (Gamepad gamepad in Gamepad.all)
            {
                if (gamepad.startButton.wasPressedThisFrame)
                {
                    Debug.Log("1. Нажата кнопка Options!");
                    TryShowPrompt(gamepad);
                    break;
                }
            }
        }
        else
        {
            currentTimer -= Time.deltaTime;
            if (currentTimer <= 0f)
            {
                Debug.Log("2. Таймер истек.");
                CancelJoin();
                return;
            }

            if (pendingGamepad.startButton.wasPressedThisFrame || pendingGamepad.buttonSouth.wasPressedThisFrame)
            {
                Debug.Log("2. Нажато подтверждение!");
                ConfirmJoin();
            }
        }
    }

    private void TryShowPrompt(Gamepad gamepad)
    {
        if (player1 != null)
        {
            PlayerInput p1Input = player1.GetComponent<PlayerInput>();
            if (p1Input != null && p1Input.currentControlScheme == "Gamepad")
            {
                foreach (var device in p1Input.devices)
                {
                    if (device == gamepad) 
                    {
                        Debug.Log("3. ОТКАЗ: Игрок 1 уже сидит на этом геймпаде.");
                        return; 
                    }
                }
            }
        }

        Debug.Log("3. Показываем панель!");
        pendingGamepad = gamepad;
        waitingForConfirm = true;
        currentTimer = confirmTimeout;
        
        if (joinPromptPanel != null) 
            joinPromptPanel.SetActive(true);
    }

    private void ConfirmJoin()
    {
        Debug.Log("4. Создаем игрока...");
        if (manager.playerCount >= 2) return;
        manager.JoinPlayer(controlScheme: "Gamepad", pairWithDevice: pendingGamepad);
        CancelJoin();
    }

    private void CancelJoin()
    {
        waitingForConfirm = false;
        pendingGamepad = null;
        if (joinPromptPanel != null) 
            joinPromptPanel.SetActive(false);
    }
}