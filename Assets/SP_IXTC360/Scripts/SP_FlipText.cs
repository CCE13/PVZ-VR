using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SP_FlipText : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Vector3 scale = transform.localScale;
        scale.x = -1* scale.x;
        transform.localScale = scale;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
