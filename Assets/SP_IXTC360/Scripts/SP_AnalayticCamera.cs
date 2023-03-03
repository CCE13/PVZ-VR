using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SP_AnalayticCamera : MonoBehaviour
{
    private static SP_AnalayticCamera _instance;
    public static SP_AnalayticCamera Instance { get { return _instance; } }


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


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //foreach (KeyValuePair<int, GameObject> hotspot in m_HotSpotCollection)
        //{
        //    Now you can access the key and value both separately from this attachStat as:
        //    Debug.Log(hotspot.Key);
        //    Debug.Log(hotspot.Value);
        //}
    }
}
