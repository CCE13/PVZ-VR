using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SP_PlayerManager : MonoBehaviour
{
    private static SP_PlayerManager _instance;
    public static SP_PlayerManager Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }
}
