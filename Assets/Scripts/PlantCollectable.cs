using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantCollectable : MonoBehaviour
{
    [SerializeField] private SeedData _seedType;
    public static event Action<SeedData> UpdateUI;
    public void OnCollected()
    {
        UpdateUI?.Invoke(_seedType);
        Destroy(gameObject);
    }
    public void SetSeedType(SeedData seedType)
    {
        _seedType = seedType;
    }
}
