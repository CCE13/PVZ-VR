using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BasePlant : MonoBehaviour, IAttackable
{
    [SerializeField] private int _health;
    public void OnAttacked()
    {
        _health--;
        if (_health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
