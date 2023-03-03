using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SP_ShereDataCollection : MonoBehaviour
{
    private void Start()
    {
        if (SP_DataCapturing.Instance)
        {
            if (this.gameObject.activeSelf)
            {
                SP_DataCapturing.Instance.AddSphereToCollection(this.gameObject);
            }
        }
    }

    private void OnDisable()
    {
        if (SP_DataCapturing.Instance)
        {
            SP_DataCapturing.Instance.RemoveSphereFromCollection(this.gameObject.GetInstanceID());
        }
    }

    private void OnEnable()
    {
        if (SP_DataCapturing.Instance)
        {
            if (this.gameObject.activeSelf)
            {
                SP_DataCapturing.Instance.AddSphereToCollection(this.gameObject);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}
