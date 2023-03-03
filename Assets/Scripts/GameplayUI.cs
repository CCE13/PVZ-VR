using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Spawner;
using UnityEngine.SceneManagement;

namespace UI
{
    [DefaultExecutionOrder(-1000)]
    public class GameplayUI : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text _countdownUI, _currentWaveText;
        [SerializeField] private GameObject _gameUIPanel;
        [SerializeField] private Transform _canvas;
        // Start is cal led before the first frame update

        private void Awake()
        {
            Time.timeScale = 1f;
        }
        void Start()
        {
            EnemySpawner.UpdateWaveCountdownUI += UpdateCountdownUI;
            EnemySpawner.UpdateWaveIndex += UpdateCurrentWave;
            EndGame.GameOver += DeactivateGameUIPanel;
            PauseMenuController.GamePaused += GamePaused;
        }
        private void OnDestroy()
        {
            EnemySpawner.UpdateWaveCountdownUI -= UpdateCountdownUI;
            EnemySpawner.UpdateWaveIndex -= UpdateCurrentWave;
            EndGame.GameOver -= DeactivateGameUIPanel;
            PauseMenuController.GamePaused -= GamePaused;
        }
        private void LateUpdate()
        {
            _canvas.LookAt(Camera.main.transform);
        }
        private void GamePaused(bool paused)
        {
            if(paused)
            {
                DeactivateGameUIPanel();
            }
            else
            {
                _gameUIPanel.SetActive(true);
            }
        }
        private void DeactivateGameUIPanel()
        {
            _gameUIPanel.SetActive(false);
        }
        private void UpdateCountdownUI(int time)
        {
            if(time == 0)
            {
                _countdownUI.gameObject.SetActive(false);
            }
            else
            {
                _countdownUI.gameObject.SetActive(true);
            }
            _countdownUI.text = $"Next Wave:{time}";
        }
        private void UpdateCurrentWave(int index)
        {
            _currentWaveText.text = $"WAVE {index +1 }";
        }
    }
}

