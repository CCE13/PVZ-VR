using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SP_InitSpheres : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject spPlayer = GameObject.Find("SP_Player");
        SP_Sphere[] spheres = GameObject.FindObjectsOfType<SP_Sphere>();
        for(int i=0; i< spheres.Length; i++)
        {
            if (Vector3.Distance(spheres[i].transform.position, spPlayer.transform.position) == 0)
            {
                spheres[i].gameObject.SetActive(true);
            }
            //else spheres[i].gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
