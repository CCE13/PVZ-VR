using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.GraphicsBuffer;


namespace Spawner
{
    public class BuildManager : MonoBehaviour
    {
        public static BuildManager instance;
        public SeedData ObjectToSpawn;
        public int PeaShooterCount
        {
            get; private set;
        }
        public static event Action<SeedData> UpdateUI;
        private void Awake()
        {
            instance = this;
        }
        public GameObject SpawnObject(Node node)
        {
            GameObject plant = Instantiate(ObjectToSpawn.PlantToSpawn,node.PositionToSpawn(ObjectToSpawn.PlantToSpawn),Quaternion.identity);
            UpdateUI?.Invoke(ObjectToSpawn);
            ObjectToSpawn = null;
            plant.transform.SetParent(node.transform);
            return plant;
        }


    }
}

