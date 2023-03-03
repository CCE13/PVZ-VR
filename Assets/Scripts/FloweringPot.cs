
using Enemy;
using Peashooters;
using Spawner;
using UnityEngine;
using UnityEngine.UI;

namespace SeedPlanting
{
    public class FloweringPot : MonoBehaviour,IAttackable
    {
        [SerializeField] private Image _countdown;
        [SerializeField] private BoxCollider _deathTrigger;
        private float _timeForSeedToGrow;
        private SeedData _seedPlanted;
        private bool _attacked;

        private Vector3 _localStartPos;

        private void Start()
        {
            _countdown.gameObject.SetActive(false);
            _localStartPos = transform.localPosition;
            EnemySpawner.ResetPottedPlants += ResetPlant;
        }
        private void OnDestroy()
        {
            EnemySpawner.ResetPottedPlants -= ResetPlant;
        }
        public void PlantSeed()
        {
            if (!SeedManager._currentSeedSelected) return;
            if (_seedPlanted != null) return;
            _seedPlanted = SeedManager._currentSeedSelected;
            SeedManager.UpdateCount();
            _timeForSeedToGrow = _seedPlanted.TimeItTakesToSpawnPlant;
            AudioManager.Instance.Play("PlantingSeed");
            _countdown.gameObject.SetActive(true);
        }
        private void ResetPlant()
        {
            transform.localPosition = _localStartPos;
            _attacked = false;
            _seedPlanted = null;
            _countdown.gameObject.SetActive(false);
            if (transform.childCount >= 3)
            {
                Destroy(transform.GetChild(2).gameObject);
            }
            
        }

        private void Update()
        {
            if (_attacked) transform.position += Vector3.forward * 0.2f;
            if (!_seedPlanted) return;
            _timeForSeedToGrow -= Time.deltaTime;
            _countdown.fillAmount = _timeForSeedToGrow / _seedPlanted.TimeItTakesToSpawnPlant;
            if (_timeForSeedToGrow <= 0)
            {
                GameObject plant = Instantiate(_seedPlanted.PlantToCollectOnPlanting, PositionToSpawn(_seedPlanted.PlantToCollectOnPlanting), Quaternion.identity);
                AudioManager.Instance.Play("PlantGrowth");
               plant.transform.SetParent(transform);
                if(null != plant.GetComponent<PeaShooter>()) plant.GetComponent<PeaShooter>().enabled = false;
                if(null != plant.GetComponent<PeaShooterGun>()) plant.GetComponent<PeaShooterGun>().enabled = false;
                plant.GetComponent<PlantCollectable>().SetSeedType(_seedPlanted);
                _seedPlanted = null;
                _countdown.gameObject.SetActive(false);
            }
        }
        public Vector3 PositionToSpawn(GameObject plant)
        {
            return new Vector3(transform.position.x, plant.transform.localScale.y / 2 + transform.lossyScale.y / 2 + transform.position.y, transform.position.z);
        }
        public void OnAttacked()
        {
            if (_attacked) return;
            _attacked = true;
            _deathTrigger.enabled = true;
            AudioManager.Instance.Play("FlowerPotZoom");
            
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Enemy"))
            {
                other.GetComponent<EnemyController>().LoseHeath(true);
            }
        }
    }
}

