using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Spawner
{ 
    public class ObjectSpawner : MonoBehaviour
    {
        [SerializeField] private SeedData _seedDataToCompareTo;
        [SerializeField] private int _objectCount;
        [SerializeField] private TMP_Text _plantCounter;
        [SerializeField] private Image _plantTitle;

        private Image _image;
        private Button _button;

        private void Awake()
        {
            TryGetComponent(out _image);
            TryGetComponent(out _button);
        }
        private void Start()
        {
            PlantCollectable.UpdateUI += UpdateUI;
            BuildManager.UpdateUI += LoseObjectCount;
        }
        private void OnDestroy()
        {
            PlantCollectable.UpdateUI -= UpdateUI;
            BuildManager.UpdateUI -= LoseObjectCount;
        }
        private void LateUpdate()
        {
            if(_objectCount > 0)
            {
                _button.enabled = true;
                _image.enabled = true;
                _plantCounter.enabled = true;
                _plantTitle.enabled = true;
            }
            else
            {
                _button.enabled = false;
                _image.enabled = false;
                _plantCounter.enabled = false;
                _plantTitle.enabled = false;
            }
        }
        public void SetObjectToSpawn()
        {
            BuildManager.instance.ObjectToSpawn = _seedDataToCompareTo;
        }
        private void UpdateUI(SeedData seedData)
        {
            if (seedData.seedName!= _seedDataToCompareTo.seedName) return;
            _objectCount++;
            _plantCounter.text = _objectCount.ToString();
        }
        private void LoseObjectCount(SeedData seedData)
        {
            if (seedData.seedName!= _seedDataToCompareTo.seedName) return;
            _objectCount--;
            _plantCounter.text = _objectCount.ToString();
        }
    }

}

