using Spawner;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class GameEndController : MonoBehaviour
    {
        [SerializeField] private GameObject _gameOverPanel;
        [SerializeField] private GameObject _gameWonPanel;
        private void Start()
        {
            _gameOverPanel.SetActive(false);
            EndGame.GameOver += GameOver;
            EnemySpawner.AllWavesCleared += GameWon;
        }
        private void OnDestroy()
        {
            EndGame.GameOver -= GameOver;
            EnemySpawner.AllWavesCleared -= GameWon;
        }
        private void GameOver()
        {
            _gameOverPanel.SetActive(true);
            AudioManager.Instance.Play("Lose");
            Time.timeScale = 0f;
        }
        private void GameWon()
        {
            _gameWonPanel.SetActive(true);
            AudioManager.Instance.Play("Won");
            Time.timeScale = 0f;
        }
        public void Retry() 
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            _gameOverPanel.SetActive(false);
        }
        public void ReturnToMainMenu()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene("Main Menu");
            _gameOverPanel.SetActive(false);
        }
    }

}
