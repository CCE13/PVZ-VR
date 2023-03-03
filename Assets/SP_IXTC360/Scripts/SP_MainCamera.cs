using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;


public class SP_MainCamera : MonoBehaviour
{
    public delegate void HotSpotClick(GameObject gameObject);
    public static event HotSpotClick HotSpotClicked;
    private static SP_MainCamera _instance;
    public bool filterRaycast = false;
    public static SP_MainCamera Instance { get { return _instance; } }
    public GameObject InformationPanel, MCQPanel;

  
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
        InformationPanel.SetActive(false);
    }


    void PerformRayTrace()
    {
        
        Camera cam = this.gameObject.GetComponent<Camera>();
        if (cam)
        {
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            float Scale_Distance = 0.0f;
            if(SP_DataCapturing.Instance)
            {
                Scale_Distance = SP_DataCapturing.Instance.m_CurrentSphereWrapper.sphereObj.transform.lossyScale.x;

            }
            if (Physics.Raycast(ray, out hit, Scale_Distance))
            {
                GameObject hitObj = hit.transform.gameObject;
                SP_Hotspot hotspot = hitObj.GetComponent<SP_Hotspot>();
                SP_MCQ mcqhotspot = hitObj.GetComponent<SP_MCQ>();
                if (hotspot)
                {
                    if (Input.GetMouseButtonDown(0) && !hitObj.GetComponent<SP_Status>() && HotSpotClicked != null) HotSpotClicked(hitObj);
                    InformationPanel.SetActive(true);
                    Vector3 screenPos = cam.WorldToScreenPoint(hitObj.transform.position);
                    InformationPanel.transform.position = screenPos;
                    SP_HotspotDisplayInformation dsp = InformationPanel.GetComponent<SP_HotspotDisplayInformation>();
                    if (dsp)
                    {
                        if (hotspot.myInfo.displayImage)
                        {
                            dsp.hotspotImage.texture = hotspot.myInfo.displayImage.texture;
                        }

                        dsp.hotspotText.text = hotspot.myInfo.displayText;
                    }
                }
                else if (mcqhotspot)
                {
                    if (!MCQPanel.activeSelf)
                    {
                        MCQPanel.SetActive(true);
                        Vector3 screenPos = cam.WorldToScreenPoint(hitObj.transform.position);
                        MCQPanel.transform.position = screenPos;
                        SP_MCQDisplayInformation mdsp = MCQPanel.GetComponent<SP_MCQDisplayInformation>();
                        if (mdsp)
                        {
                            mdsp.Question.text = mcqhotspot.spMCQInfo.Question;
                            mdsp.CorrectAnswers = mcqhotspot.spMCQInfo.Answers;
                            mdsp.SetAnswerOptions(mcqhotspot.spMCQInfo.Options, hitObj);
                        }
                    }
                }
            }
            else
            {
                if (Input.GetKey(KeyCode.LeftAlt))
                {
                    if (MCQPanel.activeSelf)
                    {
                        MCQPanel.GetComponent<SP_MCQDisplayInformation>().OffMCQ();
                        MCQPanel.SetActive(false);
                    }
                }
                InformationPanel.SetActive(false);
            }
        }

    
    }


    // Update is called once per frame
    void Update()
    {
        if (filterRaycast)
        {
            InformationPanel.SetActive(false);
            return;
        }
        PerformRayTrace();

    }
}
