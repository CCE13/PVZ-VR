using Enemy;
using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;
using Wave;

namespace Spawner
{
    public class EnemySpawner : MonoBehaviour
    {
        public static bool S_INITWAVE;
        private enum WaveState
        {
            CountingDown,
            Spawning,
            Finished,
            GameOver
        }
        [SerializeField] private WaveState _waveState;
        [SerializeField] private float _waveCountdown;
        [SerializeField] private List<Transform> _spawnPos;
        [SerializeField] private List<WaveData> _waves;

        private float _ogWaveCountdown;
        private int _currentWaveIndex;
        private bool _audioPlayed;
        private bool _gameOver;
        public static event Action<int> UpdateWaveCountdownUI;
        public static event Action<int> UpdateWaveIndex;
        public static event Action ResetPottedPlants;
        public static event Action AllWavesCleared;
        
        
         private void Start()
        {
            if(SceneManager.GetActiveScene().name != "Main Menu")
            {
                AudioManager.Instance?.Play("Background");
            }
            _ogWaveCountdown = _waveCountdown;
            foreach (var item in _waves)
            {
                item.SetSpawnRate();
            }
            StartWave.Start += InitWave;
            EndGame.GameOver += GameOver;
            Reseting();
        }
        private void OnDestroy()
        {
            StartWave.Start -= InitWave;
            EndGame.GameOver -= GameOver;
        }

        private void InitWave()
        {
            S_INITWAVE = true;
        }
        private void GameOver()
        {
            _waveState = WaveState.GameOver;
        }

        private void Update()
        {
            if (S_INITWAVE)
            {
                _waveCountdown = 0f;
            }
            switch (_waveState)
            {
                case WaveState.CountingDown:
                    Countdown();
                    break;
                case WaveState.Spawning:
                    Spawning();
                    break;
                case WaveState.Finished:
                    WaveFinished();
                    break;
            }
            UpdateWaveCountdownUI?.Invoke(Mathf.CeilToInt(_waveCountdown));
        }
        private void WaveFinished()
        {
            if (_gameOver) return;
            _gameOver = true;
            S_INITWAVE = false;
            AllWavesCleared?.Invoke();
            Debug.Log("Wave Finished");
        }
        private void Countdown()
        {
            UpdateWaveIndex?.Invoke(_currentWaveIndex);
            _waveCountdown -= Time.deltaTime;
            if(_waveCountdown <= 5f && !_audioPlayed)
            {
                if (SceneManager.GetActiveScene().name != "Main Menu")
                {
                    AudioManager.Instance?.Play("WaveStarting");
                }
                _audioPlayed= true;
            }
            if (_waveCountdown <= 0f)
            {
                _waveState = WaveState.Spawning;
            }
        }
        private void Spawning()
        { 
            var wave = _waves[_currentWaveIndex];

            if (wave.AllEnemiesSpawned())
            {
                if (FindObjectsOfType<EnemyController>().Length == 0)
                {
                    Debug.Log("going to next wave");
                    _currentWaveIndex++;
                    S_INITWAVE = false;
                    _waveCountdown = wave.TimeForWaveToSpawn;
                    _audioPlayed = false;
                    if(_currentWaveIndex == _waves.Count - 1)
                    {
                        AudioManager.Instance.Play("LastWave");
                    }
                    ResetPottedPlants?.Invoke();
                    _waveState = _currentWaveIndex == _waves.Count ? WaveState.Finished : WaveState.CountingDown;

                }
                return;
            }
            wave.SpawnRatePerSecond -= Time.deltaTime;
            if (wave.SpawnRatePerSecond <= 0f)
            {
                GameObject enemy = Instantiate(wave.EnemyToSpawn, transform);
                wave.ResetSpawnRate();
                EnemyController enemyController = enemy.GetComponent<EnemyController>();
                enemyController.SetSpeed(wave.EnemySpeed);
                enemyController.SetHealth(wave.EnemyHealth);
                enemyController.SetDropChance(wave.DropSeedChance);
                enemy.transform.position = _spawnPos[UnityEngine.Random.Range(0, _spawnPos.Count)].position;
                wave.AddEnemySpawned();
            }

        }
        public void Reseting()
        {
            S_INITWAVE = false;
            _waveCountdown = _ogWaveCountdown;
            _waveState = WaveState.CountingDown;
            _currentWaveIndex = 0;
        }

        [Serializable]
        public class WaveData
        {
            [field: SerializeField] public int Count { get; private set; }
            [field: SerializeField] public float SpawnRatePerSecond { get; set; }
            [field: SerializeField] public GameObject EnemyToSpawn { get; private set; }
            [field: SerializeField] public float EnemySpeed { get; private set; }
            [field: SerializeField] public int EnemyHealth { get; private set; }
            [field: SerializeField] public float TimeForWaveToSpawn { get; private set; }
            [Range(0, 100)] public float DropSeedChance;

            private float _ogSpawnRatePerSecond;
            public void ResetSpawnRate()
            {
                SpawnRatePerSecond = _ogSpawnRatePerSecond;
            }
            public void SetSpawnRate()
            {
                _ogSpawnRatePerSecond = SpawnRatePerSecond;
            }
            public void AddEnemySpawned()
            {
                Count--;
            }
            public bool AllEnemiesSpawned()
            {
                return Count == 0;
            }
        }
    }
}

