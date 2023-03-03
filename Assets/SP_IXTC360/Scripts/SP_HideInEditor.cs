using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[ExecuteInEditMode]

public class SP_HideInEditor : MonoBehaviour
{
    private void OnEnable()
    {
        this.gameObject.hideFlags = HideFlags.HideInHierarchy;
    }
}
