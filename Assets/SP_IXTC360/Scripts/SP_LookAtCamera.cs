using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SP_LookAtCamera : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        if (SP_MainCamera.Instance)
        {
            GameObject mainCameraObj = SP_MainCamera.Instance.gameObject;
            this.transform.LookAt(mainCameraObj.transform);
        }
    }
}
