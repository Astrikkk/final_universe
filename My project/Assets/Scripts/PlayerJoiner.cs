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
        // Если игроков уже 2, отключаем ожидание
        if (manager.playerCount >= 2)
        {
            if (waitingForConfirm) CancelJoin();
            return;
        }

        if (!waitingForConfirm)
        {
            // 1. Проверка нажатия Options на геймпаде
            foreach (Gamepad gamepad in Gamepad.all)
            {
                if (gamepad.startButton.wasPressedThisFrame)
                {
                    TryShowPrompt(gamepad);
                    break;
                }
            }

            // 2. ВРЕМЕННЫЙ ТЕСТ: проверка нажатия J на клавиатуре
            if (Keyboard.current != null && Keyboard.current.jKey.wasPressedThisFrame)
            {
                TryShowPrompt(null); // null означает, что мы вызвали тест с клавиатуры
            }
        }
        else
        {
            // Таймер ожидания
            currentTimer -= Time.deltaTime;
            if (currentTimer <= 0f)
            {
                CancelJoin();
                return;
            }

            // Подтверждение с геймпада
            if (pendingGamepad != null)
            {
                if (pendingGamepad.startButton.wasPressedThisFrame || pendingGamepad.buttonSouth.wasPressedThisFrame)
                {
                    ConfirmJoin();
                }
            }
            // Подтверждение с клавиатуры (ТЕСТ)
            else
            {
                if (Keyboard.current != null && Keyboard.current.jKey.wasPressedThisFrame)
                {
                    ConfirmJoin();
                }
            }
        }
    }

    private void TryShowPrompt(Gamepad gamepad)
    {
        // Проверка: не занят ли геймпад Игроком 1
        if (player1 != null && gamepad != null)
        {
            PlayerInput p1Input = player1.GetComponent<PlayerInput>();
            if (p1Input != null && p1Input.currentControlScheme == "Gamepad")
            {
                foreach (var device in p1Input.devices)
                {
                    if (device == gamepad) return;
                }
            }
        }

        pendingGamepad = gamepad;
        waitingForConfirm = true;
        currentTimer = confirmTimeout;
        
        if (joinPromptPanel != null) 
            joinPromptPanel.SetActive(true);
            
        Debug.Log("Панель появилась. Ждем подтверждения (Options, Прыжок или J).");
    }

    private void ConfirmJoin()
    {
        if (manager.playerCount >= 2) return;

        if (pendingGamepad != null)
        {
            manager.JoinPlayer(controlScheme: "Gamepad", pairWithDevice: pendingGamepad);
        }
        else
        {
            // Для теста создаем игрока на клавиатуре
            manager.JoinPlayer(controlScheme: "Keyboard", pairWithDevice: Keyboard.current);
        }

        CancelJoin();
        Debug.Log("Player 2 создан.");
    }

    private void CancelJoin()
    {
        waitingForConfirm = false;
        pendingGamepad = null;
        if (joinPromptPanel != null) 
            joinPromptPanel.SetActive(false);
            
        Debug.Log("Панель скрыта.");
    }
}