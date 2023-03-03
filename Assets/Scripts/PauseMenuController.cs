using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class PauseMenuController : MonoBehaviour
    {
        [Header("--- PAUSE MENU---")]
        [SerializeField] private GameObject _pauseMenuUIPanel;
        public static bool S_PAUSED;

        public static event Action<bool> GamePaused;
        public static event Action ReturnToMM;

        private void Awake()
        {
            S_PAUSED = false;
        }
        public void PauseGame()
        {
            //if paused, resume the game
            if (S_PAUSED)
            {
                Time.timeScale = 1f;
                S_PAUSED = false;
                _pauseMenuUIPanel.SetActive(false);
                GamePaused?.Invoke(false);
            }
            else
            {
                Time.timeScale = 0f;
                S_PAUSED = true;
                _pauseMenuUIPanel.SetActive(true);
                GamePaused?.Invoke(true);
            }
        }
        public void ReturnToMainMenu()
        {
            ReturnToMM?.Invoke();
            Time.timeScale = 1f;
            SceneManager.LoadScene("Main Menu");
        }
    }

}

