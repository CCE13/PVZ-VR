using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace SeedPlanting 
{
    public class Seeds : MonoBehaviour
    {
        [SerializeField] private SeedData _seedData;

        private GameObject _seedpacketObject;
        public static event Action<SeedData> ShowOnUI;

        private void Awake()
        {
            _seedpacketObject = transform.GetChild(0).gameObject;
        }
        private void Start()
        {
            SeedData type = _seedData;
            _seedData = Instantiate(type);
        }
        private void LateUpdate()
        {
            transform.LookAt(Camera.main.transform);
        }
        public void OnCollected()
        {
            ShowOnUI?.Invoke(_seedData);

            //Disable the seed packet rendererObject
            _seedpacketObject.SetActive(false);
        }
    }
}


