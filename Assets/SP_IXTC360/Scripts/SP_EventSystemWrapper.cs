using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class SP_EventSystemWrapper : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.LeftControl))
        {
            if (!this.GetComponent<GvrPointerInputModule>().enabled)
            {
                this.GetComponent<GvrPointerInputModule>().enabled = true;
            }

        }
        else
        {
            if (this.GetComponent<GvrPointerInputModule>().enabled)
            {
                this.GetComponent<GvrPointerInputModule>().enabled = false;
            }
        }
    }
}
