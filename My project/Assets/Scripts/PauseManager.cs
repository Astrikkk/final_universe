using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject removePlayer2Button; // Наша новая кнопка

    private bool isPaused;

    private void Start()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
    }

    private void Update()
    {
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            TogglePause();
        }
    }

    private void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            pausePanel.SetActive(true);
            Time.timeScale = 0f;

            // Проверяем, есть ли Player 2
            // PlayerInput.all.Count > 1 означает, что в игре больше одного игрока
            if (PlayerInput.all.Count > 1)
            {
                removePlayer2Button.SetActive(true);
            }
            else
            {
                removePlayer2Button.SetActive(false);
            }
        }
        else
        {
            pausePanel.SetActive(false);
            Time.timeScale = 1f;
        }
    }

    // Этот метод мы привяжем к кнопке "Убрать Игрока 2"
    public void OnRemovePlayer2Clicked()
    {
        // Находим всех игроков
        var players = PlayerInput.all;
        
        // Если игроков больше 1, удаляем второго (индекс 1)
        if (players.Count > 1)
        {
            Destroy(players[1].gameObject);
        }

        // Убираем паузу после удаления
        TogglePause();
    }
}