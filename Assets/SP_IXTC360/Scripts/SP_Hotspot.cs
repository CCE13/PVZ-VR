using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

[System.Serializable]
public class SP_HotspotInformation
{
    public Sprite displayImage;
    [TextArea(0,4)]public string displayText;
    public string hotspotName = "Demo";
}

public class SP_Hotspot : MonoBehaviour
{
    public float recordingTreshold = 0.35f;
    public bool HideMeshOnStart = true;
    public SP_HotspotInformation myInfo = new SP_HotspotInformation();
    public float ActiveTime = 0.0f;

    // Start is called before the first frame update
    void Start()
    {

        if (HideMeshOnStart)
        {
            this.GetComponent<MeshRenderer>().enabled = false;
        }

        if (SP_DataCapturing.Instance)
        {
            if (this.gameObject.activeSelf)
            {
                SP_HotspotManager.Instance.AddHotSpotToCollection(this.gameObject);
            }
        }
    }

    public SP_HotspotInformation RetriveHotspotInformation()
    {
        return myInfo;
    }


    // Update is called once per frame
    void Update()
    {
        if (SP_MainCamera.Instance)
        {
            Camera cam = SP_MainCamera.Instance.gameObject.GetComponent<Camera>();
            if (cam)
            {
                float distance = Vector3.Distance(this.gameObject.transform.position, cam.transform.position);

                if (SP_DataCapturing.Instance)
                {

                    //Get the scale of the current active sphere
                    if (SP_DataCapturing.Instance.m_CurrentSphereWrapper == null)
                    {
                        return;
                    }
                    GameObject currentActiveSphere = SP_DataCapturing.Instance.m_CurrentSphereWrapper.sphereObj;
       
                    float activeSphereScale = currentActiveSphere.transform.lossyScale.x;
                    Vector3 screenPos = cam.WorldToScreenPoint(this.transform.position);
                    //Check if hotspot is within the sphere
                    if ((distance < activeSphereScale) && (screenPos.z > 0.0f))
                    {

                        float screenCenterWidth = Screen.width * 0.5f;
                        float screenCenterHeight = Screen.height * 0.5f;

                        bool withinX = false;
                        bool insideTreshold = false;
                        if ((screenPos.x > (screenCenterWidth - (screenCenterWidth * recordingTreshold))) &&
                            (screenPos.x < (screenCenterWidth + (screenCenterWidth * recordingTreshold))))
                        {
                            withinX = true;
                        }

                        if (withinX)
                        {
                            if ((screenPos.y > (screenCenterHeight - (screenCenterHeight * recordingTreshold))) &&
                                (screenPos.y < (screenCenterHeight + (screenCenterHeight * recordingTreshold))))
                            {
                                insideTreshold = true;
                            }
                        }

                        if (insideTreshold)
                        {
                            Debug.Log(this.gameObject.name + " Inside Treshold");
                            ActiveTime += Time.deltaTime;
                        }
                    }
                }
            }
        }
    }
}

