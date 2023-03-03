using UnityEngine;
using System.Collections;
using UnityEditor;

[ExecuteInEditMode]
public class SP_TeleportBuilderScript : MonoBehaviour
{
    public SP_Sphere TargetSphere;
    private GameObject childObj;

    private void OnEnable()
    {
        childObj = this.transform.GetChild(0).gameObject;
        
    }
    public void LinkObject()
    {

        //Instantiate(obj, spawnPoint, Quaternion.identity);
    }

    public void Teleport()
    {
        if (TargetSphere)
        {
            //TargetSphere.gameObject.SetActive(true);
            SP_Sphere sphr = TargetSphere.GetComponent<SP_Sphere>();
            if (sphr)
            {
                sphr.Teleport();
                //transform.parent.gameObject.SetActive(false);
            }
        }
    }

    private void Update()
    {  
        if (childObj)
        {
            SpriteRenderer sr = childObj.GetComponent<SpriteRenderer>();
            if (sr)
            {
                if (!TargetSphere)
                {
                    sr.color = Color.red;
                }
                else
                {
                    sr.color = Color.white;
                }
            }
        }
        if (TargetSphere)
        {

            //Handles.DrawLine(myScript.transform.position, myScript.TargetSphere.transform.position);
            Vector3 dir = ( transform.position - TargetSphere.gameObject.transform.position ).normalized * (TargetSphere.gameObject.transform.lossyScale.x * 0.65f);
            Vector3 targetPos = TargetSphere.gameObject.transform.position + dir;
            targetPos.y = transform.position.y;
            Debug.DrawLine(transform.position, targetPos, Color.green);
        }

        
    }

}


