using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SeedData")]
public class SeedData : ScriptableObject
{
    public string seedName;
    public GameObject PlantToSpawn;
    public GameObject PlantToCollectOnPlanting;
    public Sprite SeedSprite;
    public int TimeItTakesToSpawnPlant;
    public int numberOfSeeds;
}
